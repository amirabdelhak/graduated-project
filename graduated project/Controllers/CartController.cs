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
        private readonly IConfiguration _configuration; 

        public CartController(ShopSpheredbcontext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;  
        }

        
    


    [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var cartCookie = Request.Cookies["cart"];
            List<int> cart;

            if (string.IsNullOrEmpty(cartCookie))
            {
                cart = new List<int>();
            }
            else
            {
                cart = JsonSerializer.Deserialize<List<int>>(cartCookie);
            }
            //الجزء ده عشان ميكررش اضافه منتج هو ضايفه بالفعل 
            if (!cart.Contains(productId))
            {
                cart.Add(productId);

                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7) 
                };
                Response.Cookies.Append("cart", JsonSerializer.Serialize(cart), options);// الكارت هنا عباره عن جيسون يعنى شويه استرينج فلما تطلبه فى اى حته على مستوى البرنامج لازم تفكه بالجيسون بردو
            }
            
            return RedirectToAction("GetProducts", "Products");
        }


        public IActionResult ViewCart()
        {
            var cartCookie = Request.Cookies["cart"];
            List<int> cart;

            if (string.IsNullOrEmpty(cartCookie))
            {
                cart = new List<int>();
            }
            else
            {
                cart = JsonSerializer.Deserialize<List<int>>(cartCookie);
            }

            var products = _context.Products
                            .Where(p => cart.Contains(p.Id))
                            .ToList();

            return View(products);
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cartCookie = Request.Cookies["cart"];
            List<int> cart;

            if (string.IsNullOrEmpty(cartCookie))
            {
                cart = new List<int>();
            }
            else
            {
                cart = JsonSerializer.Deserialize<List<int>>(cartCookie);
            }

            if (cart.Contains(productId))
            {
                cart.Remove(productId);

                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7)   
                };
                Response.Cookies.Append("cart", JsonSerializer.Serialize(cart), options);
            }

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
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            var cartCookie = Request.Cookies["cart"];
            if (string.IsNullOrEmpty(cartCookie))
            {
                return RedirectToAction("ViewCart", "Cart");
            }

            var cartProductIds = JsonSerializer.Deserialize<List<int>>(cartCookie);

            var products = _context.Products
                            .Where(p => cartProductIds.Contains(p.Id))
                            .ToList();

            if (products == null || !products.Any())
            {
                return RedirectToAction("ViewCart", "Cart");
            }

            var lineItems = new List<SessionLineItemOptions>();

            foreach (var product in products)
            {
                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(product.Priceafterdiscount * 100),
                        Currency = "EGP",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name,
                        },
                    },
                    Quantity = 1,
                };

                lineItems.Add(lineItem);
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
        {
            "card",
        },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:44321/Order/Add/", 
                CancelUrl = "https://localhost:44321/Cart/ViewCart/",         
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return Redirect(session.Url);
        }
    }
}

