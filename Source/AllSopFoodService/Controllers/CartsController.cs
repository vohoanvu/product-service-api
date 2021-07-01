namespace AllSopFoodService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Services;
    using AllSopFoodService.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : Controller
    {
        private readonly ICartsService _cartService;

        public CartsController(ICartsService cartService) => this._cartService = cartService;

        [HttpGet("get-all-user-carts")]
        public async Task<IActionResult> GetAllCarts()
        {
            var response = await this._cartService.GetAllCarts().ConfigureAwait(true);
            if (response.Data == null)
            {
                return this.NotFound(response);
            }

            return this.Ok(response);
        }

        [HttpPost("add-new-cart")]
        public IActionResult CreateNewCart([FromBody] CartSaves cart)
        {
            var res = this._cartService.CreateShoppingCart(cart);
            return this.Ok(res);
        }

        [HttpGet("get-cart-with-products-by-id/{id}")]
        public IActionResult GetCartWithProducts(int id)
        {
            var response = this._cartService.GetCartWithProducts(id);
            if (response.Data == null)
            {
                this.NotFound(response);
            }
            return this.Ok(response);
        }

        [HttpGet("get-cart-by-id")]
        public IActionResult GetCartById(int id)
        {
            var res = this._cartService.GetCartById(id);
            if (res.Data == null)
            {
                return this.NotFound(res);
            }

            return this.Ok(res);
        }

        //[HttpGet]
        //public IActionResult GetCartByUserId(int userId)
        //{
        //    var id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        //}
    }
}
