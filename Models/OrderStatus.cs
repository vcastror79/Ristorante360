using System;
using System.Collections.Generic;

namespace Ristorante360.Models;

public partial class OrderStatus
{
    public int OrderStatusId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
