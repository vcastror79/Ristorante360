using System;
using System.Collections.Generic;

namespace Ristorante360.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int SuppliesId { get; set; }

    public int Amount { get; set; }

    public int UnitId { get; set; }
    public int Lote { get; set; }

    public int MinimumAmount { get; set; }

    public DateTime AdmissionDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public bool Status { get; set; }
    public virtual UnitType oUnitType { get; set; } = null!;

    public virtual Supply oSupplies { get; set; } = null!;
    public List<Notification> Notifications { get; set; }

}
