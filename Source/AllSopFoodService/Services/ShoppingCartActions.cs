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

    public class ShoppingCartActions
    {
        //public string ShoppingCartId { get; set; } = default!;
        private readonly FoodDBContext _db;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private ISession _session => this._httpContextAccessor.HttpContext.Session;

        //public const string CartSessionKey = "CartId";

        //public ShoppingCartActions(IHttpContextAccessor httpContextAccessor) => this._httpContextAccessor = httpContextAccessor;

        public ShoppingCartActions(FoodDBContext dbcontext)
        {
            this._db = dbcontext;
        }

        public async Task AddToCartAsync(int id)
        {
            // Retrieve the product from the database.           
            //this.ShoppingCartId = GetCartId();
            var cartItem = await _db.ShoppingCartItems.SingleOrDefaultAsync(ci => ci.ProductId == id).ConfigureAwait(true);
            //var cartItem = _db.ShoppingCartItems.SingleOrDefault(
            //    c => c.CartId == this.ShoppingCartId
            //    && c.ProductId == id);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists.                 
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ProductId = id,
                    //CartId = ShoppingCartId,
                    Product = _db.FoodProducts.SingleOrDefault(
                    p => p.Id == id),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                _db.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart,                  
                // then add one to the quantity.                 
                cartItem.Quantity++;
            }
            await _db.SaveChangesAsync().ConfigureAwait(true);
        }

        public bool IsThisProductExistInCart(int productID)
        {
            return _db.ShoppingCartItems.Any(e => e.ProductId == productID);
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
            return allCartItems;
            //return _db.ShoppingCartItems.Where(
            //    c => c.CartId == ShoppingCartId).ToList();
        }
    }
}
