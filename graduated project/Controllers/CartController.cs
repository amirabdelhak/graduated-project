using graduated_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace graduated_project.Controllers
{

    public class CartController : Controller
    {
        private readonly ShopSpheredbcontext _context;

        public CartController(ShopSpheredbcontext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            // 1- قراءة الكارت الحالي من الكوكي
            var cartCookie = Request.Cookies["cart"];
            List<int> cart;

            if (string.IsNullOrEmpty(cartCookie))
            {
                // مفيش كارت - نبدأ واحد جديد
                cart = new List<int>();
            }
            else
            {
                // فيه كارت - نفكه من شكل نصي إلى ليست
                cart = JsonSerializer.Deserialize<List<int>>(cartCookie);
            }

            // 2- نتأكد المنتج مش موجود أصلا
            if (!cart.Contains(productId))
            {
                // لو مش موجود - نضيفه
                cart.Add(productId);

                // 3- نخزن الكارت الجديد في الكوكي
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7) // الكوكي يعيش 7 أيام
                };
                Response.Cookies.Append("cart", JsonSerializer.Serialize(cart), options);
            }
            // لو موجود بالفعل.. ولا كأننا ضفنا حاجة

            // 4- نرجع مثلا لصفحة المنتجات أو أي مكان
            return RedirectToAction("GetProducts", "Products");
        }

        public IActionResult ViewCart()
        {
            // 1- نقرأ الكوكي
            var cartCookie = Request.Cookies["cart"];
            List<int> cart;

            if (string.IsNullOrEmpty(cartCookie))
            {
                // لو الكوكي فاضية.. نعمل ليست فاضية
                cart = new List<int>();
            }
            else
            {
                // لو فيها بيانات.. نفكها
                cart = JsonSerializer.Deserialize<List<int>>(cartCookie);
            }

            // 2- نجيب المنتجات الحقيقية من الداتا بيز بناءً على الـ IDs اللي جوه الكارت
            var products = _context.Products
                            .Where(p => cart.Contains(p.Id))
                            .ToList();

            // 3- نبعته للمشهد (View)
            return View(products);
        }


    }


}

