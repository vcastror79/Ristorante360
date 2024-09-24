using System.ComponentModel.DataAnnotations;

namespace Ristorante360.Models.ViewModels
{
    public class RecoveryViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "El correo electrónico es requerido para realizar la recuperación.")]
        public string? Email { get; set; }
    }
}
