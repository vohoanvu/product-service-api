namespace AllSopFoodService.Mappers
{
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;
    using AutoMapper;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<FoodProduct, FoodProductVM>();
            this.CreateMap<FoodProductDTO, FoodProduct>();
        }
    }
}
