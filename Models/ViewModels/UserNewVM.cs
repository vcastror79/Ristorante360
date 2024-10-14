using System.ComponentModel.DataAnnotations;

namespace Ristorante360Admin.Models.ViewModels
{
    public class UserNewVM
    {
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Por favor, ingresa una dirección de correo electrónico válida.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [DataType(DataType.Password)] // Esto ayuda en la vista para que el campo se trate como una contraseña
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Debe confirmar su contraseña.")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)] // También debería ser tratado como un campo de contraseña
        public string ConfirmPassword { get; set; } = null!;

        public bool Status { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un rol válido.")]
        public int RoleId { get; set; }
    }
}

