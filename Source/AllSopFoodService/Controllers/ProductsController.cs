namespace AllSopFoodService.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels;
    using Microsoft.Extensions.Logging;
    using Services.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService foodItemService;

        public ProductsController(IProductsService foodProductsService) => this.foodItemService = foodProductsService;

        // GET: api/FoodProducts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [LoggerMessage(0, LogLevel.Information, "This is a log test in GetAllFoodProducts Controller")]
        // or ---public async Task<ActionResult<IEnumerable<FoodProductDTO>>> GetFoodProductsAsync()--- is also correct!
        public async Task<IActionResult> GetFoodProductsAsync(string sortBy, string? searchString, int pageNum, int pageSize)
        {
            //this.logger.LogInformation("This is a log test in GetAllFoodProducts Controller");
            var response = await this.foodItemService.GetAllProductsAsync(sortBy, pageNum, pageSize, searchString).ConfigureAwait(true);
            response.Message = $"There are a total of {response.Data.Count} product records";
            return this.Ok(response); // if returned type was ActionResult<T>, then only need to 'return foodItems;'
        }

        // GET: api/FoodProducts/5
        [HttpGet("get-single/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFoodProductByIdAsync(int id)
        {
            var response = await this.foodItemService.GetFoodProductByIdAsync(id).ConfigureAwait(true);
            if (!response.Success)
            {
                return this.NotFound(response);
            }
            return this.Ok(response);
        }

        // PUT: api/FoodProducts/5
        // This Update request might need refactoring
        [HttpPut("update-product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFoodProductAsync(int id, ProductSaves foodProduct)
        {
            var serviceResponse = this.foodItemService.IsFoodProductInStock(id);
            if (!serviceResponse.Success)
            {
                return this.BadRequest(serviceResponse);
            }
            var response = await this.foodItemService.UpdateFoodProductAsync(id, foodProduct).ConfigureAwait(true);
            response.Success = true;
            response.Message = "This product has been updated successfully";
            return this.Ok(response);
        }

        //POST: api/FoodProducts
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add-product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFoodProductAsync([FromBody] ProductSaves foodProduct)
        {
            if (foodProduct == null)
            {
                return this.BadRequest(); // might be unecessary
            }

            var response = await this.foodItemService.CreateFoodProductAsync(foodProduct).ConfigureAwait(true);
            response.Message = $"There are a total of {response.Data.Count} product records";

            return this.Ok(response);
        }

        // DELETE: api/FoodProducts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFoodProductAsync(int id)
        {
            var response = await this.foodItemService.RemoveFoodProductByIdAsync(id).ConfigureAwait(true);
            response.Message = $"There are a total of {response.Data.Count} product records";

            return this.Ok(response);
        }
    }
}
