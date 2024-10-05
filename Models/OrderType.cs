using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

public partial class OrderType
{
    public int OrderTypeId { get; set; }

    public string? Type { get; set; }
}
