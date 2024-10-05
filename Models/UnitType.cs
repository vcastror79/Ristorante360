using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

public partial class UnitType
{
    public int UnitId { get; set; }

    public string? Unit { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<SuppliesForProduct> SuppliesForProducts { get; set; } = new List<SuppliesForProduct>();
}
