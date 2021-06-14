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
        private readonly FoodDBContext _context;
        private readonly IFoodProductsService _foodItemService;
        private readonly IShoppingCartActions _CartItemService;

        public FoodProductsController(IFoodProductsService foodProductsService, IShoppingCartActions cartItemService, FoodDBContext context)
        {
            this._context = context;
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
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodProduct>> GetFoodProduct(int id)
        {
            var foodProduct = await this._foodItemService.GetFoodProductByIdAsync(id);

            if (foodProduct == null)
            {
                return NotFound();
            }

            return foodProduct;
        }

        // PUT: api/FoodProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodProductAsync(int id, FoodProduct foodProduct)
        {
            if (id != foodProduct.Id)
            {
                return BadRequest();
            }

            _context.Entry(foodProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/FoodProducts
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodProductDTO>> PostFoodProductAsync(FoodProductDTO foodProductDto)
        {
            if (foodProductDto == null)
            {
                return this.BadRequest();
            }

            var createdFoodProduct = await this._foodItemService.CreateFoodProductAsync(foodProductDto).ConfigureAwait(true);

            return this.CreatedAtAction("GetFoodProduct", new { id = createdFoodProduct.Id }, createdFoodProduct);
        }

        // DELETE: api/FoodProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFoodProduct(int id)
        {
            var foodProduct = await _context.FoodProducts.FindAsync(id);
            if (foodProduct == null)
            {
                return NotFound();
            }

            _context.FoodProducts.Remove(foodProduct);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FoodProductExists(int id)
        {
            return _context.FoodProducts.Any(e => e.Id == id);
        }
    }
}
