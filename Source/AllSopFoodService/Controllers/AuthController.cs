#nullable disable
namespace AllSopFoodService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.ViewModels.UserAuth;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;
        public AuthController(IAuthRepository authRepository)
        {
            this.authRepo = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            var userRegistration = new User()
            {
                UserName = request.Username,
                Cart = new ShoppingCart()
                {
                    CartLabel = $"This Shopping Cart is owned by {request.Username}"
                }
            };

            var response = await this.authRepo.Register(userRegistration, request.Password).ConfigureAwait(true);

            if (!response.Success)
            {
                return this.BadRequest(response);
            }

            return this.Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            var response = await this.authRepo.Login(request.Username, request.Password).ConfigureAwait(true);

            if (!response.Success)
            {
                return this.BadRequest(response);
            }

            return this.Ok(response);
        }
    }
}
