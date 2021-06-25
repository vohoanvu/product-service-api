namespace AllSopFoodService.Services.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.Model;

    public interface IProductsInCartsService
    {
        // Get all products info in a cart
        Task<ServiceResponse<List<ProductsInCartsVM>>> GetProductsByCartId(int cartId);
        // Get all carts info that contains a product, perhaps unnecessary
        //ServiceResponse<List<FoodProduct_ShoppingCart>> GetCartsByProductId(int productId);
        // Get all Products In Carts, should not be used that much
        Task<ServiceResponse<List<FoodProduct_ShoppingCart>>> GetAllProductsInCarts(); // Really should refactor this
        // Add a product into a cart
        Task<ServiceResponse<CartVM>> AddToCartAsync(int productId, int cartId);
        // Remove a product from a cart
        Task<ServiceResponse<FoodProduct_ShoppingCart>> RemoveFromCartAsync(int productId, int cartId);
        // Apply a voucher code to a cart
        Task<ServiceResponse<VoucherResponseModel>> ApplyVoucherToCartAsync(string voucherCode, int cardId);
        //Task<ServiceResponse<List<FoodProduct_ShoppingCart>>> UpdateCartAsync(int cartId); // unnecessary
        // Get the total price of the cart, before any discount
        //decimal GetTotal(List<FoodProduct_ShoppingCart> cart);
        ServiceResponse<decimal> GetTotal(int cartId);
        bool CheckVoucherExists(string code); // Really should move this into PromotionController

        //bool Is10orMoreDrinksItemInCart(List<FoodProduct_ShoppingCart> wholeCart); // Really should refactor this
        bool Is10orMoreDrinksItemInCart(int cartId);
        Task<bool> IsThisProductExistInCartAsync(int productID);
    }
}
