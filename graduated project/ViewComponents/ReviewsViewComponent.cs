using graduated_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace graduated_project.ViewComponents
{
    public class ReviewsViewComponent:ViewComponent
    {
        private readonly IAppUserProductRepository _repo;

        public ReviewsViewComponent(IAppUserProductRepository repo)
        {
            _repo = repo;
        }

        public IViewComponentResult Invoke(int productId)
        {
            var reviews = _repo.GetAll(productId);
            return View(reviews);
        }
    }
}
