namespace AllSopFoodService.Mappers
{
    using System;
    using Boxed.Mapping;
    using Model;
    using ViewModels;

    public class FoodProductToVMMapper : IMapper<Product, FoodProductVM>
    {
        //private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly LinkGenerator linkGenerator;

        public void Map(Product source, FoodProductVM destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.ProductId = source.Id;
            destination.Name = source.Name;
            destination.Price = source.Price;
            destination.Quantity = source.Quantity;
            destination.CategoryId = source.CategoryId;
            destination.CategoryName = source.Category.Label;
            //destination.ShoppingCartNames = source.FoodProduct_Carts.Select(n => n.ShoppingCart != null ? n.ShoppingCart.CartLabel : "empty").ToList()
        }
    }
}
