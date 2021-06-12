namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using Microsoft.EntityFrameworkCore;

    public class FoodProductsService
    {
        private readonly FoodDBContext db;

        public FoodProductsService(FoodDBContext dbcontext) => this.db = dbcontext;

        public async Task<FoodProduct> GetFoodProductByIdAsync(int id)
        {
            //var productInStock = await this._db.FoodProducts.FindAsync(productId).ConfigureAwait(true);
            var foodProduct = await this.db.FoodProducts.FindAsync(id).ConfigureAwait(true);

            return foodProduct;
        }

        public void DecrementProductStockUnit(int id)
        {
            var currentFoodProd = this.db.FoodProducts.Find(id);

            currentFoodProd.Quantity--;
        }

        public async Task<bool> IsFoodProductInStockAsync(int id) => await this.db.FoodProducts.AnyAsync(fp => fp.Id == id && fp.Quantity > 0).ConfigureAwait(true);
    }
}
