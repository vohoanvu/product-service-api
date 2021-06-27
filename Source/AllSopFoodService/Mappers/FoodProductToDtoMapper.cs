namespace AllSopFoodService.Mappers
{
    using System;
    using Boxed.Mapping;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.Model;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class FoodProductToVMMapper : IMapper<FoodProduct, FoodProductVM>
    {
        //private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly LinkGenerator linkGenerator;

        public FoodProductToVMMapper()
        {

        }
        public void Map(FoodProduct source, FoodProductVM destination)
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
            destination.CategoryName = source.Category.Label;
            //destination.ShoppingCartNames = source.FoodProduct_Carts.Select(n => n.ShoppingCart != null ? n.ShoppingCart.CartLabel : "empty").ToList()
        }
    }
}
