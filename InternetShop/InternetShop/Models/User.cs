using Microsoft.AspNetCore.Identity;

namespace InternetShop.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public ICollection<Order>? Orders { get; set; }
    }
}