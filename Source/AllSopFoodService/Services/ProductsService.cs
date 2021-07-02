#nullable disable
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
    using Boxed.Mapping;

    public class ProductsService : IProductsService
    {
        private readonly FoodDBContext db;

        private readonly ICategoryService categoryService;
        private readonly IMapper<Product, FoodProductVM> productMapper;

        public ProductsService(FoodDBContext dbcontext, ICategoryService categoryService, IMapper<Product, FoodProductVM> mapper)
        {
            this.db = dbcontext;
            this.productMapper = mapper;
            this.categoryService = categoryService;
        }

        //Perhaps this should be placed at the repo layer?
        public async Task<ServiceResponse<List<FoodProductVM>>> GetAllProducts(string sortBy, string searchString, int pageNum, int pageSize)
        {
            var serviceResponse = new ServiceResponse<List<FoodProductVM>>();
            // Eager loading
            var dbProducts = await this.db.Products.Include(p => p.Category).ToListAsync().ConfigureAwait(true);
            // Explicit Loading, only load the n amount of distinct 'Category', still less efficient than Eager Loading
            //foreach (var prod in dbProducts)
            //{
            //    this.db.Entry(prod).Reference(u => u.Category).Load();
            //}

            serviceResponse.Data = dbProducts.Select(fooditem => this.productMapper.Map(fooditem)).ToList();
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
            var dbProduct = await this.db.Products.Include(p => p.Category).FirstOrDefaultAsync(fp => fp.Id == id).ConfigureAwait(true);
            if (dbProduct != null)
            {
                serviceResponse.Data = this.productMapper.Map(dbProduct);
            }
            else
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Cannot find a product with the given ID";
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<FoodProductVM>>> CreateFoodProduct(ProductSaves foodProductDto)
        {
            var serviceResponse = new ServiceResponse<List<FoodProductVM>>();
            //possibly use AutoMapper here
            var foodProduct = new Product()
            {
                Name = foodProductDto.Name,
                Price = foodProductDto.Price,
                Quantity = foodProductDto.Quantity,
                CategoryId = foodProductDto.CategoryId,
                FoodProduct_Carts = null
            };

            this.db.Products.Add(foodProduct);
            await this.db.SaveChangesAsync().ConfigureAwait(true);

            //Mapping all products to FoodProductVM
            serviceResponse.Data = await this.db.Products.Include(p => p.Category)
                                        .Select(foodItem => this.productMapper.Map(foodItem))
                                        .ToListAsync().ConfigureAwait(true);

            return serviceResponse;
        }

        public void DecrementProductStockUnit(int id)
        {
            var currentFoodProd = this.db.Products.First(p => p.Id == id);

            currentFoodProd.Quantity--;
            //could use Update() if the changes take places far away from the context, Or use Entry().State = Modified
            //this.db.Products.Update(currentFoodProd);
            this.db.SaveChanges();
        }

        public ServiceResponse<Product> IsFoodProductInStock(int id)
        {
            var serviceResponse = new ServiceResponse<Product>();
            //Check if product is real first
            if (!this.db.Products.Any(p => p.Id == id))
            {
                serviceResponse.Success = this.db.Products.Any(p => p.Id == id);
                serviceResponse.Message = "Cannot find a product with this ID! Please check again!";
                return serviceResponse;
            }

            var thisProduct = this.db.Products.FirstOrDefault(p => p.Id == id);

            if (thisProduct.Quantity > 0)
            {
                serviceResponse.Data = thisProduct;
                serviceResponse.Success = true;
                serviceResponse.Message = "This product is stil in stock!";
            }
            else
            {
                serviceResponse.Data = thisProduct;
                serviceResponse.Success = false;
                serviceResponse.Message = "This product is Out Of Stock!";
            }

            return serviceResponse;
        }


        public decimal GetOriginalCostbyFoodProductId(int id) => this.db.Products.Find(id).Price;

        public async Task<ServiceResponse<FoodProductVM>> UpdateFoodProduct(int id, ProductSaves foodProductDto)
        {
            //At the moment, the update request is not returning product Id upon successful update
            var serviceResponse = new ServiceResponse<FoodProductVM>();
            try
            {
                var currentFood = await this.db.Products.Include(p => p.Category).FirstOrDefaultAsync(foodItem => foodItem.Id == id).ConfigureAwait(true);
                // mapping from ProductSaves to Product
                currentFood.Name = foodProductDto.Name;
                currentFood.Price = foodProductDto.Price;
                currentFood.Quantity = foodProductDto.Quantity;
                currentFood.CategoryId = foodProductDto.CategoryId;

                this.db.Products.Update(currentFood);
                await this.db.SaveChangesAsync().ConfigureAwait(true);

                // mapping from Product to ProductVM
                serviceResponse.Data = this.productMapper.Map(currentFood);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<FoodProductVM>>> RemoveFoodProductById(int id)
        {
            var serviceResponse = new ServiceResponse<List<FoodProductVM>>();
            try
            {
                var foodProduct = this.db.Products.First(food => food.Id == id);
                this.db.Products.Remove(foodProduct);
                this.db.SaveChanges();

                //manuall mapping
                serviceResponse.Data = await this.db.Products.Include(p => p.Category).Select(foodItem => this.productMapper.Map(foodItem)).ToListAsync().ConfigureAwait(true);
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
