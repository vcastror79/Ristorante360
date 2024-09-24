namespace Ristorante360.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int SuppliesId { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }

        // Propiedad de navegación para acceder al objeto de Inventario asociado
        public Inventory Inventory { get; set; }
    }
}
