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
                CartLabel = cart.CartLabel
            };

            this._db.ShoppingCarts.Add(newcart);
            this._db.SaveChanges();
        }

        public CartWithProductsVM GetCartWithProducts(int cartId)
        {
            var cart = this._db.ShoppingCarts.Where(c => c.Id == cartId).Select(c => new CartWithProductsVM()
            {
                CartLabel = c.CartLabel,
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
