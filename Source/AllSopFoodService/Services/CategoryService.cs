#nullable disable
namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Model.Paging;
    using AllSopFoodService.Repositories.Interfaces;
    using AllSopFoodService.ViewModels;
    using Boxed.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class CategoryService : ICategoryService
    {
        private readonly IMapper<Product, FoodProductVM> productMapper;
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IMapper<Product, FoodProductVM> productMapper, IUnitOfWork unitOfWork)
        {
            this.productMapper = productMapper;
            this.unitOfWork = unitOfWork;
        }


        public void CreateCategory(CategoryVM cartegory)
        {
            var newCategory = new Category()
            {
                Label = cartegory.Label,
                IsAvailable = cartegory.IsAvailable
            };

            //this._db.Categories.Add(newCategory);
            //this._db.SaveChanges();
            this.unitOfWork.Categories.Add(newCategory);
            this.unitOfWork.Complete();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var allCategories = await this.unitOfWork.Categories.GetAllAsync().ConfigureAwait(true);
            allCategories = allCategories.OrderBy(n => n.Label).ToList();

            return allCategories.ToList();
        }

        public CategoryWithProducts GetCategoryData(int categoryId)
        {
            //var categoryData = this._db.Categories.Include(c => c.FoodProducts.Where(p => p.CategoryId == categoryId)).ThenInclude(fp => fp.Category)
            var categoryData = this.unitOfWork.Categories.GetCategoryDataWithEagerLoad(categoryId)
                                                    .Select(c => new CategoryWithProducts()
                                                    {
                                                        Label = c.Label,
                                                        Products = c.FoodProducts.Select(fp => this.productMapper.Map(fp)).ToList()
                                                    }).FirstOrDefault();

            return categoryData;
        }

        public void DeleteCategoryById(int id)
        {
            var category = this.unitOfWork.Categories.GetById(id);

            if (category != null)
            {
                //this._db.Remove(category);
                //this._db.SaveChanges();
                this.unitOfWork.Categories.Delete(category);
                this.unitOfWork.Complete();
            }
        }

    }
}
