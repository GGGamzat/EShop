using Microsoft.AspNetCore.Mvc;
using InternetShop.Models;
using System.Text.Json;
using InternetShop.Data;
using Microsoft.AspNetCore.Authorization;

namespace InternetShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "ShoppingCart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
                return NotFound();

            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                if (quantity <= 0)
                    cart.Remove(cartItem);
                else
                    cartItem.Quantity = quantity;

                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction("Index");

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View(order);

            order.UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            order.OrderDate = DateTime.Now;
            order.TotalAmount = cart.Sum(c => c.Total);
            order.Status = "Pending";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var cartItem in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.ProductName,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                };
                _context.OrderItems.Add(orderItem);

                // Обновление остатков
                var product = await _context.Products.FindAsync(cartItem.ProductId);
                if (product != null)
                {
                    product.Stock -= cartItem.Quantity;
                }
            }

            await _context.SaveChangesAsync();

            // Очистка корзины
            HttpContext.Session.Remove(CartSessionKey);

            return RedirectToAction("OrderConfirmation", new { id = order.Id });
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }
    }
}