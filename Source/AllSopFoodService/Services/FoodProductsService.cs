namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Model.Paging;
    using AllSopFoodService.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class FoodProductsService : IFoodProductsService
    {
        private readonly FoodDBContext db;

        public FoodProductsService(FoodDBContext dbcontext) => this.db = dbcontext;

        //Perhaps this should be placed at the repo layer?
        public async Task<List<FoodProductVM>> GetFoodProductsAsync(string? sortBy, string? searchString, int? pageNum, int? pageSize)
        {
            // Query Data via EF core DbSet
            // transform results to dto object (non-entity type)
            var foodItems = await this.db.FoodProducts.AsNoTracking().Select(fooditem => new FoodProductVM()
            {
                Name = fooditem.Name,
                Price = fooditem.Price,
                Quantity = fooditem.Quantity,
                InCart = fooditem.IsInCart,
                CategoryName = fooditem.Category.Label,
                ShoppingCartNames = fooditem.FoodProduct_Carts.Select(n => n.ShoppingCart != null ? n.ShoppingCart.CartLabel : "empty").ToList()
            }).ToListAsync().ConfigureAwait(true);

            // Server side sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        foodItems = foodItems.OrderByDescending(n => n.Name).ToList();
                        break;
                    default:
                        foodItems = foodItems.OrderBy(n => n.Name).ToList();
                        break;
                }
            }
            // server side Searching
            if (!string.IsNullOrEmpty(searchString))
            {
                foodItems = foodItems.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            // server side Paging
            // default pageNum = 1, pageSize = 5 if null
            foodItems = PaginatedList<FoodProductVM>.Create(foodItems.AsQueryable(), pageNum ?? 1, pageSize ?? 5);

            return foodItems;
        }

        public FoodProductVM GetFoodProductById(int id)
        {
            //perhaps we can use FirstorDefaultAsync
            var foodwithCategory = this.db.FoodProducts.Where(fp => fp.Id == id).Select(fp => new FoodProductVM()
            {
                Name = fp.Name,
                Price = fp.Price,
                Quantity = fp.Quantity,
                InCart = fp.IsInCart,
                CategoryName = fp.Category.Label,
                ShoppingCartNames = fp.FoodProduct_Carts.Select(n => n.ShoppingCart != null ? n.ShoppingCart.CartLabel : "empty").ToList()
            }).FirstOrDefault();

#pragma warning disable CS8603 // Possible null reference return.
            return foodwithCategory;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void CreateFoodProduct(FoodProductDTO foodProductDto)
        {
            var foodProduct = new FoodProduct()
            {
                Name = foodProductDto.Name,
                Price = foodProductDto.Price,
                Quantity = foodProductDto.Quantity,
                IsInCart = foodProductDto.InCart,
                CategoryId = foodProductDto.CategoryId,
            };

            this.db.FoodProducts.Add(foodProduct);
            this.db.SaveChanges();

            if (foodProductDto.ShoppingCartIds != null)
            {
                foreach (var id in foodProductDto.ShoppingCartIds)
                {
                    var food_Cart = new FoodProduct_ShoppingCart()
                    {
                        FoodProductId = foodProduct.Id,
                        ShoppingCartId = id
                    };
                    this.db.FoodProducts_Carts.Add(food_Cart);
                    this.db.SaveChanges();
                }
            }
            //return createdataction("getfoodproduct", new { id = foodproduct.id }, foodproduct);
        }

        public void DecrementProductStockUnit(int id)
        {
            var currentFoodProd = this.db.FoodProducts.Find(id);

            currentFoodProd.Quantity--;
        }

        public async Task<bool> IsFoodProductInStockAsync(int id) => await this.db.FoodProducts.AnyAsync(fp => fp.Id == id && fp.Quantity > 0).ConfigureAwait(true);

        public decimal GetOriginalCostbyFoodProductId(int id) => this.db.FoodProducts.Find(id).Price;

        public FoodProduct UpdateFoodProduct(int id, FoodProductDTO foodProductDto)
        {
            var currentFood = this.db.FoodProducts.FirstOrDefault(foodItem => foodItem.Id == id);

            if (currentFood != null)
            {
                currentFood.Name = foodProductDto.Name;
                currentFood.Price = foodProductDto.Price;
                currentFood.Quantity = foodProductDto.Quantity;
                currentFood.IsInCart = foodProductDto.InCart;
                currentFood.CategoryId = foodProductDto.CategoryId;

                this.db.SaveChanges();
            }

#pragma warning disable CS8603 // Possible null reference return.
            return currentFood;
#pragma warning restore CS8603 // Possible null reference return.
        }

        public bool FoodProductExists(int id) => this.db.FoodProducts.Any(e => e.Id == id);

        public bool RemoveFoodProductById(int id)
        {
            var entity = this.db.FoodProducts.FirstOrDefault(food => food.Id == id);
            if (entity != null)
            {
                this.db.FoodProducts.Remove(entity);
                this.db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
