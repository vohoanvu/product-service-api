namespace AllSopFoodService.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Model;
    using Microsoft.AspNetCore.Authorization;
    using Repositories.Interfaces;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PromotionsController(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        // GET: api/Promotions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPromotionsAsync()
        {
            try
            {
                return this.Ok(await this.unitOfWork.Promotions.GetAllAsync().ConfigureAwait(true));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCouponCodeById(int id)
        {
            var promotion = this.unitOfWork.Promotions.GetById(id);

            if (promotion == null)
            {
                return this.NotFound($"This Voucher Code is Invalid! Please try again!");
            }

            return this.Ok(promotion);
        }

        // PUT: api/Promotions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutPromotion(int id, Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return this.BadRequest();
            }
            try
            {
                this.unitOfWork.Promotions.Update(promotion);
                this.unitOfWork.Complete();
            }
            catch (Exception)
            {
                throw;
            }

            return this.Ok($"The promotion has successfully been updated");
        }

        // POST: api/Promotions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<Promotion> PostPromotion(Promotion promotion)
        {
            try
            {
                this.unitOfWork.Promotions.Add(promotion);
                this.unitOfWork.Complete();
            }
            catch (Exception)
            {
                throw;
            }

            return this.CreatedAtAction("GetPromotions", new { id = promotion.Id }, promotion);
        }

        // DELETE: api/Promotions/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeletePromotion(int id)
        {
            var promotion = this.unitOfWork.Promotions.GetById(id);
            if (promotion == null)
            {
                return this.NotFound();
            }

            this.unitOfWork.Promotions.Delete(promotion);
            this.unitOfWork.Complete();

            return this.Ok();
        }
    }
}
