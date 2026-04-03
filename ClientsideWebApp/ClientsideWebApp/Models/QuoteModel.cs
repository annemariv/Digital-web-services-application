using System.ComponentModel.DataAnnotations;

namespace ClientsideWebApp.Models
{
    public class QuoteModel
    {
        [Required(ErrorMessage = "Palun vali soovitud teenus")]
        public string Service { get; set; }

        [Required(ErrorMessage = "Palun lisa oma nimi")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Palun lisa e-post")]
        [EmailAddress(ErrorMessage = "Palun sisesta korrektne e-post")]
        public string Email { get; set; }

        [MaxLength(1000, ErrorMessage = "Kirjeldus ei tohi olla üle 1000 tähemärgi")]
        public string Description { get; set; }
    }
}
