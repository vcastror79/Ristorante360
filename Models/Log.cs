﻿using System;
using System.Collections.Generic;

namespace Ristorante360Admin.Models;

public partial class Log
{
    public int LogId { get; set; }

    public string? Detail { get; set; }

    public DateTime LogDate { get; set; }

    public string? Module { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
