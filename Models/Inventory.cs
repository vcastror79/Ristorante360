﻿using Ristorante360Admin.Models;
using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

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

    // Cambiar List<Notification> a ICollection<Notification>
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}



