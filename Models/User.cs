using System.ComponentModel.DataAnnotations;

namespace Ristorante360Admin.Models;

public partial class User
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El campo Nombre debe tener entre 2 y 20 caracteres.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "El campo Apellidos debe tener entre 2 y 20 caracteres.")]
    public string Surname { get; set; } = null!;
    [Required(ErrorMessage = "Este campo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Por favor, ingresa una dirección de correo electrónico válida.")]
    public string Email { get; set; } = null!;

    public bool Status { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio.")]

    public int RoleId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$", ErrorMessage = "La contraseña debe contener al menos una minúscula, una mayúscula y un número.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public string Password { get; set; } = null!;

    public string? TokenRecovery { get;set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role oRole { get; set; } = null!;
}
