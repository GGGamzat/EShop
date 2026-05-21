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

            // Добавление начальных данных
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Ноутбук Lenovo ThinkPad",
                    Description = "Мощный ноутбук для работы и учебы. Процессор Intel Core i5, 16GB RAM, 512GB SSD",
                    Price = 65000,
                    Stock = 10,
                    Category = "Электроника",
                    ImageUrl = "/images/laptop.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Смартфон Xiaomi Mi 11",
                    Description = "Современный смартфон с отличной камерой 108MP, AMOLED экран",
                    Price = 35000,
                    Stock = 15,
                    Category = "Электроника",
                    ImageUrl = "/images/phone.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Наушники Sony WH-1000XM4",
                    Description = "Беспроводные наушники с шумоподавлением, время работы до 30 часов",
                    Price = 25000,
                    Stock = 8,
                    Category = "Аксессуары",
                    ImageUrl = "/images/headphones.jpg"
                },
                new Product
                {
                    Id = 4,
                    Name = "Мышь Logitech MX Master 3",
                    Description = "Беспроводная мышь для профессионалов, эргономичный дизайн",
                    Price = 8000,
                    Stock = 20,
                    Category = "Аксессуары",
                    ImageUrl = "/images/mouse.jpg"
                },
                new Product
                {
                    Id = 5,
                    Name = "Клавиатура Mechanical",
                    Description = "Механическая клавиатура с подсветкой RGB, красные свитчи",
                    Price = 6500,
                    Stock = 12,
                    Category = "Аксессуары",
                    ImageUrl = "/images/keyboard.jpg"
                }
            );
        }
    }
}