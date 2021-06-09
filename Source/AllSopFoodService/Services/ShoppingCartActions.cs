#nullable disable
namespace AllSopFoodService.Services
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;

    public class ShoppingCartActions : IShoppingCartActions
    {
        //public string ShoppingCartId { get; set; } = default!;
        private readonly FoodDBContext _db;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private ISession _session => this._httpContextAccessor.HttpContext.Session;

        //public const string CartSessionKey = "CartId";

        //public ShoppingCartActions(IHttpContextAccessor httpContextAccessor) => this._httpContextAccessor = httpContextAccessor;

        public ShoppingCartActions(FoodDBContext dbcontext) => this._db = dbcontext;

        public async Task<CartItem> AddToCartAsync(int productId)
        {
            // Retrieve the product from the database.           
            //this.ShoppingCartId = GetCartId();
            var cartItem = await this._db.ShoppingCartItems.SingleOrDefaultAsync(ci => ci.ProductId == productId).ConfigureAwait(true);
            //var cartItem = _db.ShoppingCartItems.SingleOrDefault(
            //    c => c.CartId == this.ShoppingCartId
            //    && c.ProductId == id);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists.                 
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ProductId = productId,
                    //CartId = ShoppingCartId,
                    Product = this._db.FoodProducts.SingleOrDefault(
                    p => p.Id == productId),
                    Quantity = 1,
                    Description = this._db.FoodProducts.SingleOrDefault(
                    p => p.Id == productId).Name,
                    DateCreated = DateTime.Now
                };

                this._db.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart,                  
                // then add one to the quantity.                 
                cartItem.Quantity++;
            }
            await this._db.SaveChangesAsync().ConfigureAwait(true);

            return cartItem;
        }

        public bool IsThisProductExistInCart(int productID) => this._db.ShoppingCartItems.Any(e => e.ProductId == productID);

        public async Task<CartItem> RemoveFromCartAsync(int productId)
        {
            var isExisted = this.IsThisProductExistInCart(productId);
            if (!isExisted)
            {
                return null;
            }
            var cartItem = await this._db.ShoppingCartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId).ConfigureAwait(true);
            this._db.ShoppingCartItems.Remove(cartItem);
            await this._db.SaveChangesAsync().ConfigureAwait(true);

            return cartItem;
        }

        public decimal GetTotal()
        {
            var total = decimal.Zero;
            foreach (var cartItem in this.GetCartItems())
            {
                total += cartItem.Product.Price * cartItem.Quantity;
            }

            return total;
        }

        //public void Dispose()
        //{
        //    if (_db != null)
        //    {
        //        _db.Dispose();
        //        _db = null;
        //    }
        //}

        //public string GetCartId()
        //{
        //    if (_session.GetString(CartSessionKey) == null)
        //    {
        //        if (!string.IsNullOrWhiteSpace(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value))
        //        {
        //            _session.SetString(CartSessionKey, this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value);
        //        }
        //        else
        //        {
        //            // Generate a new random GUID using System.Guid class.     
        //            var tempCartId = Guid.NewGuid();
        //            _session.SetString(CartSessionKey, tempCartId.ToString());
        //            //HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
        //        }
        //    }
        //    //return HttpContext.Current.Session[CartSessionKey].ToString();
        //    return _session.GetString(CartSessionKey);
        //}

        public List<CartItem> GetCartItems()
        {
            //this.ShoppingCartId = GetCartId();
            var allCartItems = _db.ShoppingCartItems.ToList();

            foreach (var item in allCartItems)
            {
                item.Product = _db.FoodProducts.Find(item.ProductId);
            }

            return allCartItems;
            //return _db.ShoppingCartItems.Where(
            //    c => c.CartId == ShoppingCartId).ToList();
        }
    }
}
