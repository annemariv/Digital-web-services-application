using System.ComponentModel.DataAnnotations;

namespace ClientsideWebApp.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "Palun vali soovitud teenus")]
        public string Service { get; set; }

        [Required(ErrorMessage = "Palun lisa oma nimi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Palun lisa e-post")]
        [EmailAddress(ErrorMessage = "Palun sisesta korrektne e-post")]
        public string Email { get; set; }

        public string Description { get; set; }
    }
}
