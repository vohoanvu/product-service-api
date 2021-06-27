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
    using AllSopFoodService.Services.Interfaces;

    [Route("api/[controller]")]
    [ApiController]
    public class UserCartController : ControllerBase
    {
        private readonly ICartsService _cartService;
        private readonly IProductsInCartsService _productInCartService;
        private readonly IFoodProductsService _foodCatalogService;

        public UserCartController(ICartsService cartService, IFoodProductsService foodCatalogService, IProductsInCartsService productInCartService)
        {
            this._cartService = cartService;
            this._foodCatalogService = foodCatalogService;
            this._productInCartService = productInCartService;
        }


        //GET: api/CartItems
        [HttpGet("get-all-carts-with-products")]
        public async Task<IActionResult> GetAllCartsWithProducts()
        {
            var serviceResponse = await this._productInCartService.GetAllProductsInCarts().ConfigureAwait(true);
            if (serviceResponse.Data == null)
            {
                return this.NotFound(serviceResponse);
            }

            return this.Ok(serviceResponse);
        }

        // POST: api/CartItems/
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add-to-cart")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddToCartItemAsync(int productId, int cartId)
        {
            // Check if the product is available in Stock (FoodProduct DB) first thing first!
            var isInStock = this._foodCatalogService.IsFoodProductInStock(productId);
            // Still need to perform validation check for both product Id and cart Id


            if (!isInStock.Success)
            {
                // cover the last user story
                return this.NotFound($"This product is currently Out of Stock!");
            }

            var response = await this._productInCartService.AddToCartAsync(productId, cartId).ConfigureAwait(true);
            if (response.Data == null)
            {
                return this.NotFound(response);
            }

            //this._foodCatalogService.DecrementProductStockUnit(productId);

            return this.Ok(response);
        }

        //DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCartItemAsync(int productId, int cartId)
        {
            // Still need to perform validation check for both product Id and cart Id

            var response = await this._productInCartService.RemoveFromCart(productId, cartId).ConfigureAwait(true);
            if (response.Data == null)
            {
                return this.NotFound(response);
            }

            return this.Ok(response);

        }

        //GET: api/CartItems/applyVoucher
        // This project assume only 1 coupon can be applied to the cart at a time
        [HttpPut("applyVoucher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ApplyVoucherToCart(string voucherCode, int cartId)
        {
            //perform validation check for Cart here 

            var response = await this._productInCartService.ApplyVoucherToCart(voucherCode, cartId).ConfigureAwait(true);
            if (response.Data == null)
            {
                return this.NotFound(response);
            }

            return this.Ok(response);
        }

        //GET: api/ShoppingCart/sum
        //to protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetTotalPrice(int cartId)
        {
            var response = this._productInCartService.GetTotal(cartId);
            if (response.Success == false)
            {
                this.NotFound(response);
            }

            return this.Ok(response);
        }

    }
}
