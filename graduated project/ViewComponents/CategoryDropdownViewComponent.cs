using graduated_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace graduated_project.ViewComponents
{
    public class CategoryDropdownViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryDropdownViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }
    }
}
