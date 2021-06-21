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
        Task<List<FoodProductVM>> GetFoodProductsAsync(string? sortBy, string? searchString, int? pageNum, int? pageSize);
        FoodProductVM GetFoodProductById(int id);
        FoodProduct UpdateFoodProduct(int id, FoodProductDTO foodProductDto);
        void CreateFoodProduct(FoodProductDTO foodProductDto);

        Task<bool> IsFoodProductInStockAsync(int id);

        void DecrementProductStockUnit(int id);

        bool FoodProductExists(int id);
        decimal GetOriginalCostbyFoodProductId(int id);

        bool RemoveFoodProductById(int id);
    }
}
