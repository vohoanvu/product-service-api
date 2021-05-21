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

    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        //private readonly FoodDBContext _context;
        //private readonly IHttpContextAccessor httpcontextaccessor;
        private readonly ShoppingCartActions _usersShoppingCart;

        public CartItemsController(ShoppingCartActions usersShoppingCart)
        {
            this._usersShoppingCart = usersShoppingCart;
        }

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

        // POST: api/CartItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartItem>> AddToCartItem(int productItem)
        {
            //_context.ShoppingCartItems.Add(cartItem);
            await _usersShoppingCart.AddToCartAsync(productItem);
            if (CartItemExists(productItem))
            {
                return Conflict();
            }

            return CreatedAtAction("GetCartItem", new { id = productItem }, productItem);
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

        private bool CartItemExists(int productID)
        {
            return _usersShoppingCart.IsThisProductExistInCart(productID);
            //return _context.ShoppingCartItem.Any(e => e.ProductId == productID);
        }
    }
}
