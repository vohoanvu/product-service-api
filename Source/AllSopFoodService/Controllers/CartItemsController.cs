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

        //// GET: api/CartItems/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<CartItem>> GetCartItem(string id)
        //{
        //    var cartItem = await _context.ShoppingCartItems.FindAsync(id);

        //    if (cartItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return cartItem;
        //}

        //GET: api/ShoppingCart/sum
        //to protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("sum")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public decimal GetTotalPrice() => this._usersShoppingCart.GetTotal();

        // POST: api/ShoppingCart/add
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
        public async Task<ActionResult<CartItem>> DeleteCartItemAsync(int id)
        {
            try
            {
                var cartItem = await this._usersShoppingCart.RemoveFromCartAsync(id).ConfigureAwait(true);
                if (cartItem == null)
                {
                    return this.NotFound($"The Cart does not have any product with Id= {id} !");
                }

                return cartItem;
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }

        }

        //private bool CartItemExists(int productID) => this._usersShoppingCart.IsThisProductExistInCart(productID);//return _context.ShoppingCartItem.Any(e => e.ProductId == productID);
    }
}
