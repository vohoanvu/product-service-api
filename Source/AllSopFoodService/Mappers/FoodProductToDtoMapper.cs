namespace AllSopFoodService.Mappers
{
    using System;
    using Boxed.Mapping;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.Model;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class FoodProductToDtoMapper : IMapper<FoodProduct, FoodProductDTO>
    {
        //private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly LinkGenerator linkGenerator;

        public FoodProductToDtoMapper()
        {

        }
        public void Map(FoodProduct source, FoodProductDTO destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.FoodId = source.Id;
            destination.Name = source.Name;
            destination.Price = source.Price;
            destination.Quantity = source.Quantity;
            destination.InCart = source.IsInCart;
            destination.CategoryName = source.Category.Label;
            destination.CategoryId = source.CategoryId;
        }
    }
}
