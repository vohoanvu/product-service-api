#nullable disable
namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Model.Paging;
    using AllSopFoodService.ViewModels;
    using Boxed.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly FoodDBContext _db;
        private readonly IMapper<Product, FoodProductVM> productMapper;

        public CategoryService(FoodDBContext dbcontext, IMapper<Product, FoodProductVM> productMapper)
        {
            this._db = dbcontext;
            this.productMapper = productMapper;
        }


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

        public List<Category> GetAllCategories()
        {
            var allCategories = this._db.Categories.OrderBy(n => n.Label).ToList();

            return allCategories;
        }

        public CategoryWithProducts GetCategoryData(int categoryId)
        {
            var categoryData = this._db.Categories.Include(c => c.FoodProducts.Where(p => p.CategoryId == categoryId)).ThenInclude(fp => fp.Category)
                                                    .Select(c => new CategoryWithProducts()
                                                    {
                                                        Label = c.Label,
                                                        Products = c.FoodProducts.Select(fp => this.productMapper.Map(fp)).ToList()
                                                    }).FirstOrDefault();

            return categoryData;
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
