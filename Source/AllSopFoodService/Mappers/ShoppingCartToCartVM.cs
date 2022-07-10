namespace AllSopFoodService.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Boxed.Mapping;
    using Model;
    using ViewModels;

    public class ShoppingCartToCartVM : IMapper<ShoppingCart, CartVM>
    {
        public void Map(ShoppingCart source, CartVM destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.CartId = source.Id;
            destination.CartLabel = source.CartLabel;
            destination.IsDiscounted = source.IsDiscounted;
            destination.UserId = source.UserId;
            destination.ProductsInCart = source.FoodProductCarts != null ? source.FoodProductCarts
                                            .Select(prodCart => new ProductsInCartsVM
                                            {
                                                ProductDescription = prodCart.FoodProduct.Name,
                                                QuantityInCart = prodCart.QuantityInCart,
                                                OriginalPrice = prodCart.FoodProduct.Price,
                                                CartId = prodCart.CartId
                                            }).ToList() : new List<ProductsInCartsVM>();
        }
    }
}
