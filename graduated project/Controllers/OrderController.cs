using graduated_project.Models;
using graduated_project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace graduated_project.Controllers
{
    public class OrderController : Controller
    {
        ShopSpheredbcontext context = new ShopSpheredbcontext();

        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public IActionResult GetAll()
        {
            var orders = orderRepository.GetAll();
            return View(orders);
        }
        public IActionResult Get(int id)
        {
            var order = orderRepository.GetByid(id);
            return View(order);
        }

        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add()
        {
            var cartCookie = Request.Cookies["cart"];
            if (string.IsNullOrEmpty(cartCookie))
            {
                return RedirectToAction("ViewCart", "Cart");
            }

            var cartProductIds = JsonSerializer.Deserialize<List<int>>(cartCookie);
            var products = context.Products
                             .Where(p => cartProductIds.Contains(p.Id))
                             .ToList();

            if (products == null || !products.Any())
            {
                return RedirectToAction("ViewCart", "Cart");
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = await context.Users.FindAsync(userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = new graduated_project.Models.Order
            {
                OrderDate = DateTime.Now,
                ShippingAddress = user.Address,
                TotalPrice = products.Sum(p => p.Priceafterdiscount),
                UserId = userId
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            foreach (var product in products)
            {
                var productOrder = new ProductOrder
                {
                    ProductId = product.Id,
                    OrderId = order.Id
                };
                context.ProductOrders.Add(productOrder);
            }

            await context.SaveChangesAsync();

            Response.Cookies.Delete("cart");

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }


        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await context.Orders
                                .Include(o => o.ProductOrders)
                                .ThenInclude(po => po.Product) // نربط الـ Product مع الـ ProductOrder
                                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }



[HttpGet]
public IActionResult Update(int id)
{
    var order = orderRepository.GetByid(id);
    return View(order);
}
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Update(int id, graduated_project.Models.Order order)
{
    if (ModelState.IsValid)
    {
        orderRepository.Update(id, order);
        return RedirectToAction(nameof(GetAll));
    }
    return View(order);
}

public IActionResult Delete(int id)
{
    orderRepository.Delete(id);
    return RedirectToAction(nameof(GetAll));
}
    }
}
