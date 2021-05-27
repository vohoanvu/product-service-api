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

        //// PUT: api/CartItems/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCartItem(string id, CartItem cartItem)
        //{
        //    if (id != cartItem.ItemId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(cartItem).State = EntityState.Modified;

        //    await _context.SaveChangesAsync().ConfigureAwait(true);

        //    return NoContent();
        //}

        // POST: api/ShoppingCart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/add")]
        public async Task<ActionResult> AddToCartItemAsync(int productItem)
        {
            //_context.ShoppingCartItems.Add(cartItem);
            var newCartItem = await this._usersShoppingCart.AddToCartAsync(productItem).ConfigureAwait(true);

            //return this.CreatedAtAction("GetCartItem", new { itemID = newCartItem.ItemId }, newCartItem);
            //return this.CreatedAtRoute("GetCartItem", new { itemID = newCartItem.ItemId }, newCartItem);
            return this.CreatedAtRoute(new { itemID = newCartItem.ItemId }, newCartItem);
        }


        // DELETE: api/CartItems/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCartItem(string id)
        //{
        //    var cartItem = await _context.ShoppingCartItems.FindAsync(id);
        //    if (cartItem == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.ShoppingCartItems.Remove(cartItem);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CartItemExists(int productID) => this._usersShoppingCart.IsThisProductExistInCart(productID);//return _context.ShoppingCartItem.Any(e => e.ProductId == productID);
    }
}
