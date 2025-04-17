using graduated_project.Models;

namespace graduated_project.Services
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ShopSpheredbcontext context;

        public CategoryRepository(ShopSpheredbcontext context)
        {
            this.context = context;
        }
        public void Add(Category Category)
        {
            context.Categorys.Add(Category);
            context.SaveChanges();

        }
        public void Delete(int id)
        {
            var Category = context.Categorys.SingleOrDefault(c => c.Id == id);
            context.Categorys.Remove(Category);
            context.SaveChanges();
        }
        public IEnumerable<Category> GetAll()
        {
            return context.Categorys.ToList();
        }
        public Category Get(int id)
        {
            return context.Categorys.SingleOrDefault(c=>c.Id==id);
        }
        public void Update(int id ,Category Category)
        {
            var ctg = context.Categorys.SingleOrDefault(c=>c.Id==id);
            ctg.Name = Category.Name;
            context.SaveChanges();
        }
    }

    
}
