namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class CartsService : ICartsService
    {
        private readonly FoodDBContext _db;

        public CartsService(FoodDBContext dbcontext) => this._db = dbcontext;

        public ServiceResponse<List<CartVM>> CreateShoppingCart(AddCartDto cart)
        {
            var newcart = new ShoppingCart()
            {
                CartLabel = cart.CartLabel,
                UserName = cart.User,
                IsDiscounted = cart.IsDiscounted
            };

            this._db.ShoppingCarts.Add(newcart);
            this._db.SaveChanges();

            var response = new ServiceResponse<List<CartVM>>
            {
                Data = this._db.ShoppingCarts.Select(c => new CartVM()
                {
                    CartId = c.Id,
                    CartLabel = c.CartLabel,
                    IsDiscounted = c.IsDiscounted,
                    User = c.UserName,
                    Products = c.FoodProduct_Carts != null ? c.FoodProduct_Carts.Select(fp => fp.ProductId).ToList() : new List<int>()
                }).ToList()
            };

            return response;
        }

        public async Task<ServiceResponse<List<CartVM>>> GetAllCarts()
        {
            var response = new ServiceResponse<List<CartVM>>();

            try
            {
                var allUserCarts = await this._db.ShoppingCarts.ToListAsync().ConfigureAwait(true);

                response.Data = allUserCarts.Select(c => new CartVM()
                {
                    CartId = c.Id,
                    CartLabel = c.CartLabel,
                    IsDiscounted = c.IsDiscounted,
                    User = c.UserName,
                    Products = c.FoodProduct_Carts != null ? c.FoodProduct_Carts.Select(fp => fp.ProductId).ToList() : new List<int>()
                }).ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public ServiceResponse<CartVM> GetCartById(int cartId)
        {
            var response = new ServiceResponse<CartVM>();
            var cart = this._db.ShoppingCarts.Where(c => c.Id == cartId).Select(c => new CartVM()
            {
                CartId = c.Id,
                CartLabel = c.CartLabel,
                IsDiscounted = c.IsDiscounted,
                User = c.UserName,
                Products = c.FoodProduct_Carts.Select(foodcart => foodcart.Id).ToList()
            }).FirstOrDefault();
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

        public CartWithProductsVM GetCartWithProducts(int cartId)
        {
            var cart = this._db.ShoppingCarts.Where(c => c.Id == cartId).Select(c => new CartWithProductsVM()
            {
                CartLabel = c.CartLabel,
                IsDiscounted = c.IsDiscounted,
                ProductNames = c.FoodProduct_Carts != null ? c.FoodProduct_Carts
                                .Select(foodcart => new FoodProductVM()
                                {
                                    ProductId = foodcart.ProductId,
                                    Name = foodcart.FoodProduct.Name,
                                    Price = foodcart.FoodProduct.Price,
                                    CategoryName = foodcart.FoodProduct.Category.Label,
                                    Quantity = foodcart.FoodProduct.Quantity
                                }).ToList() : new List<FoodProductVM>()
            }).FirstOrDefault();

#pragma warning disable CS8603 // Possible null reference return.
            return cart;
#pragma warning restore CS8603 // Possible null reference return.
        }

    }
}
