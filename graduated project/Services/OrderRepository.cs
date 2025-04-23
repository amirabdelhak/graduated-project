using graduated_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace graduated_project.Services
{
    public class OrderRepository: IOrderRepository
    {
        private readonly ShopSpheredbcontext context;

        public OrderRepository(ShopSpheredbcontext context)
        {
            this.context = context;
        }

        public Order GetByid(int id)
        {
            return context.Orders.FirstOrDefault(o => o.Id == id);
        }
        public List<Order> GetAll()
        {
            return context.Orders
                .Include(o => o.User)
                .ToList();
        }

        public void Add(Order order)
        {
            context.Orders.Add(order);
            context.SaveChanges();
        }
        public void Update(int id ,Order order)
        {
            var ord = context.Orders.SingleOrDefault(o => o.Id == id);
            ord.OrderDate = order.OrderDate;
            ord.ShippingAddress = order.ShippingAddress;
            ord.TotalPrice = order.TotalPrice;
            context.SaveChanges();
        }
        public void Delete(int id)
        {
            var ord = context.Orders.SingleOrDefault(o => o.Id == id);
            context.Orders.Remove(ord);
            context.SaveChanges();
        }
    }
}
