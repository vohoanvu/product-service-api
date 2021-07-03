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
    using Microsoft.AspNetCore.Authorization;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly FoodDBContext _context;

        public PromotionsController(FoodDBContext context) => this._context = context;

        // GET: api/Promotions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromotionsAsync() => await this._context.CouponCodes.ToListAsync().ConfigureAwait(true);

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Promotion>> GetCouponCodeByIdAsync(int id)
        {
            var promotion = await this._context.CouponCodes.FindAsync(id).ConfigureAwait(true);

            if (promotion == null)
            {
                return this.NotFound($"This Voucher Code is Invalid! Please try again!");
            }

            return promotion;
        }

        // PUT: api/Promotions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutPromotionAsync(int id, Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return this.BadRequest();
            }

            this._context.Entry(promotion).State = EntityState.Modified;

            try
            {
                await this._context.SaveChangesAsync().ConfigureAwait(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.PromotionExists(id))
                {
                    return this.NotFound();
                }
                else
                {
                    throw;
                }
            }

            return this.NoContent();
        }

        // POST: api/Promotions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Promotion>> PostPromotionAsync(Promotion promotion)
        {
            this._context.CouponCodes.Add(promotion);
            await this._context.SaveChangesAsync().ConfigureAwait(true);

            return this.CreatedAtAction("GetPromotion", new { id = promotion.Id }, promotion);
        }

        // DELETE: api/Promotions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePromotionAsync(int id)
        {
            var promotion = await this._context.CouponCodes.FindAsync(id).ConfigureAwait(true);
            if (promotion == null)
            {
                return this.NotFound();
            }

            this._context.CouponCodes.Remove(promotion);
            await this._context.SaveChangesAsync().ConfigureAwait(true);

            return this.NoContent();
        }

        private bool PromotionExists(int id) => this._context.CouponCodes.Any(e => e.Id == id);
    }
}
