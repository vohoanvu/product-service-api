namespace AllSopFoodService.Repositories.Interfaces
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        ICartRepository ShoppingCarts { get; }
        //still need interface for FoodProduct_ShoppingCart entity
        IProductinShoppingCartRepository ProductsInCarts { get; }
        IPromotionRepository Promotions { get; }
        int Complete();
    }
}
