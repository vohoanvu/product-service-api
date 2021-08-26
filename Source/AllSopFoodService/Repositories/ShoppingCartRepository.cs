#nullable disable
namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, ICartRepository
    {
        public ShoppingCartRepository(FoodDBContext context) : base(context)
        {
        }

        public bool CheckUserCart(int userId) => this.context.ShoppingCarts.Any(c => c.UserId == userId);

        public ShoppingCart GetShoppingCartByUserId(int userId) => this.context.ShoppingCarts.Include(c => c.FoodProduct_Carts).FirstOrDefault(c => c.UserId == userId);

        public ShoppingCart GetCartWithProductDataByUserId(int userId) => this.context.ShoppingCarts.Include(c => c.FoodProduct_Carts)
                                                                                                    .ThenInclude(pc => pc.FoodProduct)
                                                                                                    .FirstOrDefault(u => u.UserId == userId);

        public IQueryable<ShoppingCart> GetAllCartsWithProductData() => this.context.ShoppingCarts.Include(c => c.FoodProduct_Carts).ThenInclude(u => u.FoodProduct);

        public ShoppingCart GetShoppingCartByCartId(int cartId) => this.context.ShoppingCarts.Include(c => c.FoodProduct_Carts).FirstOrDefault(c => c.Id == cartId);
    }
}
