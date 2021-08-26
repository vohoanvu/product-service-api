namespace AllSopFoodService.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;

    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsWithEagerLoad();
        Task<Product> GetProductWithEagerLoad(int productId);
        IEnumerable<Product> SearchProducts(string searchString);
        bool CheckProductExist(int productId);
    }
}
