using ClientsideWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace ClientsideWebApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _config;

        public ContactController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Index() => View();

        //ToDo: AWS secret manager
        [HttpPost]
        public IActionResult Index(ContactModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var smtpSection = _config.GetSection("Smtp");
                var host = smtpSection["Host"];
                var port = int.Parse(smtpSection["Port"]);
                var user = smtpSection["User"];
                var pass = smtpSection["Pass"];
                var toEmail = smtpSection["ToEmail"];

                using var mail = new MailMessage(user, toEmail)
                {
                    Subject = $"{model.Service} hinnapäring",
                    Body = $"Nimi: {model.Name}\n" +
                           $"E-post: {model.Email}\n" +
                           $"Teenus: {model.Service}\n" +
                           $"Kirjeldus: {model.Description}"
                };

                using var smtpClient = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(user, pass),
                    EnableSsl = true
                };

                smtpClient.Send(mail);

                TempData["Success"] = "Päring on saadetud!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Päringu saatmine ebaõnnestus: " + ex.Message);
                return View(model);
            }
        }
    }
}
