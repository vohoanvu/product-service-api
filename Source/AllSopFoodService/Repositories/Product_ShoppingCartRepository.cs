namespace AllSopFoodService.Repositories
{
    using Interfaces;
    using Model;

    public class ProductinShoppingCartRepository : GenericRepository<FoodProductShoppingCart>, IProductinShoppingCartRepository
    {
        public ProductinShoppingCartRepository(FoodDbContext context) : base(context)
        {
        }

    }
}
