using ClientsideWebApp.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace ClientsideWebApp.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<QuoteController> _logger;

        public QuoteController(IConfiguration config, ILogger<QuoteController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(HomeViewModel viewModel)
        {
            var model = viewModel.Quote;

            if (!ModelState.IsValid)
            {
                viewModel.Services = ServiceDataModel.GetAll();
                return View("~/Views/Home/Index.cshtml", viewModel);
            }

            try
            {
                var host = _config["Smtp:Host"] ?? throw new InvalidOperationException("Smtp:Host missing");
                var port = int.Parse(_config["Smtp:Port"] ?? "587");
                var user = _config["Smtp:User"] ?? throw new InvalidOperationException("Smtp:User missing");
                var pass = _config["Smtp:Pass"] ?? throw new InvalidOperationException("Smtp:Pass missing");
                var toEmail = _config["Smtp:ToEmail"] ?? throw new InvalidOperationException("Smtp:ToEmail missing");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Hinnapäring", user));
                message.To.Add(new MailboxAddress("", toEmail));
                message.ReplyTo.Add(new MailboxAddress(model.Name, model.Email));
                message.Subject = $"{model.Service} hinnapäring";
                message.Body = new TextPart("plain")
                {
                    Text = $"Nimi: {model.Name}\n" +
                           $"E-post: {model.Email}\n" +
                           $"Teenus: {model.Service}\n" +
                           $"Kirjeldus: {model.Description}"
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(user, pass);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                TempData["Success"] = "Päring on saadetud!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send quote email");
                ModelState.AddModelError("", "Päringu saatmine ebaõnnestus. Palun proovi hiljem uuesti.");
                viewModel.Services = ServiceDataModel.GetAll();
                return View("~/Views/Home/Index.cshtml", viewModel);
            }
        }
    }
}