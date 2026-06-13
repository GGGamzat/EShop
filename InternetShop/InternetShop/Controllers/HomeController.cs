using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InternetShop.Data;
using InternetShop.Models;
using System.Diagnostics;

namespace InternetShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string category = null, string searchTerm = null, string sortBy = "name")
        {
            var products = _context.Products.AsQueryable();

            // Фильтрация по категории
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
                ViewBag.SelectedCategory = category;
            }

            // Поиск
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.Name.Contains(searchTerm) || (p.Description != null && p.Description.Contains(searchTerm)));
                ViewBag.SearchTerm = searchTerm;
            }

            // Сортировка
            switch (sortBy)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }
            ViewBag.SortBy = sortBy;

            // Категории
            var categories = await _context.Products
                .Where(p => p.Category != null)
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();

            ViewBag.Categories = categories;

            // Продукты с истекающим сроком
            var expiringProducts = await _context.Products
                .Where(p => p.ExpiryDate.HasValue && p.ExpiryDate.Value <= DateTime.Now.AddDays(3) && p.ExpiryDate.Value >= DateTime.Now)
                .Take(5)
                .ToListAsync();

            ViewBag.ExpiringProducts = expiringProducts;

            return View(await products.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var similarProducts = await _context.Products
                .Where(p => p.Category == product.Category && p.Id != product.Id)
                .Take(4)
                .ToListAsync();

            ViewBag.SimilarProducts = similarProducts;

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Organic()
        {
            var organicProducts = await _context.Products
                .Where(p => p.IsOrganic && p.Stock > 0)
                .ToListAsync();

            ViewBag.Title = "Органические продукты";
            return View("Index", organicProducts);
        }

        public async Task<IActionResult> Fresh()
        {
            var freshProducts = await _context.Products
                .Where(p => p.CreatedAt >= DateTime.Now.AddDays(-7))
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.Title = "Новинки";
            return View("Index", freshProducts);
        }

        public async Task<IActionResult> QuickSearch(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            var products = await _context.Products
                .Where(p => p.Name.Contains(term) && p.Stock > 0)
                .Select(p => new { p.Id, p.Name, p.Price, p.Unit })
                .Take(5)
                .ToListAsync();

            return Json(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}