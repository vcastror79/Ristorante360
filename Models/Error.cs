using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

public partial class Error
{
    public int ErrorId { get; set; }

    public DateTime DateTime { get; set; }

    public string? ErrorMessage { get; set; }

    public string? ExceptionMessage { get; set; }
}
