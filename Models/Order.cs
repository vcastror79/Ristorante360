using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime OrderDate { get; set; }

    public double? TotalAmount { get; set; }

    public int? ClientId { get; set; }

    public int OrderStatusId { get; set; }

    public int UserId { get; set; }

    public int? OrderTypeId { get; set; }

    public int? PaymentMethodId { get; set; }

    public string? OrderSpecifications { get; set; }
    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual OrderType OrderType { get; set; } = null!;

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
