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
    public class ShoppingCartController : Controller
    {
        private readonly ICartsService _cartService;

        public ShoppingCartController(ICartsService cartService) => this._cartService = cartService;

        [HttpPost("add-new-cart")]
        public IActionResult AddBook([FromBody] CartVM author)
        {
            this._cartService.CreateShoppingCart(author);
            return this.Ok();
        }

        [HttpGet("get-cart-with-products-by-id/{id}")]
        public IActionResult GetCartWithProducts(int id)
        {
            var response = this._cartService.GetCartWithProducts(id);
            return this.Ok(response);
        }
    }
}
