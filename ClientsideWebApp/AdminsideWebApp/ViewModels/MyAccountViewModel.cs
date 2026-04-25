using System.ComponentModel.DataAnnotations;

namespace AdminsideWebApp.ViewModels
{
    public class MyAccountViewModel
    {
        [Required(ErrorMessage = "Palun täida väli.")]
        [Display(Name = "Kasutajanimi")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Praegune parool")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Uus parool")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Korrake parooli")]
        [Compare(nameof(NewPassword), ErrorMessage = "Paroolid ei kattu.")]
        public string? ConfirmPassword { get; set; }
    }
}