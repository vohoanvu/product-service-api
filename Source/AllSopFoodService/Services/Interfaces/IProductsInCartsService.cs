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
        //Task<ServiceResponse<FoodProduct_ShoppingCart>> GetSinglebyProductIdAndCartId(int productId, int cartId);
        // Get all Products In Carts, should not be used that much
        Task<ServiceResponse<List<FoodProduct_ShoppingCart>>> GetAllProductsInCarts(); // Really should refactor this
        // Add a product into a cart
        Task<ServiceResponse<ProductsInCartsVM>> AddToCartAsync(int productId, int cartId);
        // Remove a product from a cart
        Task<ServiceResponse<CartVM>> RemoveFromCart(int productId, int cartId);
        // Apply a voucher code to a cart
        Task<ServiceResponse<VoucherResponseModel>> ApplyVoucherToCart(string voucherCode, int cartId);
        //Task<ServiceResponse<List<FoodProduct_ShoppingCart>>> UpdateCartAsync(int cartId); // unnecessary
        // Get the total price of the cart, before any discount
        //decimal GetTotal(List<FoodProduct_ShoppingCart> cart);
        ServiceResponse<decimal> GetTotal(int cartId);
        bool CheckVoucherExists(string code); // PromotionService worthy

        //bool Is10orMoreDrinksItemInCart(List<FoodProduct_ShoppingCart> wholeCart); // Really should refactor this
        bool Is10orMoreDrinksItemInCart(int cartId);
    }
}
