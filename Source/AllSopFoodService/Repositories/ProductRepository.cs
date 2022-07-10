namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(FoodDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetAllProductsWithEagerLoadAsync()
        {
            var children = new[] { "Category" };
            return await this.EntitiesWithEagerLoadAsync(null, children).ConfigureAwait(true);
        }

        public async Task<Product> GetProductWithEagerLoadAsync(int productId) => await this.context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == productId).ConfigureAwait(true);

        public bool CheckProductExist(int productId) => this.context.Products.Any(p => p.Id == productId);

        public IEnumerable<Product> SearchProducts(string searchString)
        {
            var searchResults = this.context.Products.Include(p => p.Category).Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            return searchResults;
        }
    }
}
