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
        Task<ServiceResponse<List<CartVM>>> GetAllCarts();
        ServiceResponse<List<CartVM>> CreateShoppingCart(CartSaves cart);

        ServiceResponse<ShoppingCart> GetCartById(int cartId);

        //ServiceResponse<ShoppingCart> RemoveAProductFromCart(int productId, int cartId);
        ServiceResponse<CartVM> GetCartWithProducts(int cartId);
    }
}
