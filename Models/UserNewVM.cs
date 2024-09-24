using System.ComponentModel.DataAnnotations;

namespace Ristorante360.Models.ViewModels
{
    public class UserNewVM
    {
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Por favor, ingresa una dirección de correo electrónico válida.")]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; }

        [Compare("Password")]
        [Required]
        public string ConfirmPassword { get; set; }
        public bool Status { get; set; }

        public int RoleId { get; set; }
    }
}
