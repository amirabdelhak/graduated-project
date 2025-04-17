using graduated_project.Models;

namespace graduated_project.Services
{
    public interface IOrderRepository
    {
        Order GetByid(int id);
        List<Order> GetAll();
        void Add(Order order);
        void Update(int id, Order order);
        void Delete(int id);
    }
}