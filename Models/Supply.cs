namespace Ristorante360.Models;

public partial class Supply
{
    public int SuppliesId { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<SuppliesForProduct> SuppliesForProducts { get; set; } = new List<SuppliesForProduct>();
}
