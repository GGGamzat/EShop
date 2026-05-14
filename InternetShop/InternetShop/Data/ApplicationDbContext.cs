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
                    Name = "Ноутбук Lenovo",
                    Description = "Мощный ноутбук для работы и игр",
                    Price = 45000,
                    Stock = 10,
                    Category = "Электроника",
                    ImageUrl = "/images/laptop.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "Смартфон Xiaomi",
                    Description = "Современный смартфон с отличной камерой",
                    Price = 25000,
                    Stock = 20,
                    Category = "Электроника",
                    ImageUrl = "/images/phone.jpg"
                },
                new Product
                {
                    Id = 3,
                    Name = "Наушники Sony",
                    Description = "Беспроводные наушники с шумоподавлением",
                    Price = 8000,
                    Stock = 30,
                    Category = "Аксессуары",
                    ImageUrl = "/images/headphones.jpg"
                }
            );
        }
    }
}