using System.ComponentModel.DataAnnotations;

namespace InternetShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название товара")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите описание")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите цену")]
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Введите количество")]
        [Range(0, 10000)]
        public int Stock { get; set; }

        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}