namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;

    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, ICartRepository
    {
        public ShoppingCartRepository(FoodDBContext context) : base(context)
        {
        }
    }
}
