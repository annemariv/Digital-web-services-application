using System.ComponentModel.DataAnnotations;

namespace AdminsideWebApp.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Palun täida väli.")]
        [Display(Name = "Kasutajanimi")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Uus parool")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Korrake parooli")]
        [Compare(nameof(NewPassword), ErrorMessage = "Paroolid ei kattu.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Palun täida väli.")]
        [DataType(DataType.Password)]
        [Display(Name = "Sinu praegune parool (kinnitamiseks)")]
        public string? AdminCurrentPassword { get; set; }
    }
}