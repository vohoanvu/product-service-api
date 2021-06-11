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
    using System.Net.Http;

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

        public decimal GetTotalAfterDiscount(IEnumerable<CartItem> discountedCart)
        {
            var total = decimal.Zero;
            foreach (var cartItem in discountedCart)
            {
                total += cartItem.Product.Price * cartItem.Quantity;
            }

            return total;
        }

        public bool CheckCodeExists(string code) => this._db.CouponCodes.Any(promo => promo.CouponCode == code);

        public async Task<HttpResponseMessage> ApplyVoucherToCartAsync(string voucherCode)
        {
            var promotion = await this._db.CouponCodes.SingleOrDefaultAsync(promo => promo.CouponCode == voucherCode).ConfigureAwait(true);
            var allCartItems = this.GetCartItems(); //Get all current items in the cart
            var response = new HttpResponseMessage();
            // processing the promotion logic here
            switch (promotion.CouponCode)
            {
                case "10OFFPROMODRI":
                    // Bool: True if there are 10 or more Drinks Item in Cart, False otherwise
                    var quantityCondition = this.Is10orMoreDrinksItemInCart(allCartItems);
                    if (!quantityCondition)
                    {
                        response.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
                        response.Headers.Add("Message", "You need to buy at least 10 or more Drinks Item!");
                        return response;
                    }

                    foreach (var cartItem in allCartItems)
                    {
                        // mapping Product Object property for each CartItem
                        var discountedProduct = new FoodProduct()
                        {
                            // might be using AutoMapper here
                            Id = cartItem.ProductId,
                            Name = cartItem.Product.Name,
                            Price = cartItem.Product.Price - (cartItem.Product.Price * Convert.ToDecimal(0.1)),
                            Quantity = cartItem.Product.Quantity,
                            IsInCart = cartItem.Product.IsInCart,
                            CategoryId = cartItem.Product.CategoryId
                        };
                        // use the newly updated Product Object of each CartItem
                        cartItem.Product = discountedProduct;
                        // update each CartItem in DB
                        await this.UpdateCartItemAsync(cartItem.ItemId, cartItem).ConfigureAwait(true);
                    }

                    //mark this coupon as used
                    promotion.IsClaimed = true;
                    this._db.SaveChanges();
                    break;
                case "5OFFPROMOALL":


                    break;
                case "20OFFPROMOALL":


                    break;
                default:

                    break;
            }

            return response;
        }

        public async Task<CartItem> UpdateCartItemAsync(string id, CartItem newItem)
        {
            newItem.ItemId = id;
            //update database
            if (newItem == null)
            {
                throw new ArgumentNullException(nameof(newItem));
            }
            var currentCartItem = await this._db.ShoppingCartItems.FindAsync(id).ConfigureAwait(true);
            if (currentCartItem != null)
            {
                return null;
            }
            // remove the current CartItem
            this._db.ShoppingCartItems.Remove(currentCartItem);
            await this._db.SaveChangesAsync().ConfigureAwait(true);
            //Add new CartItem
            var updateCartItem = this._db.ShoppingCartItems.Add(newItem);
            await this._db.SaveChangesAsync().ConfigureAwait(true);

            return updateCartItem.Entity;
        }

        //True if there are 10 or more Drinks Item in Cart, False otherwise
        public bool Is10orMoreDrinksItemInCart(List<CartItem> wholeCart)
        {
            var drinksList = new List<CartItem>();
            foreach (var item in wholeCart)
            {
                if (item.Product.CategoryId == 3) // specifically checking for 'Drinks' Category
                {
                    drinksList.Add(item);
                }
            }
            // count DrinksList
            if (drinksList.Count < 10)
            {
                return false;
            }

            return true;
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

            // assign the FoodProduct object to the list of returned results
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
