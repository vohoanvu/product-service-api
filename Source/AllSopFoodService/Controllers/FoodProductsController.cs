#nullable disable
namespace AllSopFoodService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using AllSopFoodService.Model;
    using AllSopFoodService.Mappers;
    using Microsoft.AspNetCore.JsonPatch;
    using AllSopFoodService.Services;
    using AllSopFoodService.ViewModels;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    [ApiController]
    public class FoodProductsController : ControllerBase
    {
        private readonly IProductsService _foodItemService;
        private readonly ILogger<FoodProductsController> _logger;

        public FoodProductsController(IProductsService foodProductsService, ILogger<FoodProductsController> logger)
        {
            this._foodItemService = foodProductsService;
            this._logger = logger;
        }

        // GET: api/FoodProducts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // or ---public async Task<ActionResult<IEnumerable<FoodProductDTO>>> GetFoodProductsAsync()--- is also correct!
        public async Task<IActionResult> GetFoodProducts(string sortBy, string searchString, int pageNum, int pageSize)
        {
            this._logger.LogInformation("This is a log test in GetAllFoodProducts Controller");
            var response = await this._foodItemService.GetAllProducts(sortBy, searchString, pageNum, pageSize).ConfigureAwait(true);
            if (response.Data == null)
            {
                return this.NotFound(response);
            }
            response.Message = $"There are a total of {response.Data.Count} product records";
            return this.Ok(response); // if returned type was ActionResult<T>, then only need to 'return foodItems;'
        }

        // GET: api/FoodProducts/5
        [HttpGet("get-single/{id}")]
        public async Task<IActionResult> GetFoodProductById(int id)
        {
            var response = await this._foodItemService.GetFoodProductById(id).ConfigureAwait(true);
            if (!response.Success)
            {
                return this.NotFound(response);
            }
            return this.Ok(response);
        }

        // PUT: api/FoodProducts/5
        // This Update request might need refactoring
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateFoodProduct(int id, ProductSaves foodProduct)
        {
            var response = await this._foodItemService.UpdateFoodProduct(id, foodProduct).ConfigureAwait(true);
            if (response.Data == null)
            {
                return this.NotFound(response);
            }
            response.Success = true;
            response.Message = "This product has been updated successfully";
            return this.Ok(response);
        }

        //POST: api/FoodProducts
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add-product")]
        public async Task<IActionResult> AddFoodProduct([FromBody] ProductSaves foodProduct)
        {
            if (foodProduct == null)
            {
                return this.BadRequest(); // might be unecessary
            }

            var response = await this._foodItemService.CreateFoodProduct(foodProduct).ConfigureAwait(true);
            response.Message = $"There are a total of {response.Data.Count} product records";

            return this.Ok(response);
        }

        // DELETE: api/FoodProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodProduct(int id)
        {
            var response = await this._foodItemService.RemoveFoodProductById(id).ConfigureAwait(true);
            if (response.Data == null)
            {
                return this.NotFound(response);
            }
            response.Message = $"There are a total of {response.Data.Count} product records";

            return this.Ok(response);

        }
    }
}
