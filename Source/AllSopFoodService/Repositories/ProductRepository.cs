namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(FoodDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProductsWithEagerLoad()
        {
            var children = new string[] { "Category" };
            return await this.EntitiesWithEagerLoad(null, children).ConfigureAwait(true);
        }

        public async Task<Product> GetProductWithEagerLoad(int productId) => await this.context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId).ConfigureAwait(true);

        public bool CheckProductExist(int productId) => this.context.Products.Any(p => p.Id == productId);

        public IEnumerable<Product> SearchProducts(string searchString)
        {
            var searchResults = this.context.Products.Include(p => p.Category).Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return searchResults;
        }
    }
}
