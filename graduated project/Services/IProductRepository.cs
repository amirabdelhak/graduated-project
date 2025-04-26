using graduated_project.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace graduated_project.Services
{
    public interface IProductRepository
    {

        Product Getbyid(int productid);

        List<Product> getproducts();
        List<Category> getCategorys();

        void Addproduct(Product product);

        void Update(int id, Product product);

        void Delete(int id);


    }
}
