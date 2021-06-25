namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Services.Interfaces;
    using AllSopFoodService.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class ProductsInCartsService : IProductsInCartsService
    {
        private readonly FoodDBContext _db;
        private readonly IFoodProductsService _foodProductService;

        //Assuming only 1 coupon can be used at a time
        public bool IsCartDiscounted { get; set; } = default!; // false by default

        public ProductsInCartsService(FoodDBContext context, IFoodProductsService foodProductsService)
        {
            this._db = context;
            this._foodProductService = foodProductsService;
        }

        public async Task<ServiceResponse<List<ProductsInCartsVM>>> GetProductsByCartId(int cartId)
        {
            var serviceResponse = new ServiceResponse<List<ProductsInCartsVM>>();

            var listDBProductsInACart = await this._db.FoodProducts_Carts.Where(record => record.ShoppingCartId == cartId).ToListAsync().ConfigureAwait(true);

            var allProductsInACart = listDBProductsInACart.Select(record => new ProductsInCartsVM()
            {
                ProductDescription = record.FoodProduct.Name,
                QuantityInCart = record.QuantityInCart,
                OriginalPrice = record.FoodProduct.Price
            }).ToList();

            serviceResponse.Data = allProductsInACart;

            return serviceResponse;
        }

        public async Task<ServiceResponse<CartVM>> AddToCartAsync(int productId, int cartId)
        {
            var serviceResponse = new ServiceResponse<CartVM>();
            // Retrieve the all items in a cart by cart ID           
            var cartItems = await this._db.FoodProducts_Carts.Where(record => record.ShoppingCartId == cartId).ToListAsync().ConfigureAwait(true);

            //Check if this Product exists in this Cart
            var productAlreadyInCart = cartItems.FirstOrDefault(record => record.FoodProductId == productId);

            if (productAlreadyInCart != null)
            {
                var productInCartVM = new ProductsInCartsVM()
                {
                    ProductDescription = productAlreadyInCart.FoodProduct.Name,
                    QuantityInCart = productAlreadyInCart.QuantityInCart,
                    OriginalPrice = productAlreadyInCart.FoodProduct.Price
                };

                var newCartVM = new CartVM()
                {
                    CartLabel = productAlreadyInCart.ShoppingCart.CartLabel,
                    Products = new List<ProductsInCartsVM>() { productInCartVM }
                };
                serviceResponse.Success = false;
                serviceResponse.Message = "This product has already existed in your cart!";
            }

            // if this product was not already in this cart then do something here...




            // Check if the product is already in Cart
            //if (cartItem == null)
            //{
            //    // Create a new cart item if no cart item exists.                 
            //    cartItem = new CartItem
            //    {
            //        ProductId = productId,
            //        //CartId = ShoppingCartId,
            //        Product = this._db.FoodProducts.SingleOrDefault(
            //        p => p.Id == productId),
            //        Quantity = 1,
            //        Description = this._db.FoodProducts.SingleOrDefault(
            //        p => p.Id == productId).Name
            //    };

            //    this._db.ShoppingCartItems.Add(cartItem);
            //}
            //else
            //{
            //    // If the item does exist in the cart,                  
            //    // then add one to the quantity.                 
            //    cartItem.Quantity++;
            //}
            //await this._db.SaveChangesAsync().ConfigureAwait(true);

            //return cartItem;
            return serviceResponse;
        }

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

        public decimal GetTotal(IEnumerable<CartItem> cart)
        {
            var total = decimal.Zero;
            foreach (var cartItem in cart)
            {
                total += cartItem.Product.Price * cartItem.Quantity;
            }

            return total;
        }

         //this should return the total price of the cart after discount
        public async Task<VoucherResponseModel> ApplyVoucherToCartAsync(string voucherCode)
        {
            var promotion = await this._db.CouponCodes.SingleOrDefaultAsync(promo => promo.CouponCode == voucherCode).ConfigureAwait(true);
            var allCartItems = this.GetCartItems(); //Get all current items in the cart
            //var response = new HttpResponseMessage();
            // processing the promotion logic here
            //decimal discountedPrice = decimal.Zero;
            var result = new VoucherResponseModel();
            var discountedCartPrice = decimal.Zero;

            switch (promotion.CouponCode)
            {
                case "10OFFPROMODRI":
                    // Bool: True if there are 10 or more Drinks Item in Cart, False otherwise
                    var quantityCondition = this.Is10orMoreDrinksItemInCart(allCartItems);
                    if (!quantityCondition)
                    {
                        //response.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
                        //response.Headers.Add("Message", "You need to buy at least 10 or more Drinks Item!");
                        result.Applied = false;
                        result.FailedMessage = "You need to buy at least 10 or more Drinks Item!";
                    }

                    foreach (var cartItem in allCartItems)
                    {
                        // check for Drinks category in each cart item (perhaps there should be a seperate service for this?)
                        if (cartItem.Product.CategoryId == 3) // 'Drinks' Categories, perhaps another way to access this static entity?
                        {
                            //var discountedProduct = new FoodProduct()
                            //{
                            //    // might be using AutoMapper here
                            //    Id = cartItem.ProductId,
                            //    Name = cartItem.Product.Name,
                            //    Price = cartItem.Product.Price - (cartItem.Product.Price * Convert.ToDecimal(0.1)),
                            //    Quantity = cartItem.Product.Quantity,
                            //    IsInCart = cartItem.Product.IsInCart,
                            //    CategoryId = cartItem.Product.CategoryId
                            //};
                            // use the newly updated Product Object of each CartItem
                            //cartItem.Product = discountedProduct;
                            // update each CartItem in DB
                            //await this.UpdateCartItemAsync(cartItem.ItemId, cartItem).ConfigureAwait(true);
                            var originalCost = this._foodProductService.GetOriginalCostbyFoodProductId(cartItem.ProductId);
                            var discountCostPerCartItem = (originalCost - (originalCost * Convert.ToDecimal(0.1))) * cartItem.Quantity;
                            discountedCartPrice += discountCostPerCartItem;
                        }

                    }

                    this.IsCartDiscounted = true;
                    //mark this coupon as used
                    promotion.IsClaimed = true;
                    this._db.SaveChanges();

                    result.Applied = true;
                    result.FailedMessage = "Successfully applied Voucher to Cart!";
                    result.DiscountedCartPrice = discountedCartPrice;

                    break;
                case "5OFFBAKING":
                    // conditions checking
                    // fetch All Baking/Cooking Ingredient items from Cart
                    var bakingCookingItems = this._db.ShoppingCartItems.Where(cartItem => cartItem.Product.CategoryId == 5);
                    if (this.GetTotal(bakingCookingItems) < Convert.ToDecimal(50))
                    {
                        result.Applied = false;
                        result.FailedMessage = "Failed to apply this coupon! You need to spend £50.00 or more on Baking/Cooking Ingredients";
                        result.DiscountedCartPrice = 0;
                    }
                    // apply discount percentage
                    result.Applied = true;
                    result.FailedMessage = "Successfully applied Voucher to Cart!";
                    result.DiscountedCartPrice = this.GetTotal(allCartItems) - Convert.ToDecimal(5);
                    break;
                case "20OFFPROMO":
                    // condition check, spending at least 100 pounds in total
                    if (this.GetTotal(allCartItems) < Convert.ToDecimal(100))
                    {
                        result.Applied = false;
                        result.FailedMessage = "Failed to apply this coupon! You need to spend at least £100.00 or more in total!";
                        result.DiscountedCartPrice = 0;
                    }
                    else
                    {
                        result.Applied = true;
                        result.FailedMessage = "Successfully applied Voucher to Cart!";
                        result.DiscountedCartPrice = this.GetTotal(allCartItems) - Convert.ToDecimal(20);
                    }

                    break;
                default:
                    result.Applied = false;
                    result.FailedMessage = "Invalid Coupon Code";
                    result.DiscountedCartPrice = 0;
                    break;
            }

            return result;
        }

        //public async Task<CartItem> UpdateCartItemAsync(int id, CartItem newItem)
        //{
        //    newItem.ItemId = id;
        //    //update database
        //    if (newItem == null)
        //    {
        //        throw new ArgumentNullException(nameof(newItem));
        //    }
        //    var currentCartItem = await this._db.ShoppingCartItems.AsNoTracking().FirstOrDefaultAsync(item => item.ItemId == id).ConfigureAwait(true);
        //    if (currentCartItem != null)
        //    {
        //        return null;
        //    }
        //    // remove the current CartItem
        //    this._db.ShoppingCartItems.Remove(currentCartItem);
        //    await this._db.SaveChangesAsync().ConfigureAwait(true);
        //    //Add new CartItem
        //    var updateCartItem = this._db.ShoppingCartItems.Add(newItem);
        //    await this._db.SaveChangesAsync().ConfigureAwait(true);

        //    return updateCartItem.Entity;
        //}

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

        public async Task<ServiceResponse<List<FoodProduct_ShoppingCart>>> GetAllProductsInCarts()
        {
            var serviceResponse = new ServiceResponse<List<FoodProduct_ShoppingCart>>();
            var allCartItems = await this._db.FoodProducts_Carts.ToListAsync().ConfigureAwait(true);

            serviceResponse.Data = allCartItems;

            return serviceResponse;
        }

        public async Task<bool> IsThisProductExistInCartAsync(int productID) => await this._db.ShoppingCartItems.AnyAsync(e => e.ProductId == productID).ConfigureAwait(true);
        public bool CheckCodeExists(string code) => this._db.CouponCodes.Any(promo => promo.CouponCode == code);
    }
}
