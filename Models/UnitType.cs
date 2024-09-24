using System;
using System.Collections.Generic;

namespace Ristorante360.Models;

public partial class UnitType
{
    public int UnitId { get; set; }

    public string Unit { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();
}
 