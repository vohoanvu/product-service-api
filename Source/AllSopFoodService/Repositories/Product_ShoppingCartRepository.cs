namespace AllSopFoodService.Repositories
{
    using Interfaces;
    using Model;

    public class ProductinShoppingCartRepository : GenericRepository<FoodProductInShoppingCart>, IProductinShoppingCartRepository
    {
        public ProductinShoppingCartRepository(FoodDbContext context) : base(context)
        {
        }

    }
}
