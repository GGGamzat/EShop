using System.ComponentModel.DataAnnotations;

namespace InternetShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название продукта")]
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

        // Новые поля для продуктов питания
        public string? Category { get; set; } // Овощи, Фрукты, Мясо, Молочка и т.д.
        public string? Unit { get; set; } // кг, шт, л, г
        public DateTime? ExpiryDate { get; set; } // Срок годности
        public string? Manufacturer { get; set; } // Производитель
        public int? Calories { get; set; } // Калории на 100г
        public bool IsOrganic { get; set; } = false; // Органический продукт
        public string? StorageConditions { get; set; } // Условия хранения

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}