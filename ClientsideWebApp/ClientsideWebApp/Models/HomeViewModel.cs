using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ClientsideWebApp.Models
{
    public class HomeViewModel
    {
        public QuoteModel Quote { get; set; } = new();
        [ValidateNever]
        public List<DigitalServiceModel> Services { get; set; } = new();
    }
}
