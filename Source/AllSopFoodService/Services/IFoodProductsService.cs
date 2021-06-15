namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;

    public interface IFoodProductsService
    {
        Task<List<FoodProductDTO>> GetFoodProductsAsync();
        Task<FoodProduct> GetFoodProductByIdAsync(int id);

        Task<FoodProduct> CreateFoodProductAsync(FoodProductDTO foodProductDto);

        Task<bool> IsFoodProductInStockAsync(int id);

        void DecrementProductStockUnit(int id);

        decimal GetOriginalCostbyFoodProductId(int id);
    }
}
