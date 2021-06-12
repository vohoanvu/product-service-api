namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;

    public interface IShoppingCartActions
    {
        Task<bool> IsThisProductExistInCartAsync(int productID);
        Task<CartItem> AddToCartAsync(int productId);
        Task<CartItem> RemoveFromCartAsync(int productId);
        decimal GetTotal(IEnumerable<CartItem> cart);
        bool CheckCodeExists(string code);
        Task<decimal> ApplyVoucherToCartAsync(string voucherCode);
        Task<CartItem> UpdateCartItemAsync(string id, CartItem newItem);
        bool Is10orMoreDrinksItemInCart(List<CartItem> wholeCart);
        List<CartItem> GetCartItems();
        decimal GetTotalWithDiscount();
    }
}
