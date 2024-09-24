using System;
using System.Collections.Generic;

namespace Ristorante360.Models;

public partial class OrderType
{
    public int OrderTypeId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
