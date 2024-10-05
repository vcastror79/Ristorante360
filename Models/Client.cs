using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public bool status { get; set; } // Agregar esta línea

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
