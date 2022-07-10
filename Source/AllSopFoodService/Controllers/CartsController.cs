namespace AllSopFoodService.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services.Interfaces;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : Controller
    {
        private readonly ICartsService cartService;
        private readonly IProductsService productService;

        public CartsController(ICartsService cartService, IProductsService productService)
        {
            this.cartService = cartService;
            this.productService = productService;
        }


        // Get all the shopping carts created in the system
        [AllowAnonymous]
        [HttpGet("get-all-user-carts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllCartsAsync()
        {
            var response = await this.cartService.GetAllCartsAsync().ConfigureAwait(true);
            if (!response.Success)
            {
                return this.NotFound(response);
            }

            return this.Ok(response);
        }

        //Create A New Cart request for the currently authenticated User, not gonna be used as much
        [HttpPost("add-new-cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateNewCart()
        {
            var res = this.cartService.CreateShoppingCart();
            if (!res.Success)
            {
                this.BadRequest(res);
            }
            return this.Ok(res);
        }

        //Get Cart with Products for the currently logged-in user
        [HttpGet("get-user-cart-with-products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCartWithProducts()
        {
            var response = this.cartService.GetCartWithProducts();
            if (!response.Success)
            {
                this.NotFound(response);
            }
            return this.Ok(response);
        }

        // Helper service: Get Cart By Cart ID
        [HttpGet("get-cart-by-id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCartById(int id)
        {
            var res = this.cartService.GetCartById(id);
            if (!res.Success)
            {
                return this.NotFound(res);
            }

            return this.Ok(res);
        }

        // Add a product to the cart owned by the currently logged-in User
        [HttpPost("add-to-cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddToCart(int productId)
        {
            // Still need to perform validation check for productId
            // Check if the product is available in Stock (FoodProduct DB) first thing first!
            var stockCheck = this.productService.IsFoodProductInStock(productId);
            if (!stockCheck.Success)
            {
                // The product does not exist
                return this.BadRequest(stockCheck);
            }
            if (!stockCheck.Success)
            {
                // The product exists, but out of stock
                return this.NotFound(stockCheck);
            }

            var response = this.cartService.AddToCart(productId);
            if (!response.Success)
            {
                return this.BadRequest(response);
            }
            //this._foodCatalogService.DecrementProductStockUnit(productId);

            return this.Ok(response);
        }

        // Delete a Product from the Cart owned by the currenly logged-in User
        [HttpDelete("remove-from-cart")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult RemoveFromCart(int productId)
        {
            var stockCheck = this.productService.IsFoodProductInStock(productId);
            if (!stockCheck.Success)
            {
                // The product does not exist
                return this.BadRequest(stockCheck);
            }
            if (!stockCheck.Success)
            {
                // The product exists, but out of stock
                return this.NotFound(stockCheck);
            }

            var response = this.cartService.RemoveFromCart(productId);
            if (!stockCheck.Success)
            {
                return this.NotFound(response);
            }

            return this.Ok(response);
        }


        // This project assume only 1 coupon can be applied to the cart at a time
        // Apply a voucher code to the Cart owned by currently authenticated user
        [HttpPut("applyVoucher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult ApplyVoucherToCart(string voucherCode)
        {
            //perform validation check for Cart here 

            var response = this.cartService.ApplyVoucherToCart(voucherCode);
            if (!response.Success)
            {
                return this.Conflict(response);
            }

            return this.Ok(response);
        }


        // Get the Total Price for the Shopping Cart owned by the currently authenticated User
        [HttpGet("total")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTotalPrice()
        {
            var response = this.cartService.GetTotal();
            if (response.Success == false)
            {
                this.NotFound(response);
            }

            return this.Ok(response);
        }

    }
}
