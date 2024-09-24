using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ristorante360.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public double Price { get; set; }

    public bool Availability { get; set; }

    public string Image { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<SuppliesForProduct> SuppliesForProducts { get; set; } = new List<SuppliesForProduct>();

    public virtual Category oCategory { get; set; } = null!;

    [NotMapped]
    [DisplayName("Upload File")]
    public IFormFile ImageFile { get; set; }


}
