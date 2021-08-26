namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;

    public class ProductinShoppingCartRepository : GenericRepository<FoodProduct_ShoppingCart>, IProductinShoppingCartRepository
    {
        public ProductinShoppingCartRepository(FoodDBContext context) : base(context)
        {
        }

    }
}
