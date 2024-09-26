using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
