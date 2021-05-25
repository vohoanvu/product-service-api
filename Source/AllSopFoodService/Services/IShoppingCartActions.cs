namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IShoppingCartActions
    {
        bool IsThisProductExistInCart(int productID);
        Task AddToCartAsync(int productId);
    }
}
