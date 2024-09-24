using System.ComponentModel.DataAnnotations;

namespace Ristorante360.Models.ViewModels
{
    public class UserLoginVM
    {
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Por favor, ingresa una dirección de correo electrónico válida.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Password { get; set; } = null!;

    }
}
