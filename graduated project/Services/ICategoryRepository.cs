using graduated_project.Models;

namespace graduated_project.Services
{
    public interface ICategoryRepository
    {
        void Add(Category Category);
        void Delete(int id);
        IEnumerable<Category> GetAll();
        Category Get(int id);
        void Update(int id, Category Category);

    }
}