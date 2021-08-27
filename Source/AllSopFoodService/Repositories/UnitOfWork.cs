namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly FoodDBContext context;

        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public ICartRepository ShoppingCarts { get; private set; }

        public IProductinShoppingCartRepository ProductsInCarts { get; private set; }
        public IPromotionRepository Promotions { get; private set; }
        public UnitOfWork(FoodDBContext context)
        {
            this.context = context;
            this.Products = new ProductRepository(context);
            this.Categories = new CategoryRepository(context);
            this.ShoppingCarts = new ShoppingCartRepository(context);
            this.ProductsInCarts = new ProductinShoppingCartRepository(context);
            this.Promotions = new PromotionRepository(context);
        }

        public int Complete() => this.context.SaveChanges();

        public void Dispose() => this.context.Dispose();
    }
}
