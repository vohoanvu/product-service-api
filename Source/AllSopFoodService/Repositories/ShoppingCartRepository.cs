#nullable disable
namespace AllSopFoodService.Repositories
{
    using System.Linq;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, ICartRepository
    {
        public ShoppingCartRepository(FoodDbContext context) : base(context)
        {
        }

        public bool CheckUserCart(int userId) => this.context.ShoppingCarts.Any(c => c.UserId == userId);

        public ShoppingCart GetShoppingCartByUserId(int userId) => this.context.ShoppingCarts.Include(c => c.FoodProductCarts).FirstOrDefault(c => c.UserId == userId);

        public ShoppingCart GetCartWithProductDataByUserId(int userId) => this.context.ShoppingCarts.Include(c => c.FoodProductCarts)
                                                                                                    .ThenInclude(pc => pc.FoodProduct)
                                                                                                    .FirstOrDefault(u => u.UserId == userId);

        public IQueryable<ShoppingCart> GetAllCartsWithProductData() => this.context.ShoppingCarts.Include(c => c.FoodProductCarts).ThenInclude(u => u.FoodProduct);

        public ShoppingCart GetShoppingCartByCartId(int cartId) => this.context.ShoppingCarts.Include(c => c.FoodProductCarts).FirstOrDefault(c => c.Id == cartId);
    }
}
