namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;

    public interface ICartsService
    {
        void CreateShoppingCart(CartVM cart);

        ServiceResponse<ShoppingCart> GetCartById(int cartId);

        CartWithProductsVM GetCartWithProducts(int cartId);
    }
}
