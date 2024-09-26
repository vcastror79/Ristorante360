using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

public partial class SuppliesForProduct
{
    public int SuppliesProductId { get; set; }

    public int SuppliesId { get; set; }

    public int ProductId { get; set; }
    public int UnitId { get; set; }

    public int Quantity { get; set; }

    public virtual Product oProduct { get; set; } = null!;

    public virtual Supply oSupplies { get; set; } = null!;

    public virtual UnitType oUnitType { get; set; } = null!;
}
