namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class FoodProductsService : IFoodProductsService
    {
        private readonly FoodDBContext db;

        public FoodProductsService(FoodDBContext dbcontext) => this.db = dbcontext;

        //Perhaps this should be placed at the repo layer?
        public async Task<List<FoodProductDTO>> GetFoodProductsAsync()
        {
            // Query Data via EF core DbSet
            // transform results to dto object (non-entity type)
            var foodItems = await this.db.FoodProducts.AsNoTracking().Select(fooditem => new FoodProductDTO()
            {
                FoodId = fooditem.Id,
                Name = fooditem.Name,
                Price = fooditem.Price,
                Quantity = fooditem.Quantity,
                InCart = fooditem.IsInCart,
                CategoryName = fooditem.Category.Label,
                CategoryId = fooditem.CategoryId
            }).ToListAsync().ConfigureAwait(true);

            return foodItems;
        }

        public async Task<FoodProduct> GetFoodProductByIdAsync(int id)
        {
            //var productInStock = await this._db.FoodProducts.FindAsync(productId).ConfigureAwait(true);
            var foodProduct = await this.db.FoodProducts.FindAsync(id).ConfigureAwait(true);

            return foodProduct;
        }

        public async Task<FoodProduct> CreateFoodProductAsync(FoodProductDTO foodProductDto)
        {
            var foodProduct = new FoodProduct()
            {
                //Id = foodProductDto.FoodId,
                Name = foodProductDto.Name,
                Price = foodProductDto.Price,
                Quantity = foodProductDto.Quantity,
                IsInCart = foodProductDto.InCart,
                CategoryId = foodProductDto.CategoryId
            };

            this.db.FoodProducts.Add(foodProduct);
            await this.db.SaveChangesAsync().ConfigureAwait(true);

            return foodProduct;
            //return createdataction("getfoodproduct", new { id = foodproduct.id }, foodproduct);
        }

        public void DecrementProductStockUnit(int id)
        {
            var currentFoodProd = this.db.FoodProducts.Find(id);

            currentFoodProd.Quantity--;
        }

        public async Task<bool> IsFoodProductInStockAsync(int id) => await this.db.FoodProducts.AnyAsync(fp => fp.Id == id && fp.Quantity > 0).ConfigureAwait(true);

        public decimal GetOriginalCostbyFoodProductId(int id) => this.db.FoodProducts.Find(id).Price;

        public async Task<FoodProduct> UpdateFoodProductAsync(int id, FoodProductDTO foodProductDto)
        {
            var currentFood = await this.db.FoodProducts.FindAsync(id).ConfigureAwait(true);

            this.db.Entry(currentFood).State = EntityState.Modified;

            this.db.FoodProducts.Remove(currentFood);

            var updatedfoodProduct = new FoodProduct()
            {
                //Id = foodProductDto.FoodId,
                Name = foodProductDto.Name,
                Price = foodProductDto.Price,
                Quantity = foodProductDto.Quantity,
                IsInCart = foodProductDto.InCart,
                CategoryId = foodProductDto.CategoryId
            };

            var newUpdate = await this.db.FoodProducts.AddAsync(updatedfoodProduct).ConfigureAwait(true);
            await this.db.SaveChangesAsync().ConfigureAwait(true);

            return newUpdate.Entity;
        }

        public bool FoodProductExists(int id) => this.db.FoodProducts.Any(e => e.Id == id);

        public async Task<bool> RemoveFoodProduct(FoodProduct foodProduct)
        {
            var entityEntry = this.db.Remove(foodProduct);
            if (entityEntry.State != EntityState.Deleted)
            {
                return false;
            }
            await this.db.SaveChangesAsync().ConfigureAwait(true);

            return true;
        }
    }
}
