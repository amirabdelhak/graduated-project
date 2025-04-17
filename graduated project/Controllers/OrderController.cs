using graduated_project.Models;
using graduated_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace graduated_project.Controllers
{
    public class OrderController : Controller
    {
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
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Order order)
        {
            if (ModelState.IsValid)
            {
                orderRepository.Add(order);
                return RedirectToAction(nameof(GetAll));
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
        public IActionResult Update(int id , Order order)
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
