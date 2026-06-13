using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InternetShop.Models;

namespace InternetShop.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связей
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Добавление продуктов питания
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Яблоки Гренни Смит",
                    Description = "Свежие, сочные яблоки с кисло-сладким вкусом. Урожай 2024 года.",
                    Price = 150,
                    Stock = 100,
                    Category = "Фрукты",
                    Unit = "кг",
                    ExpiryDate = DateTime.Now.AddDays(30),
                    Manufacturer = "Local Farm",
                    Calories = 52,
                    IsOrganic = true,
                    StorageConditions = "Хранить в холодильнике при +2-4°C",
                    ImageUrl = "/images/apples.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Картофель молодой",
                    Description = "Молодой картофель, выращенный в экологически чистых условиях",
                    Price = 80,
                    Stock = 200,
                    Category = "Овощи",
                    Unit = "кг",
                    ExpiryDate = DateTime.Now.AddDays(14),
                    Manufacturer = "Фермерское хозяйство",
                    Calories = 77,
                    IsOrganic = true,
                    StorageConditions = "Темное сухое место при +5-10°C",
                    ImageUrl = "/images/potato.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Куриное филе",
                    Description = "Свежее куриное филе без кости и кожи. Охлажденное.",
                    Price = 350,
                    Stock = 50,
                    Category = "Мясо",
                    Unit = "кг",
                    ExpiryDate = DateTime.Now.AddDays(5),
                    Manufacturer = "Птицефабрика №1",
                    Calories = 165,
                    IsOrganic = false,
                    StorageConditions = "Хранить при температуре от 0 до +4°C",
                    ImageUrl = "/images/chicken.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Молоко 3.2%",
                    Description = "Пастеризованное молоко высшего качества",
                    Price = 89,
                    Stock = 150,
                    Category = "Молочные продукты",
                    Unit = "л",
                    ExpiryDate = DateTime.Now.AddDays(7),
                    Manufacturer = "Молочная ферма",
                    Calories = 60,
                    IsOrganic = false,
                    StorageConditions = "Хранить при температуре от +2 до +6°C",
                    ImageUrl = "/images/milk.jpg"
                },
                new Product
                {
                    Id = 5,
                    Name = "Хлеб ржаной",
                    Description = "Свежевыпеченный ржаной хлеб на закваске",
                    Price = 55,
                    Stock = 30,
                    Category = "Хлебобулочные",
                    Unit = "шт",
                    ExpiryDate = DateTime.Now.AddDays(3),
                    Manufacturer = "Хлебозавод",
                    Calories = 210,
                    IsOrganic = false,
                    StorageConditions = "Хранить при комнатной температуре",
                    ImageUrl = "/images/bread.jpg"
                },
                new Product
                {
                    Id = 6,
                    Name = "Лосось слабосоленый",
                    Description = "Филе лосося слабой соли, вакуумная упаковка",
                    Price = 850,
                    Stock = 25,
                    Category = "Рыба",
                    Unit = "кг",
                    ExpiryDate = DateTime.Now.AddDays(14),
                    Manufacturer = "Рыбный промысел",
                    Calories = 208,
                    IsOrganic = true,
                    StorageConditions = "Хранить при температуре от -2 до +2°C",
                    ImageUrl = "/images/salmon.jpg"
                },
                new Product
                {
                    Id = 7,
                    Name = "Помидоры черри",
                    Description = "Сладкие помидоры черри на ветке",
                    Price = 220,
                    Stock = 80,
                    Category = "Овощи",
                    Unit = "кг",
                    ExpiryDate = DateTime.Now.AddDays(10),
                    Manufacturer = "Тепличный комплекс",
                    Calories = 18,
                    IsOrganic = true,
                    StorageConditions = "Хранить в холодильнике",
                    ImageUrl = "/images/tomatoes.jpg"
                },
                new Product
                {
                    Id = 8,
                    Name = "Сыр Пармезан",
                    Description = "Твердый итальянский сыр 24 месяца выдержки",
                    Price = 1200,
                    Stock = 40,
                    Category = "Молочные продукты",
                    Unit = "кг",
                    ExpiryDate = DateTime.Now.AddMonths(6),
                    Manufacturer = "Italy Cheese",
                    Calories = 431,
                    IsOrganic = false,
                    StorageConditions = "Хранить в холодильнике при +4-8°C",
                    ImageUrl = "/images/cheese.jpg"
                },
                new Product
                {
                    Id = 9,
                    Name = "Мед цветочный",
                    Description = "Натуральный цветочный мед без добавок",
                    Price = 450,
                    Stock = 60,
                    Category = "Бакалея",
                    Unit = "кг",
                    ExpiryDate = DateTime.Now.AddYears(2),
                    Manufacturer = "Пасека",
                    Calories = 304,
                    IsOrganic = true,
                    StorageConditions = "Хранить при комнатной температуре",
                    ImageUrl = "/images/honey.jpg"
                },
                new Product
                {
                    Id = 10,
                    Name = "Шпинат свежий",
                    Description = "Молодой свежий шпинат в упаковке",
                    Price = 120,
                    Stock = 45,
                    Category = "Зелень",
                    Unit = "100г",
                    ExpiryDate = DateTime.Now.AddDays(5),
                    Manufacturer = "Green Farm",
                    Calories = 23,
                    IsOrganic = true,
                    StorageConditions = "Хранить в холодильнике при +2-4°C",
                    ImageUrl = "/images/spinach.jpg"
                }
            );
        }
    }
}