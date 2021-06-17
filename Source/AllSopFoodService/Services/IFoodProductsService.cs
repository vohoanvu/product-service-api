namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public interface IFoodProductsService
    {
        Task<List<FoodProductDTO>> GetFoodProductsAsync();
        Task<FoodProduct> GetFoodProductByIdAsync(int id);
        Task<FoodProduct> UpdateFoodProductAsync(int id, FoodProductDTO foodProductDto);
        Task<FoodProduct> CreateFoodProductAsync(FoodProductDTO foodProductDto);

        Task<bool> IsFoodProductInStockAsync(int id);

        void DecrementProductStockUnit(int id);

        bool FoodProductExists(int id);
        decimal GetOriginalCostbyFoodProductId(int id);

        Task<bool> RemoveFoodProduct(FoodProduct foodProduct);
    }
}
