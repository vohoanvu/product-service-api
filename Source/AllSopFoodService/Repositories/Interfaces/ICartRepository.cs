namespace AllSopFoodService.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;

    public interface ICartRepository : IGenericRepository<ShoppingCart>
    {
        //Check if there is any Shopping Cart associated with a specific User
        bool CheckUserCart(int userId);

        ShoppingCart GetShoppingCartByUserId(int userId);

        ShoppingCart GetCartWithProductDataByUserId(int userId);

        IQueryable<ShoppingCart> GetAllCartsWithProductData();

        ShoppingCart GetShoppingCartByCartId(int cartId);
    }
}
