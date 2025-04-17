using graduated_project.Models;
using graduated_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace graduated_project.Controllers
{
    public class AppUserProductRepositoryController : Controller
    {
        private readonly IAppUserProductRepository appUserProductRepository;

        public AppUserProductRepositoryController(IAppUserProductRepository appUserProductRepository)
        {
            this.appUserProductRepository = appUserProductRepository;
        }
        public IActionResult GETALL()
        {
            appUserProductRepository.GetAll();
            return View();
        }
        public IActionResult GetByIds(string appUserId, int productId)
        {
            appUserProductRepository.GetByIds(appUserId, productId);
            return RedirectToAction(nameof(GETALL));
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Add(AppUserProduct appUserProduct)
        {
            if (ModelState.IsValid)
            {
                appUserProductRepository.Add(appUserProduct);
                return RedirectToAction(nameof(GETALL));
            }
            return View(appUserProduct);
        }
        public IActionResult Update(string appUserId, int productId)
        {
            var AUP = appUserProductRepository.GetByIds(appUserId,productId);
            return View(AUP);
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult Update(string appUserId, int productId, AppUserProduct appUserProduct)
        {
            if (ModelState.IsValid)
            {
                appUserProductRepository.Update(appUserId, productId, appUserProduct);
                return RedirectToAction(nameof(GETALL)); 
            }

            return View(appUserProduct);
        }
        public IActionResult Delete(string appUserId, int productId)
        {
            appUserProductRepository.Delete(appUserId, productId);
            return RedirectToAction(nameof(GETALL));
        }

    }
}
