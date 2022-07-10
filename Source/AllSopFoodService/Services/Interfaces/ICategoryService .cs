namespace AllSopFoodService.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using ViewModels;

    public interface ICategoryService
    {
        void CreateCategory(CategoryVM cartegory);

        Task<List<Category>> GetAllCategoriesAsync();

        CategoryWithProducts GetCategoryData(int categoryId);

        void DeleteCategoryById(int id);
    }
}
