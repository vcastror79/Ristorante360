using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ristorante360Admin.Models;

[Table("Unit_Type")] // Nombre exacto de la tabla en la base de datos

public partial class UnitType
{
    public int UnitId { get; set; }

    public string? Unit { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<SuppliesForProduct> SuppliesForProducts { get; set; } = new List<SuppliesForProduct>();
}
