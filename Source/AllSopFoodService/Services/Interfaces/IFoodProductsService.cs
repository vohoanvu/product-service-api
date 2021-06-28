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
        Task<ServiceResponse<List<FoodProductVM>>> GetAllFoodProducts(string? sortBy, string? searchString, int? pageNum, int? pageSize);
        Task<ServiceResponse<FoodProductVM>> GetFoodProductById(int id);
        Task<ServiceResponse<FoodProductVM>> UpdateFoodProduct(int id, FoodProductDTO foodProductDto);
        Task<ServiceResponse<List<FoodProductVM>>> CreateFoodProduct(FoodProductDTO foodProductDto);
        Task<ServiceResponse<List<FoodProductVM>>> RemoveFoodProductById(int id);
        ServiceResponse<FoodProduct> IsFoodProductInStock(int id);
        void DecrementProductStockUnit(int id);
        decimal GetOriginalCostbyFoodProductId(int id);
    }
}
