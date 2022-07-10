namespace AllSopFoodService
{
    using Commands;
    using Mappers;
    using Model;
    using Repositories;
    using Services;
    using ViewModels;
    using Boxed.Mapping;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Http;
    using Repositories.Interfaces;
    using Services.Interfaces;
    using Car = Model.Car;
    using ViewCar = ViewModels.Car;

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
                .AddSingleton<IMapper<Car, ViewCar>, CarToCarMapper>()
                .AddSingleton<IMapper<Car, SaveCar>, CarToSaveCarMapper>()
                .AddSingleton<IMapper<SaveCar, Car>, CarToSaveCarMapper>()
                .AddSingleton<IMapper<Product, FoodProductVM>, FoodProductToVMMapper>()
                .AddSingleton<IMapper<ShoppingCart, CartVM>, ShoppingCartToCartVM>();

        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddSingleton<ICarRepository, CarRepository>()
                .AddScoped<IAuthRepository, AuthRepository>()
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped<ICartRepository, ShoppingCartRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<IProductsService, ProductsService>()
                .AddScoped<ICartsService, CartsService>()
                .AddScoped<ICategoryService, CategoryService>();
    }
}
