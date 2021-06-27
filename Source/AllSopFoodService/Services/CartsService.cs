namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;

    public class CartsService : ICartsService
    {
        private readonly FoodDBContext _db;

        public CartsService(FoodDBContext dbcontext) => this._db = dbcontext;

        public void CreateShoppingCart(CartVM cart)
        {
            var newcart = new ShoppingCart()
            {
                CartLabel = cart.CartLabel,
                UserName = cart.User,
                IsDiscounted = cart.IsDiscounted
            };

            this._db.ShoppingCarts.Add(newcart);
            this._db.SaveChanges();
        }

        public ServiceResponse<ShoppingCart> GetCartById(int cartId)
        {
            var response = new ServiceResponse<ShoppingCart>();
            var cart = this._db.ShoppingCarts.First(c => c.Id == cartId);
            if (cart != null)
            {
                response.Data = cart;
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
                                .Select(foodcart => foodcart.FoodProduct != null ? foodcart.FoodProduct.Name : "empty")
                                .ToList() : new List<string>()
            }).FirstOrDefault();

#pragma warning disable CS8603 // Possible null reference return.
            return cart;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
