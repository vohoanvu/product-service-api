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
        private readonly FoodDBContext _db;

        //Assuming only 1 coupon can be used at a time
        public bool IsCartDiscounted { get; set; } = default!; // false by default

        public ShoppingCartActions(FoodDBContext dbcontext) => this._db = dbcontext;

        public async Task<CartItem> AddToCartAsync(int productId)
        {
            // Retrieve the product from the Cart database.           
            var cartItem = await this._db.ShoppingCartItems.SingleOrDefaultAsync(ci => ci.ProductId == productId).ConfigureAwait(true);

            // Check if the product is already in Cart
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

        public async Task<bool> IsThisProductExistInCartAsync(int productID) => await this._db.ShoppingCartItems.AnyAsync(e => e.ProductId == productID).ConfigureAwait(true);

        public async Task<CartItem> RemoveFromCartAsync(int productId)
        {
            var isExisted = await this.IsThisProductExistInCartAsync(productId).ConfigureAwait(true);

            if (!isExisted)
            {
                return null;
            }

            var cartItem = await this._db.ShoppingCartItems.FirstOrDefaultAsync(ci => ci.ProductId == productId).ConfigureAwait(true);
            this._db.ShoppingCartItems.Remove(cartItem);
            await this._db.SaveChangesAsync().ConfigureAwait(true);

            return cartItem;
        }

        public decimal GetTotalWithDiscount()
        {
            var total = decimal.Zero;
            var currentCart = this.GetCartItems();

            if (this.IsCartDiscounted)
            {
                total = this.GetTotal(currentCart);
            }
            // otherwise return Zero
            return total;
        }

        public decimal GetTotal(IEnumerable<CartItem> cart)
        {
            var total = decimal.Zero;
            foreach (var cartItem in cart)
            {
                total += cartItem.Product.Price * cartItem.Quantity;
            }

            return total;
        }

        public bool CheckCodeExists(string code) => this._db.CouponCodes.Any(promo => promo.CouponCode == code);

        // this should return the total price of the cart after discount
        public async Task<decimal> ApplyVoucherToCartAsync(string voucherCode)
        {
            var promotion = await this._db.CouponCodes.SingleOrDefaultAsync(promo => promo.CouponCode == voucherCode).ConfigureAwait(true);
            var allCartItems = this.GetCartItems(); //Get all current items in the cart
            //var response = new HttpResponseMessage();
            // processing the promotion logic here
            decimal discountedPrice = decimal.Zero;

            switch (promotion.CouponCode)
            {
                case "10OFFPROMODRI":
                    // Bool: True if there are 10 or more Drinks Item in Cart, False otherwise
                    var quantityCondition = this.Is10orMoreDrinksItemInCart(allCartItems);
                    if (!quantityCondition)
                    {
                        //response.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
                        //response.Headers.Add("Message", "You need to buy at least 10 or more Drinks Item!");
                        discountedPrice = -1;
                    }

                    foreach (var cartItem in allCartItems)
                    {
                        // check for Drinks category in each cart item (perhaps there should be a seperate service for this?)
                        if (cartItem.Product.CategoryId == 3) // 'Drinks' Categories, perhaps another way to access this static entity?
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

                    }
                    // not really using this value
                    discountedPrice = this.GetTotal(this.GetCartItems());
                    this.IsCartDiscounted = true;
                    //mark this coupon as used
                    promotion.IsClaimed = true;
                    this._db.SaveChanges();
                    break;
                case "5OFFPROMOALL":


                    break;
                case "20OFFPROMOALL":


                    break;
                default:
                    return discountedPrice;
            }

            return discountedPrice;
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
