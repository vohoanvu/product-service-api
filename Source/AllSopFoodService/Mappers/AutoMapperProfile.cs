namespace AllSopFoodService.Mappers
{
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;
    using AutoMapper;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Product, FoodProductVM>();
            this.CreateMap<ProductSaves, Product>();
        }
    }
}
