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
    using AllSopFoodService.Services;
    using System.Net.Http;
    using AllSopFoodService.Controllers;

    [Route("api/ShoppingCart")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        //private readonly FoodDBContext _context;
        //private readonly IHttpContextAccessor httpcontextaccessor;
        private readonly IShoppingCartActions _usersShoppingCart;
        private readonly IFoodProductsService _foodCatalogService;

        public CartItemsController(IShoppingCartActions usersShoppingCart, IFoodProductsService foodCatalogService)
        {
            this._usersShoppingCart = usersShoppingCart;
            this._foodCatalogService = foodCatalogService;
        }


        // GET: api/CartItems
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CartItem>>> GetShoppingCartItems()
        //{
        //    return await _context.ShoppingCartItems.ToListAsync();
        //}

        //GET: api/ShoppingCart/applyVoucher
        // This project assume only 1 coupon can be applied to the cart at a time
        [HttpPut("applyVoucher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ApplyVoucherAsync(string voucherCode)
        {
            var voucherCheck = this._usersShoppingCart.CheckCodeExists(voucherCode);
            if (!voucherCheck)
            {
                return this.NotFound($"This Voucher Code is Invalid! Please try another one!");
            }

            //var response = new HttpResponseMessage();

            var voucherResponse = await this._usersShoppingCart.ApplyVoucherToCartAsync(voucherCode).ConfigureAwait(true);

            if (voucherResponse.Applied)
            {
                //response.Headers.Add("Message", "Successfully applied Voucher to Cart!");
                //response.StatusCode = System.Net.HttpStatusCode.OK;
                return new ObjectResult(voucherResponse)
                {
                    StatusCode = StatusCodes.Status200OK,
                    Value = voucherResponse
                };
            }

            //return this.StatusCode(StatusCodes.Status500InternalServerError, "Error applying this coupon");
            //response.Headers.Add("Message", voucherResponse.FailedMessage);
            //response.StatusCode = System.Net.HttpStatusCode.Accepted;
            return new ObjectResult(voucherResponse)
            {
                StatusCode = StatusCodes.Status202Accepted,
                Value = voucherResponse
            };

        }

        //GET: api/ShoppingCart/sum
        //to protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("sum")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public decimal GetTotalPrice()
        {
            var currentCartItems = this._usersShoppingCart.GetCartItems();
            return this._usersShoppingCart.GetTotal(currentCartItems);
        }

        // POST: api/ShoppingCart/add/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> AddToCartItemAsync(int productItem)
        {
            // Check if the product is available in Stock (FoodProduct DB) first thing first!
            var isInStock = await this._foodCatalogService.IsFoodProductInStockAsync(productItem).ConfigureAwait(true);

            if (!isInStock)
            {
                // cover the last user story
                return this.NotFound($"This product is currently Out of Stock!");
            }

            var newCartItem = await this._usersShoppingCart.AddToCartAsync(productItem).ConfigureAwait(true);
            this._foodCatalogService.DecrementProductStockUnit(productItem);

            //return this.CreatedAtAction("GetCartItem", new { itemID = newCartItem.ItemId }, newCartItem);
            //return this.CreatedAtRoute("GetCartItem", new { itemID = newCartItem.ItemId }, newCartItem);
            return this.CreatedAtRoute(new { itemID = newCartItem.ItemId }, newCartItem);
        }

        //DELETE: api/ShoppingCart/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCartItemAsync(int id)
        {
            try
            {
                var cartItem = await this._usersShoppingCart.RemoveFromCartAsync(id).ConfigureAwait(true);
                if (cartItem == null)
                {
                    return this.NotFound($"The Cart does not have any product with Id= {id} !");
                }

                return this.Ok(cartItem);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }

        }

    }
}
