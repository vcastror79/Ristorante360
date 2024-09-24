using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ristorante360.Models;

public partial class Error
{
   
    public int ErrorId { get; set; }

    public string? ExceptionMessage { get; set; }
    public string ErrorMessage { get; set; }


    public DateTime? DateTime { get; set; }
}
