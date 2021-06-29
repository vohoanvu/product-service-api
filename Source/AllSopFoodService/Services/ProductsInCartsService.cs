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
        private readonly IProductsService _foodProductService;
        private readonly ICartsService _cartService;

        //Assuming only 1 coupon can be used at a time
        public bool IsCartDiscounted { get; set; } = default!; // false by default

        public ProductsInCartsService(FoodDBContext context, IProductsService foodProductsService, ICartsService cartService)
        {
            this._db = context;
            this._foodProductService = foodProductsService;
            this._cartService = cartService;
        }

        public async Task<ServiceResponse<List<ProductsInCartsVM>>> GetProductsByCartId(int cartId)
        {
            var serviceResponse = new ServiceResponse<List<ProductsInCartsVM>>();

            var listDBProductsInACart = await this._db.FoodProducts_Carts.Where(record => record.CartId == cartId).ToListAsync().ConfigureAwait(true);

            var allProductsInACart = listDBProductsInACart.Select(record => new ProductsInCartsVM()
            {
                ProductDescription = record.FoodProduct.Name,
                QuantityInCart = record.QuantityInCart,
                OriginalPrice = record.FoodProduct.Price,
                CartId = record.CartId
            }).ToList();

            serviceResponse.Data = allProductsInACart;

            return serviceResponse;
        }

        public async Task<ServiceResponse<ProductsInCartsVM>> AddToCartAsync(int productId, int cartId)
        {
            // assuming productId and cartId are both valid, validation are executed in controller
            var serviceResponse = new ServiceResponse<ProductsInCartsVM>();
            // Retrieve the all items in a cart by cart ID           
            //var cartItems = await this._db.FoodProducts_Carts.Where(record => record.ShoppingCartId == cartId).ToListAsync().ConfigureAwait(true);

            try
            {
                var productInACart = await this._db.FoodProducts_Carts.Where(record => record.CartId == cartId).FirstOrDefaultAsync(record => record.ProductId == productId).ConfigureAwait(true);
                //Check if this Product exists in this Cart
                if (productInACart != null)
                {
                    //Increment quantity in cart
                    productInACart.QuantityInCart++;
                    await this._db.SaveChangesAsync().ConfigureAwait(true);

                    serviceResponse.Data = new ProductsInCartsVM()
                    {
                        ProductDescription = productInACart.FoodProduct.Name,
                        QuantityInCart = productInACart.QuantityInCart,
                        OriginalPrice = productInACart.FoodProduct.Price,
                        CartId = productInACart.CartId
                    };
                    serviceResponse.Success = true;
                    serviceResponse.Message = "This product already existed in your cart! The quantity has been increased!";

                    return serviceResponse;
                }

                // if this product was not already in this cart then add new entry into Products_In_Cart joint entity
                var newProductInACart = new FoodProduct_ShoppingCart()
                {
                    ProductId = productId,
                    CartId = cartId,
                    QuantityInCart = 1
                };
                this._db.FoodProducts_Carts.Add(newProductInACart);
                this._db.SaveChanges();
                serviceResponse.Data = new ProductsInCartsVM()
                {
                    ProductDescription = newProductInACart.FoodProduct.Name, //might cause error here
                    QuantityInCart = newProductInACart.QuantityInCart,
                    OriginalPrice = newProductInACart.FoodProduct.Price, //might cause error here
                    CartId = newProductInACart.CartId
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<CartWithProductsVM>> RemoveFromCart(int productId, int cartId)
        {
            // Again, assuming both productId and cartId are valid, the validation took place in controller
            var response = new ServiceResponse<CartWithProductsVM>();
            try
            {
                var listProductInACart = await this._db.FoodProducts_Carts.Where(record => record.CartId == cartId).ToListAsync().ConfigureAwait(true);
                //var productInACart = await this._db.FoodProducts_Carts.Where(record => record.ShoppingCartId == cartId).FirstOrDefaultAsync(record => record.FoodProductId == productId).ConfigureAwait(true);
                var productInACart = listProductInACart.First(record => record.ProductId == productId);
                this._db.FoodProducts_Carts.Remove(productInACart);
                await this._db.SaveChangesAsync().ConfigureAwait(true);

                //response.Data = this._db.FoodProducts_Carts.ToList();
                response.Data = this._cartService.GetCartWithProducts(cartId);
                response.Success = true;
                response.Message = $"The product with ID={productId} has been removed from your cart!";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceResponse<decimal> GetTotal(int cartId)
        {
            var response = new ServiceResponse<decimal>();
            try
            {
                var currentCart = this._cartService.GetCartWithProducts(cartId);
                var allProductsInCart = this._db.FoodProducts_Carts.Where(c => c.CartId == cartId).ToList();

                var total = decimal.Zero;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                foreach (var item in currentCart.ProductNames)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                {
                    var currentProduct = allProductsInCart.FirstOrDefault(p => p.ProductId == item.ProductId);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    total += item.Price * currentProduct.QuantityInCart;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }
                response.Data = total;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        //this should return the total price of the cart after discount
        public async Task<ServiceResponse<VoucherResponseModel>> ApplyVoucherToCart(string voucherCode, int cartId)
        {
            // assuming cartId is valid
            var response = new ServiceResponse<VoucherResponseModel>();
            try
            {
                // check if this cart is already discounted
                //var currentCart = this._cartService.GetCartById(cardId);
                var currentCart = this._db.ShoppingCarts.Where(c => c.Id == cartId).FirstOrDefault();
                var beforeDiscountTotal = this.GetTotal(cartId).Data;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (currentCart.IsDiscounted)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                {
                    response.Success = false;
                    response.Message = "Sorry! your cart is already discounted with another voucher!";
                    return response;
                }


                if (this.CheckVoucherExists(voucherCode))
                {
                    // voucherCode is real
                    var allCartItems = currentCart.FoodProduct_Carts.ToList();
                    var result = new VoucherResponseModel();
                    var discountedCartPrice = decimal.Zero;

                    switch (voucherCode)
                    {
                        case "10OFFPROMODRI":
                            // Bool: True if there are 10 or more Drinks Item in Cart, False otherwise, PromotionService worthy
                            var quantityCondition = this.Is10orMoreDrinksItemInCart(cartId);
                            if (!quantityCondition)
                            {
                                result.Applied = false;
                                result.FailedMessage = "You need to buy at least 10 or more Drinks Item!";
                            }

                            foreach (var cartItem in allCartItems)
                            {
                                // check for Drinks category in each cart item (perhaps there should be a seperate service for this?)
                                if (cartItem.FoodProduct.CategoryId == 3) // 'Drinks' Categories, perhaps another way to access this static entity?
                                {
                                    //var originalCost = this._foodProductService.GetOriginalCostbyFoodProductId(cartItem.FoodProductId);
                                    var originalCost = cartItem.FoodProduct.Price;
                                    var discountCostPerItem = (originalCost - (originalCost * Convert.ToDecimal(0.1))) * cartItem.QuantityInCart;
                                    discountedCartPrice += discountCostPerItem;
                                }

                            }

                            // mark the current cart as already discounted
                            currentCart.IsDiscounted = true;
                            this._db.ShoppingCarts.Update(currentCart);
                            //mark this coupon as used, PromotionService worthy
                            this._db.CouponCodes.First(c => c.CouponCode == voucherCode).IsClaimed = true;
                            await this._db.SaveChangesAsync().ConfigureAwait(true);

                            result.Applied = true;
                            result.FailedMessage = "Successfully applied Voucher to Cart!";
                            result.DiscountedCartPrice = discountedCartPrice;

                            response.Data = result;
                            break;
                        case "5OFFBAKING":
                            // conditions checking
                            // fetch All Baking/Cooking Ingredient items from Cart
                            var bakingCookingItems = currentCart.FoodProduct_Carts.Where(cartItem => cartItem.FoodProduct.CategoryId == 5).ToList();
                            var bakingCookingTotal = bakingCookingItems.Sum(each => each.FoodProduct.Price * each.QuantityInCart);
                            if (bakingCookingTotal < Convert.ToDecimal(50))
                            {
                                result.Applied = false;
                                result.FailedMessage = "Failed to apply this coupon! You need to spend £50.00 or more on Baking/Cooking Ingredients";
                                result.DiscountedCartPrice = 0;
                            }
                            // apply discount percentage
                            result.Applied = true;
                            result.FailedMessage = "Successfully applied Voucher to Cart!";
                            result.DiscountedCartPrice = beforeDiscountTotal - Convert.ToDecimal(5);

                            response.Data = result;
                            break;
                        case "20OFFPROMO":
                            // condition check, spending at least 100 pounds in total
                            if (beforeDiscountTotal < Convert.ToDecimal(100))
                            {
                                result.Applied = false;
                                result.FailedMessage = "Failed to apply this coupon! You need to spend at least £100.00 or more in total!";
                                result.DiscountedCartPrice = 0;
                            }
                            else
                            {
                                result.Applied = true;
                                result.FailedMessage = "Successfully applied Voucher to Cart!";
                                result.DiscountedCartPrice = beforeDiscountTotal - Convert.ToDecimal(20);
                            }

                            response.Data = result;
                            break;
                        default:
                            result.Applied = false;
                            result.FailedMessage = "Invalid Coupon Code";
                            result.DiscountedCartPrice = 0;
                            response.Data = result;
                            break;
                    }
                }
                else
                {
                    // voucherCode is invalid
                    response.Success = false;
                    response.Message = "This voucher code is Invalid! Please use another code!";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        //True if there are 10 or more Drinks Item in Cart, False otherwise
        public bool Is10orMoreDrinksItemInCart(int cartId)
        {
            var currentCart = this._cartService.GetCartById(cartId).Data;

            var products = currentCart.Products.Select(item => this._db.Products.First(p => p.Id == item)).ToList();

            if (products.Where(fp => fp.CategoryId == 3).Count() < 10)
            {
                return false;
            }
            return true;
        }

        public async Task<ServiceResponse<List<FoodProduct_ShoppingCart>>> GetAllProductsInCarts()
        {
            var serviceResponse = new ServiceResponse<List<FoodProduct_ShoppingCart>>();
            try
            {
                var allCartItems = await this._db.FoodProducts_Carts.ToListAsync().ConfigureAwait(true);

                serviceResponse.Data = allCartItems;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public bool CheckVoucherExists(string code) => this._db.CouponCodes.Any(promo => promo.CouponCode == code);
    }
}
