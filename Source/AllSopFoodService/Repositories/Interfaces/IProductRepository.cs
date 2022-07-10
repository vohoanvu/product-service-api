namespace AllSopFoodService.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;

    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsWithEagerLoadAsync();
        Task<Product> GetProductWithEagerLoadAsync(int productId);
        IEnumerable<Product> SearchProducts(string searchString);
        bool CheckProductExist(int productId);
    }
}
