namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;

    public interface IShoppingCartActions
    {
        bool IsThisProductExistInCart(int productID);
        Task<CartItem> AddToCartAsync(int productId);
        Task<CartItem> RemoveFromCartAsync(int productId);
        decimal GetTotal();
    }
}
