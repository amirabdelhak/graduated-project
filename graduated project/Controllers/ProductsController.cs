using graduated_project.Models;
using graduated_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace graduated_project.Controllers
{
    
    public class ProductsController : Controller
    {

        IWebHostEnvironment _webHostEnvironment;
        private readonly IProductRepository productRepository;

        public ProductsController(IWebHostEnvironment webHostEnvironment, IProductRepository productRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.categorys = productRepository.getCategorys();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product)
        {
            ViewBag.categorys = productRepository.getCategorys();
            if (ModelState.IsValid)
            {
                if (product.Image != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.Image.FileName);
                    string path = Path.Combine(wwwRootPath, "image", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.Image.CopyToAsync(fileStream);
                    }

                    product.ImageName = fileName;
                }


                productRepository.Addproduct(product);
                

                return RedirectToAction(nameof(Getproducts));
            }

            return View(product);
        }
        public ActionResult Getproducts(int? categoryId)
        {
            var products=productRepository.getproducts();
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }
            return View(products);
        }

        public ActionResult getproduct(int productid)
        {
            var product = productRepository.Getbyid(productid);
            return View(product);
        }
        public ActionResult Deleteproduct(int id)
        {
            productRepository.Delete(id);
            return RedirectToAction(nameof(Getproducts));
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.categorys = productRepository.getCategorys();

            var product = productRepository.Getbyid(id);
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int id,Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.Update(id, product);
                return RedirectToAction(nameof(Getproducts));

            }
            else
            {
                return View(product);
            }
        }
    }
}

