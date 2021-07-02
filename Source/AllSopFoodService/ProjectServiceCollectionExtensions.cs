namespace AllSopFoodService
{
    using AllSopFoodService.Commands;
    using AllSopFoodService.Mappers;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories;
    using AllSopFoodService.Services;
    using AllSopFoodService.ViewModels;
    using Boxed.Mapping;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods add project services.
    /// </summary>
    /// <remarks>
    /// AddSingleton - Only one instance is ever created and returned.
    /// AddScoped - A new instance is created and returned for each request/response cycle.
    /// AddTransient - A new instance is created and returned each time.
    /// </remarks>
    public static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectCommands(this IServiceCollection services) =>
            services
                .AddSingleton<DeleteCarCommand>()
                .AddSingleton<GetCarCommand>()
                .AddSingleton<GetCarPageCommand>()
                .AddSingleton<PatchCarCommand>()
                .AddSingleton<PostCarCommand>()
                .AddSingleton<PutCarCommand>();

        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services
                .AddSingleton<IMapper<Models.Car, Car>, CarToCarMapper>()
                .AddSingleton<IMapper<Models.Car, SaveCar>, CarToSaveCarMapper>()
                .AddSingleton<IMapper<SaveCar, Models.Car>, CarToSaveCarMapper>()
                .AddSingleton<IMapper<Product, FoodProductVM>, FoodProductToVMMapper>()
                .AddSingleton<IMapper<ShoppingCart, CartVM>, ShoppingCartToCartVM>();

        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddSingleton<ICarRepository, CarRepository>()
                .AddScoped<IAuthRepository, AuthRepository>();

        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}
