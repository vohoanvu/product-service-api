namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using Microsoft.AspNetCore.Mvc;

    public interface ICartItemService
    {
        Task<bool> IsThisProductExistInCartAsync(int productID);
        Task<CartItem> AddToCartAsync(int productId);
        Task<CartItem> RemoveFromCartAsync(int productId);
        decimal GetTotal(IEnumerable<CartItem> cart);
        bool CheckCodeExists(string code); // Really should refactor this
        Task<VoucherResponseModel> ApplyVoucherToCartAsync(string voucherCode);
        Task<CartItem> UpdateCartItemAsync(int id, CartItem newItem);
        bool Is10orMoreDrinksItemInCart(List<CartItem> wholeCart); // Really should refactor this
        List<CartItem> GetCartItems(); // Really should refactor this
    }
}
