namespace AllSopFoodService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Model.Paging;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.Mappers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    public class FoodProductsService : IFoodProductsService
    {
        private readonly FoodDBContext db;

        private readonly ICategoryService categoryService;
        private readonly IMapper _mapper;

        public FoodProductsService(FoodDBContext dbcontext, ICategoryService categoryService, IMapper mapper)
        {
            this.db = dbcontext;
            this._mapper = mapper;
            this.categoryService = categoryService;
        }

        //Perhaps this should be placed at the repo layer?
        public async Task<ServiceResponse<List<FoodProductVM>>> GetAllFoodProducts(string? sortBy, string? searchString, int? pageNum, int? pageSize)
        {
            // Query Data via EF core DbSet
            var serviceResponse = new ServiceResponse<List<FoodProductVM>>();
            var dbProducts = await this.db.FoodProducts.ToListAsync().ConfigureAwait(true);
            // transform results to dto object (non-entity type), possibly use AutoMapper here
            serviceResponse.Data = dbProducts.Select(fooditem => new FoodProductVM()
            {
                Name = fooditem.Name,
                Price = fooditem.Price,
                Quantity = fooditem.Quantity,
                CategoryName = this.db.Categories.Find(fooditem.CategoryId).Label
            }).ToList();
            //serviceResponse.Data = dbProducts.Select(fp => this._mapper.Map<FoodProductVM>(fp)).ToList()

            // Server side sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        serviceResponse.Data = serviceResponse.Data.OrderByDescending(n => n.Name).ToList();
                        break;
                    default:
                        serviceResponse.Data = serviceResponse.Data.OrderBy(n => n.Name).ToList();
                        break;
                }
            }
            // server side Searching
            if (!string.IsNullOrEmpty(searchString))
            {
                serviceResponse.Data = serviceResponse.Data.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            // server side Paging
            // default pageNum = 1, pageSize = 5 if null
            //serviceResponse.Data = PaginatedList<FoodProductVM>.Create(serviceResponse.Data.AsQueryable(), pageNum ?? 1, pageSize ?? 5);

            return serviceResponse;
        }

        public async Task<ServiceResponse<FoodProductVM>> GetFoodProductById(int id)
        {
            var serviceResponse = new ServiceResponse<FoodProductVM>();
            var dbProduct = await this.db.FoodProducts.FirstOrDefaultAsync(fp => fp.Id == id).ConfigureAwait(true);
            if (dbProduct != null)
            {
                serviceResponse.Data = new FoodProductVM()
                {
                    Name = dbProduct.Name,
                    Price = dbProduct.Price,
                    Quantity = dbProduct.Quantity,
                    CategoryName = this.categoryService.GetCategoryData(dbProduct.CategoryId).Label
                };
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Cannot find a product with the given ID";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<FoodProductVM>>> CreateFoodProduct(FoodProductDTO foodProductDto)
        {
            var serviceResponse = new ServiceResponse<List<FoodProductVM>>();
            //possibly use AutoMapper here
            var foodProduct = new FoodProduct()
            {
                Name = foodProductDto.Name,
                Price = foodProductDto.Price,
                Quantity = foodProductDto.Quantity,
                CategoryId = foodProductDto.CategoryId,
                FoodProduct_Carts = null
            };

            this.db.FoodProducts.Add(foodProduct);
            await this.db.SaveChangesAsync().ConfigureAwait(true);

            ////check if the newly created product is also going to be in any Cart
            //if (foodProductDto.ShoppingCartIds != null)
            //{
            //    foreach (var id in foodProductDto.ShoppingCartIds)
            //    {
            //        var food_Cart = new FoodProduct_ShoppingCart()
            //        {
            //            FoodProductId = foodProduct.Id,
            //            ShoppingCartId = id
            //        };
            //        this.db.FoodProducts_Carts.Add(food_Cart);
            //        await this.db.SaveChangesAsync().ConfigureAwait(true);
            //    }
            //}
            //Mapping all products to FoodProductVM
            serviceResponse.Data = await this.db.FoodProducts.Select(fooditem => new FoodProductVM()
            {
                Name = fooditem.Name,
                Price = fooditem.Price,
                Quantity = fooditem.Quantity,
                CategoryName = this.categoryService.GetCategoryData(fooditem.CategoryId).Label
            }).ToListAsync().ConfigureAwait(true);

            return serviceResponse;
        }

        public void DecrementProductStockUnit(int id)
        {
            var currentFoodProd = this.db.FoodProducts.Find(id);

            currentFoodProd.Quantity--;
        }

        public ServiceResponse<FoodProduct> IsFoodProductInStock(int id)
        {
            var serviceResponse = new ServiceResponse<FoodProduct>();

            if (this.db.FoodProducts.Any(fp => fp.Id == id && fp.Quantity > 0))
            {
                serviceResponse.Data = this.db.FoodProducts.Find(id);
                serviceResponse.Success = true;
                serviceResponse.Message = "This product is stil in stock!";
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "This product is Out Of Stock!";
            }

            return serviceResponse;
        }


        public decimal GetOriginalCostbyFoodProductId(int id) => this.db.FoodProducts.Find(id).Price;

        public async Task<ServiceResponse<FoodProductVM>> UpdateFoodProduct(int id, FoodProductDTO foodProductDto)
        {
            //At the moment, the update request is not returning product Id upon successful update
            var serviceResponse = new ServiceResponse<FoodProductVM>();
            try
            {
                var currentFood = await this.db.FoodProducts.FirstOrDefaultAsync(foodItem => foodItem.Id == id).ConfigureAwait(true);

                currentFood.Name = foodProductDto.Name;
                currentFood.Price = foodProductDto.Price;
                currentFood.Quantity = foodProductDto.Quantity;
                currentFood.CategoryId = foodProductDto.CategoryId;

                await this.db.SaveChangesAsync().ConfigureAwait(true);

                //var mapper = new FoodProductToVMMapper();
                //mapper.Map(currentFood, serviceResponse.Data);
                //manuall mapping
                serviceResponse.Data = new FoodProductVM()
                {
                    Name = currentFood.Name,
                    Price = currentFood.Price,
                    Quantity = currentFood.Quantity,
                    CategoryName = this.categoryService.GetCategoryData(currentFood.CategoryId).Label
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public bool FoodProductExists(int id) => this.db.FoodProducts.Any(e => e.Id == id);

        public async Task<ServiceResponse<List<FoodProductVM>>> RemoveFoodProductById(int id)
        {
            var serviceResponse = new ServiceResponse<List<FoodProductVM>>();
            try
            {
                var foodProduct = await this.db.FoodProducts.FirstAsync(food => food.Id == id).ConfigureAwait(true);
                this.db.FoodProducts.Remove(foodProduct);
                await this.db.SaveChangesAsync().ConfigureAwait(true);

                //manuall mapping
                serviceResponse.Data = this.db.FoodProducts.Select(fooditem => new FoodProductVM()
                {
                    Name = fooditem.Name,
                    Price = fooditem.Price,
                    Quantity = fooditem.Quantity,
                    CategoryName = this.categoryService.GetCategoryData(fooditem.CategoryId).Label
                }).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
