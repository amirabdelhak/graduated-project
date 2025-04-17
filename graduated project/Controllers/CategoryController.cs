﻿using graduated_project.Models;
using graduated_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace graduated_project.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Add(category);
                return RedirectToAction(nameof(GetAll));

            }
            return View(category);
        }
        public IActionResult GetAll()
        {
            var caregory = categoryRepository.GetAll();
            return View(caregory);
        }

        public IActionResult Get(int id)
        {
            var category = categoryRepository.Get(id);
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            categoryRepository.Delete(id);
            return RedirectToAction(nameof(GetAll));
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = categoryRepository.Get(id);
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Update(id, category);
                return RedirectToAction(nameof(GetAll));
            }
            return View(category);
        }
        public IActionResult Index()
        {
            var categories = categoryRepository.GetAll();  // افترضنا أنك تجلب الفئات من الريبو
            ViewBag.Categories = categories;
            return View();
        }

    }
}
