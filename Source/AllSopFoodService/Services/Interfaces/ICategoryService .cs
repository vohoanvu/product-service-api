namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;

    public interface ICategoryService
    {
        void CreateCategory(CategoryVM cartegory);

        Task<List<Category>> GetAllCategoriesAsync();

        CategoryWithProducts GetCategoryData(int categoryId);

        void DeleteCategoryById(int id);
    }
}
