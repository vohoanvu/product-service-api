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

    [Route("api/[controller]")]
    [ApiController]
    public class FoodProductsController : ControllerBase
    {
        private readonly IFoodProductsService _foodItemService;
        private readonly IShoppingCartActions _CartItemService;

        public FoodProductsController(IFoodProductsService foodProductsService, IShoppingCartActions cartItemService)
        {
            this._foodItemService = foodProductsService;
            this._CartItemService = cartItemService;
        }

        // GET: api/FoodProducts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // or ---public async Task<ActionResult<IEnumerable<FoodProductDTO>>> GetFoodProductsAsync()--- is also correct!
        public async Task<IActionResult> GetFoodProductsAsync()
        {
            var foodItems = await this._foodItemService.GetFoodProductsAsync().ConfigureAwait(true);

            return this.Ok(foodItems); // if returned type was ActionResult<T>, then only need to 'return foodItems;' 
        }

        //PUT: api/FoodProducts/cart/5
        // adding a product to the shopping cart
        //[HttpPut("/cart/{id}")]
        //public async Task<ActionResult> AddToCartAsync(int id)
        //{
        //    var entity = await _context.FoodProducts.FindAsync(id).ConfigureAwait(true);

        //    if (entity == null)
        //    {
        //        return NotFound();
        //    }

        //    entity.IsInCart = true;
        //    await _context.SaveChangesAsync().ConfigureAwait(true);

        //    return Ok(entity);
        //}

        //DELETE: api/FoodProducts/cart/5
        //removing a product from the shopping cart
        //[HttpDelete("/cart/{id}")]
        //public async Task<ActionResult> RemoveFromAsync(int id)
        //{
        //    var entity = await _context.FoodProducts.FindAsync(id).ConfigureAwait(true);

        //    if (entity == null)
        //    {
        //        return NotFound();
        //    }

        //    entity.IsInCart = false;
        //    await _context.SaveChangesAsync().ConfigureAwait(true);

        //    return Ok(entity);
        //}

        // GET: api/FoodProducts/5
        [HttpGet("get-product-by-id/{id}")]
        public IActionResult GetFoodProductById(int id)
        {
            var foodProduct = this._foodItemService.GetFoodProductById(id);
            if (foodProduct == null)
            {
                return this.NotFound();
            }

            return this.Ok(foodProduct);
        }

        // PUT: api/FoodProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update-product/{id}")]
        public IActionResult UpdateFoodProduct(int id, [FromBody] FoodProductDTO foodProduct)
        {
            var updatedFoodProduct = this._foodItemService.UpdateFoodProduct(id, foodProduct);
            if (updatedFoodProduct == null)
            {
                return this.NoContent();
            }

            return new ObjectResult(updatedFoodProduct); // or we can use Ok()
        }

        //POST: api/FoodProducts
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add-product")]
        public IActionResult AddFoodProduct([FromBody] FoodProductDTO foodProductDto)
        {
            if (foodProductDto == null)
            {
                return this.BadRequest(); // might be unecessary
            }

            this._foodItemService.CreateFoodProduct(foodProductDto);

            return this.Ok();
            //return this.CreatedAtAction("GetFoodProduct", new { id = createdFoodProduct.Id }, createdFoodProduct);
        }

        // DELETE: api/FoodProducts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteFoodProduct(int id)
        {
            //var foodProduct = this._foodItemService.GetFoodProductByIdAsync(id);
            //if (foodProduct == null)
            //{
            //    return this.NotFound();
            //}

            //_context.FoodProducts.Remove(foodProduct);
            //await _context.SaveChangesAsync();
            var deletedSuccess = this._foodItemService.RemoveFoodProductById(id);
            if (!deletedSuccess)
            {
                return new ObjectResult("Failed to delete the Product!"); // or use Ok()
            }

            return this.NoContent();

        }
    }
}
