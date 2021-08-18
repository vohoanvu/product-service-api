namespace AllSopFoodService.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;

    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Product> GetAllProductsData();

        //Product GetSingleProduct(int productId);

        //void CreateProduct(Product newProduct);

        IEnumerable<Product> SearchProducts(string searchString);
        bool CheckProductExist(int productId);

        //void UpdateFoodProduct(Product product);

        //void RemoveProductById(int productId);
    }
}
