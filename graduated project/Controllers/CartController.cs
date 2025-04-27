using graduated_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using Stripe;
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


        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
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

            // 2- نتأكد إذا كان المنتج موجود في السلة
            if (cart.Contains(productId))
            {
                // 3- نحذف المنتج من السلة
                cart.Remove(productId);

                // 4- نخزن الكارت المحدث في الكوكي
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7) // الكوكي يعيش 7 أيام
                };
                Response.Cookies.Append("cart", JsonSerializer.Serialize(cart), options);
            }

            // 5- نرجع لعرض السلة أو صفحة المنتجات أو أي صفحة أخرى
            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public IActionResult EmptyCart()
        {
            Response.Cookies.Delete("cart");
            return RedirectToAction("ViewCart");
        }



        public async Task<IActionResult> Checkout()
        {
            StripeConfiguration.ApiKey = "sk_test_51RIEzNPteMQm3MrSLbt9HzbXjE7sEjihHZxdC38Z8iTuSGpaOJZBnlJhAy2zj931zePVsgqddCesOs8y14kGgAXi00gwmPDupl";

            // 1- نقرأ IDs المنتجات من الكوكي
            var cartCookie = Request.Cookies["cart"];
            if (string.IsNullOrEmpty(cartCookie))
            {
                // لو مفيش منتجات في السلة، رجعه لصفحة الكارت
                return RedirectToAction("ViewCart", "Cart");
            }

            var cartProductIds = JsonSerializer.Deserialize<List<int>>(cartCookie);

            // 2- نجيب المنتجات من قاعدة البيانات
            var products = _context.Products
                            .Where(p => cartProductIds.Contains(p.Id))
                            .ToList();

            if (products == null || !products.Any())
            {
                return RedirectToAction("ViewCart", "Cart");
            }

            // 3- نحول المنتجات إلى Line Items لسترايب
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var product in products)
            {
                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(product.Priceafterdiscount * 100), // السعر لازم يبقى بالقرش (مش بالجنيه)
                        Currency = "EGP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name,
                            // ممكن تضيف صورة كمان هنا لو حابب لازم ندخله بيانات عشان يعرف يعرضها 
                        },
                    },
                    Quantity = 1, // كل منتج قطعة واحدة (ممكن تعدل لو عندك كميات)
                };

                lineItems.Add(lineItem);
            }

            // 4- نحضر سيشن الدفع
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                "card",
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://yourwebsite.com/Success",
                CancelUrl = "https://yourwebsite.com/Cancel",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Redirect(session.Url);
            
        } 
    }
}

