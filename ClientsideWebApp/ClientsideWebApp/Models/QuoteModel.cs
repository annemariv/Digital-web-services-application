using System.ComponentModel.DataAnnotations;

namespace ClientsideWebApp.Models
{
    public class QuoteModel
    {
        [Required(ErrorMessage = "Palun vali soovitud teenus")]
        //Arbitrary strings can't be injected as the service
        [RegularExpression("^(web|design|accounting|consultation)$",ErrorMessage = "Vigane teenus")]
        public string Service { get; set; } = string.Empty;

        [Required(ErrorMessage = "Palun lisa oma nimi")]
        [MaxLength(100, ErrorMessage = "Nimi on liiga pikk")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Palun lisa e-post")]
        [EmailAddress(ErrorMessage = "Palun sisesta korrektne e-post")]
        [MaxLength(254)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Kirjeldus ei tohi olla üle 1000 tähemärgi")]
        public string? Description { get; set; }
    }
}
