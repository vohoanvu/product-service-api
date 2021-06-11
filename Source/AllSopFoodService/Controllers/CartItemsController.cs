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

        public CartItemsController(IShoppingCartActions usersShoppingCart) => this._usersShoppingCart = usersShoppingCart;

        // GET: api/CartItems
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<CartItem>>> GetShoppingCartItems()
        //{
        //    return await _context.ShoppingCartItems.ToListAsync();
        //}

        //GET: api/ShoppingCart/applyCode/11
        [HttpPut("applyCode/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<HttpResponseMessage>> ApplyVoucherAsync(string voucherCode)
        {
            var voucherCheck = this._usersShoppingCart.CheckCodeExists(voucherCode);
            if (!voucherCheck)
            {
                return this.NotFound($"This Voucher Code is Invalid! Please try another one!");
            }


            var response = await this._usersShoppingCart.ApplyVoucherToCartAsync(voucherCode).ConfigureAwait(true);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                response.Headers.Add("Message", "Successfully applied Voucher to Cart!");
                //response.StatusCode = System.Net.HttpStatusCode.OK;
                return response;
            }
            else
            {
                //response.Headers.Add("Message", "This Voucher Code is already claimed! Please use another code!");
                //response.StatusCode = (System.Net.HttpStatusCode)StatusCodes.Status204NoContent;
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error applying this coupon");
            }

        }

        //GET: api/ShoppingCart/sum
        //to protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("sum")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public decimal GetTotalPrice() => this._usersShoppingCart.GetTotal();

        // POST: api/ShoppingCart/add/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> AddToCartItemAsync(int productItem)
        {
            //_context.ShoppingCartItems.Add(cartItem);
            var newCartItem = await this._usersShoppingCart.AddToCartAsync(productItem).ConfigureAwait(true);

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
