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

    [Route("api/[controller]")]
    [ApiController]
    public class FoodProductsController : ControllerBase
    {
        private readonly FoodDBContext _context;

        public FoodProductsController(FoodDBContext context)
        {
            _context = context;
        }

        // GET: api/FoodProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodProduct>>> GetFoodProductsAsync()
        {
            return await _context.FoodProducts.ToListAsync().ConfigureAwait(true);
        }

        // GET: api/FoodProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodProduct>> GetFoodProduct(int id)
        {
            var foodProduct = await _context.FoodProducts.FindAsync(id);

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

        // POST: api/FoodProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FoodProduct>> PostFoodProduct(FoodProduct foodProduct)
        {
            _context.FoodProducts.Add(foodProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodProduct", new { id = foodProduct.Id }, foodProduct);
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
