namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.ViewModels.UserAuth;

    public interface ICartsService
    {
        // See all available cart associated with each User in the system
        // Since all Cart API service requires User Authorization, there's no need for newUserId really
        Task<ServiceResponse<List<CartVM>>> GetAllCarts();
        // Create A New User Cart but rarely executed, since new user registration automatically create new shopping cart
        Task<ServiceResponse<ShoppingCart>> CreateShoppingCart();
        // See all Cart information owned by currently authenticated User
        Task<ServiceResponse<ShoppingCart>> GetSingleCartByCurrentUser();
        // This one is just a Helper function
        ServiceResponse<ShoppingCart> GetCartById(int cartId);
        // See all products information within the Cart owned by currently authenticated User
        ServiceResponse<CartVM> GetCartWithProducts();
        // Add a product to the cart owned by the currently logged-in User
        Task<ServiceResponse<ProductsInCartsVM>> AddToCart(int productId);
        // Delete a Product from the Cart owned by the currenly logged-in User
        Task<ServiceResponse<CartVM>> RemoveFromCart(int productId);
        // Apply a voucher code to the Cart owned by currently authenticated user
        Task<ServiceResponse<VoucherResponseModel>> ApplyVoucherToCart(string voucherCode);
        // Get the Total Price for the Shopping Cart owned by the currently authenticated User
        ServiceResponse<decimal> GetTotal();
        // Delete A User Account, along with his/her cart
        ServiceResponse<bool> DeleteUserAccount(int userId);
        Task<ServiceResponse<List<UserAccountVM>>> GetAllUsers();
    }
}
