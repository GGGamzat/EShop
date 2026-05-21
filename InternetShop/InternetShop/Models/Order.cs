using System.ComponentModel.DataAnnotations;

namespace InternetShop.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string ShippingAddress { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public User? User { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}