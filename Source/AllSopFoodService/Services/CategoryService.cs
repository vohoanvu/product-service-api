namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;

    public class CategoryService : ICategoryService
    {
        private readonly FoodDBContext _db;

        public CategoryService(FoodDBContext dbcontext) => this._db = dbcontext;

        public void CreateCategory(CategoryVM cartegory)
        {
            var newCategory = new Category()
            {
                Label = cartegory.Label,
                IsAvailable = cartegory.IsAvailable
            };

            this._db.Categories.Add(newCategory);
            this._db.SaveChanges();
        }

        public CategoryWithProductsAndCartsVM GetCategoryData(int categoryId)
        {
            var categoryData = this._db.Categories.Where(c => c.Id == categoryId)
                                                    .Select(c => new CategoryWithProductsAndCartsVM()
                                                    {
                                                        Label = c.Label,
                                                        ProductCarts = c.FoodProducts.Select(fp => new Product_CartVM()
                                                        {
                                                            ProductName = fp.Name,
                                                            ProductCartLabels = fp.FoodProduct_Carts.Select(n => n.ShoppingCart.CartLabel).ToList()
                                                        }).ToList()
                                                    }).FirstOrDefault();

#pragma warning disable CS8603 // Possible null reference return.
            return categoryData;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void DeleteCategoryById(int id)
        {
            var category = this._db.Categories.FirstOrDefault(c => c.Id == id);

            if (category != null)
            {
                this._db.Remove(category);
                this._db.SaveChanges();
            }
        }

    }
}
