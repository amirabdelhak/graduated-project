using graduated_project.Models;

namespace graduated_project.Services
{
    public interface IAppUserProductRepository
    {
        List<AppUserProduct> GetAll(int prodductid);
        AppUserProduct? GetByIds(string appUserId, int productId);
        void Add(AppUserProduct appUserProduct);
        void Update(string appUserId, int productId, AppUserProduct appUserProduct);
        void Delete(string appUserId, int productId);
    }
}