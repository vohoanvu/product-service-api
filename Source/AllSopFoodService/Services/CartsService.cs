#nullable disable
namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.ViewModels.UserAuth;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class CartsService : ICartsService
    {
        private readonly FoodDBContext _db;
        private readonly IMapper<ShoppingCart, CartVM> cartMapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUnitOfWork unitOfWork;

        public CartsService(FoodDBContext dbcontext, IMapper<ShoppingCart, CartVM> mapper, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            this._db = dbcontext;
            this.cartMapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
        }

        //since each cart is associated with a specific user, we're gonna need lots of User Id
        private int GetUserId() => int.Parse(this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        // Create a new cart for the currently logged-in user
        // I really just need to a UserID to be able to create a new cart,
        // need to pass in newUserId since the new Id might have not been created yet
        public ServiceResponse<ShoppingCart> CreateShoppingCart()
        {
            var response = new ServiceResponse<ShoppingCart>();
            var currentUser = this.GetUserId();
            // need to check if the new User has already been assigned a Shopping Cart
            var cartExist = this.unitOfWork.ShoppingCarts.CheckUserCart(currentUser);
            if (cartExist)
            {
                response.Data = this.unitOfWork.ShoppingCarts.GetShoppingCartByUserId(currentUser);
                response.Success = false;
                response.Message = $"The User with ID {currentUser} was already assigned a Shopping Cart";

                return response;
            }

            var newcart = new ShoppingCart()
            {
                CartLabel = $"This Shopping Cart is owned by {currentUser}",
                IsDiscounted = false,
                UserId = currentUser
            };

            //this._db.ShoppingCarts.Add(newcart);
            //this._db.SaveChanges();
            this.unitOfWork.ShoppingCarts.Add(newcart);
            this.unitOfWork.Complete();


            //perhaps a ServiceResponse wrapper here is way over the top
            response.Data = this.unitOfWork.ShoppingCarts.GetShoppingCartByUserId(currentUser);
            response.Success = true;
            response.Message = "A new shopping cart has also been created for this currently authenticated User";

            return response;
        }

        public async Task<ServiceResponse<List<CartVM>>> GetAllCarts()
        {
            var response = new ServiceResponse<List<CartVM>>();

            try
            {
                var allUserCarts = await this.unitOfWork.ShoppingCarts.GetAllCartsWithProductData()
                                                .Select(cart => this.cartMapper.Map(cart))
                                                .ToListAsync().ConfigureAwait(true);
                //var allUserCarts = await this._db.ShoppingCarts.Include(c => c.FoodProduct_Carts).Select(cart => this.cartMapper.Map(cart)).ToListAsync().ConfigureAwait(true);

                response.Data = allUserCarts;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceResponse<ShoppingCart> GetSingleCartByCurrentUser()
        {
            var response = new ServiceResponse<ShoppingCart>();

            var theCart = this.unitOfWork.ShoppingCarts.GetCartWithProductDataByUserId(this.GetUserId());

            if (theCart != null)
            {
                response.Data = theCart;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "Cannot find a Cart for this User";
            }

            return response;
        }

        public ServiceResponse<ShoppingCart> GetCartById(int cartId)
        {
            var response = new ServiceResponse<ShoppingCart>();
            var cart = this.unitOfWork.ShoppingCarts.GetShoppingCartByCartId(cartId);
            if (cart != null)
            {
                response.Data = cart;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "Cannot find a Cart with this ID";
            }

            return response;
        }

        // Get Cart with Products Info for currently logged-in user
        public ServiceResponse<CartVM> GetCartWithProducts()
        {
            var response = new ServiceResponse<CartVM>();
            //querying ShoppingCart including related Products List
            var cart = this.unitOfWork.ShoppingCarts.GetCartWithProductDataByUserId(this.GetUserId());

            if (cart != null)
            {
                // mapping from ShoppingCart to CartVM
                var cartView = this.cartMapper.Map(cart);
                // mapping from list of FoodProduct_Carts into list of FoodProduct, overriding the default ProductsInCart
                cartView.ProductsInCart = cart.FoodProduct_Carts.Select(pc => new ProductsInCartsVM()
                {
                    ProductDescription = pc.FoodProduct.Name,
                    QuantityInCart = pc.QuantityInCart,
                    OriginalPrice = pc.FoodProduct.Price,
                    CartId = pc.CartId
                }).ToList();

                response.Data = cartView;
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = "Cannot find a Cart for this User ID";
            }

            return response;
        }


        public ServiceResponse<ProductsInCartsVM> AddToCart(int productId)
        {
            // assuming productId is both valid, validation are executed in controller
            var serviceResponse = new ServiceResponse<ProductsInCartsVM>();

            try
            {
                // retrieve the Cart owned by the currently logged-in User
                var yourCart = this.unitOfWork.ShoppingCarts.GetShoppingCartByUserId(this.GetUserId());
                //Check if this Product exists in this Cart
                var isProductInCart = yourCart.FoodProduct_Carts.FirstOrDefault(p => p.ProductId == productId);
                if (isProductInCart != null)
                {
                    //Increment quantity in cart
                    isProductInCart.QuantityInCart++;
                    this.unitOfWork.Complete();

                    serviceResponse.Data = new ProductsInCartsVM()
                    {
                        ProductDescription = isProductInCart.FoodProduct.Name,
                        QuantityInCart = isProductInCart.QuantityInCart,
                        OriginalPrice = isProductInCart.FoodProduct.Price,
                        CartId = isProductInCart.CartId
                    };
                    serviceResponse.Success = true;
                    serviceResponse.Message = "This product already existed in your cart! The quantity has been increased!";

                    return serviceResponse;
                }

                // if this product was NOT already in this cart then add new entry into Products_In_Cart joint entity
                var newProductInACart = new FoodProduct_ShoppingCart()
                {
                    ProductId = productId,
                    CartId = yourCart.Id,
                    QuantityInCart = 1
                };
                //this._db.FoodProducts_Carts.Add(newProductInACart);
                //this._db.SaveChanges();
                this.unitOfWork.ProductsInCarts.Add(newProductInACart);
                this.unitOfWork.Complete();

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


        public ServiceResponse<CartVM> RemoveFromCart(int productId)
        {
            // Again, assuming productId is Valid, the validation took place in controller
            var response = new ServiceResponse<CartVM>();
            try
            {
                // retrieve the Cart owned by the currently logged-in User
                var yourCart = this.unitOfWork.ShoppingCarts.GetShoppingCartByUserId(this.GetUserId());
                //Get the Product association with this Cart
                var isProductInCart = yourCart.FoodProduct_Carts.FirstOrDefault(p => p.ProductId == productId);

                //this._db.FoodProducts_Carts.Remove(isProductInCart);
                //await this._db.SaveChangesAsync().ConfigureAwait(true);
                this.unitOfWork.ProductsInCarts.Delete(isProductInCart);
                this.unitOfWork.Complete();

                var cartResponse = this.GetCartWithProducts();
                response.Data = cartResponse.Data;
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


        //this should return the total price of the cart after discount, not yet refactored
        public ServiceResponse<VoucherResponseModel> ApplyVoucherToCart(string voucherCode)
        {
            var response = new ServiceResponse<VoucherResponseModel>();
            try
            {
                // check if this cart is already discounted
                var currentCart = this.unitOfWork.ShoppingCarts.GetCartWithProductDataByUserId(this.GetUserId());
                //var currentCart = this._db.ShoppingCarts.Include(c => c.FoodProduct_Carts).Where(c => c.Id == cartId).FirstOrDefault();
                var beforeDiscountTotal = this.GetTotal().Data;

                if (currentCart.IsDiscounted)
                {
                    response.Success = false;
                    response.Message = "Sorry! your cart is already discounted with another voucher!";
                    return response;
                }


                if (this.unitOfWork.Promotions.CheckVoucherExist(voucherCode))
                {
                    // voucherCode is real
                    var allCartItems = currentCart.FoodProduct_Carts.ToList();
                    var result = new VoucherResponseModel();
                    var discountedCartPrice = decimal.Zero;

                    switch (voucherCode)
                    {
                        case "10OFFPROMODRI":
                            // Bool: True if there are 10 or more Drinks Item in Cart, False otherwise, PromotionService worthy
                            var quantityCondition = Is10orMoreDrinksItemInCart(allCartItems); // could have just passed in AllCartItems
                            if (!quantityCondition)
                            {
                                result.Applied = false;
                                result.FailedMessage = "You need to buy at least 10 or more Drinks Item!";
                                response.Data = result;
                                break;
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
                            this.unitOfWork.ShoppingCarts.Update(currentCart);
                            //mark this coupon as used, PromotionService worthy
                            this.unitOfWork.Promotions.GetCouponByCode(voucherCode).IsClaimed = true;
                            this.unitOfWork.Complete();

                            result.Applied = true;
                            result.FailedMessage = "Successfully applied Voucher to Cart!";
                            result.DiscountedCartPrice = discountedCartPrice;

                            response.Data = result;
                            break;
                        case "5OFFPROMOALL":
                            // conditions checking
                            // fetch All Baking/Cooking Ingredient items from Cart
                            var bakingCookingItems = currentCart.FoodProduct_Carts.Where(cartItem => cartItem.FoodProduct.CategoryId == 5).ToList();
                            var bakingCookingTotal = bakingCookingItems.Sum(each => each.FoodProduct.Price * each.QuantityInCart);
                            if (bakingCookingTotal < Convert.ToDecimal(50))
                            {
                                result.Applied = false;
                                result.FailedMessage = "Failed to apply this coupon! You need to spend £50.00 or more on Baking/Cooking Ingredients";
                                result.DiscountedCartPrice = 0;

                                response.Data = result;
                                break;
                            }
                            // apply discount percentage
                            result.Applied = true;
                            result.FailedMessage = "Successfully applied Voucher to Cart!";
                            result.DiscountedCartPrice = beforeDiscountTotal - Convert.ToDecimal(5);

                            response.Data = result;
                            break;
                        case "20OFFPROMOALL":
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


        public ServiceResponse<decimal> GetTotal()
        {
            var response = new ServiceResponse<decimal>();
            try
            {
                var currentCart = this.unitOfWork.ShoppingCarts.GetCartWithProductDataByUserId(this.GetUserId());
                var allProductsInCart = currentCart.FoodProduct_Carts.ToList();

                var total = decimal.Zero;

                foreach (var item in allProductsInCart)
                {
                    //var currentProduct = this._db.Products.First(fp => fp.Id == item.ProductId);
                    total += item.FoodProduct.Price * item.QuantityInCart;
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

        //True if there are 10 or more Drinks Item in Cart, False otherwise
        private static bool Is10orMoreDrinksItemInCart(List<FoodProduct_ShoppingCart> productsIncart) // parameter changed => List<FoodProduct_Cart> 
        {
            //var currentCart = this._cartService.GetCartById(cartId).Data;
            //var allProductsInCart = this._db.FoodProducts_Carts.Where(c => c.CartId == cartId).ToList();
            var numOfDrinksItems = productsIncart.Where(fp => fp.FoodProduct.CategoryId == 3).Sum(item => item.QuantityInCart);

            if (numOfDrinksItems < 10)
            {
                return false;
            }
            return true;
        }

    }
}
