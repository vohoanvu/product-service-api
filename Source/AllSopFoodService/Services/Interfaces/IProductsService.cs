namespace AllSopFoodService.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;

    public interface IProductsService
    {
        Task<ServiceResponse<List<FoodProductVM>>> GetAllProductsAsync(string sortBy, string searchString, int pageNum, int pageSize);
        Task<ServiceResponse<FoodProductVM>> GetFoodProductByIdAsync(int id);
        Task<ServiceResponse<FoodProductVM>> UpdateFoodProductAsync(int id, ProductSaves foodProductDto);
        Task<ServiceResponse<List<FoodProductVM>>> CreateFoodProductAsync(ProductSaves foodProductDto);
        Task<ServiceResponse<List<FoodProductVM>>> RemoveFoodProductByIdAsync(int id);
        ServiceResponse<Product> IsFoodProductInStock(int id);
        void DecrementProductStockUnit(int id);
        decimal GetOriginalCostbyFoodProductId(int id);
    }
}
