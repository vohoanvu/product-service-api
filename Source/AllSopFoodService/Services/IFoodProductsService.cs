namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;

    public interface IFoodProductsService
    {
        Task<FoodProduct> GetFoodProductByIdAsync(int id);

        Task<bool> IsFoodProductInStockAsync(int id);

        void DecrementProductStockUnit(int id);
    }
}
