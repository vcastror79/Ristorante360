using System.ComponentModel.DataAnnotations;

namespace Ristorante360.Models.ViewModels
{
    public class RecoveryPasswordViewModel
    {
        public string token { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", ErrorMessage = "La contraseña debe contener al menos una minúscula, una mayúscula y un número.")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "La contraseña no coincide")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", ErrorMessage = "La contraseña debe contener al menos una minúscula, una mayúscula y un número.")]
        [Required]
        public string ConfirmPassword { get; set;}

    }
}
