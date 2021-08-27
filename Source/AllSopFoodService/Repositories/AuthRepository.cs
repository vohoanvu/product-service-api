#nullable disable
namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Services;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.ViewModels.UserAuth;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public class AuthRepository : IAuthRepository
    {
        private readonly FoodDBContext context;
        private readonly IConfiguration configuration;
        private readonly ICartsService cartService;

        public AuthRepository(FoodDBContext context, IConfiguration configuration, ICartsService cartService)
        {
            this.context = context;
            this.configuration = configuration;
            this.cartService = cartService;
        }


        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();

            var user = await this.context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(username.ToLower())).ConfigureAwait(true);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found!";
            }
            else if (!this.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password!";
            }
            else
            {
                response.Data = this.CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if (await this.UserExists(user.UserName).ConfigureAwait(true))
            {
                response.Success = false;
                response.Message = "User already exists!";
                return response;
            }

            this.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync().ConfigureAwait(true);
            response.Data = user.Id;

            return response;
        }
        public async Task<bool> UserExists(string username)
        {
            if (await this.context.Users.AnyAsync(x => x.UserName.ToLower().Equals(username.ToLower())).ConfigureAwait(true))
            {
                return true;
            }

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private string CreateToken(User user)
        {
            // Create Claim from Username/Password
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            //Symmetric security key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this.configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public ServiceResponse<bool> DeleteUserAccount(int userId)
        {
            var response = new ServiceResponse<bool>();

            var currentUser = this.context.Users.FirstOrDefault(u => u.Id == userId);
            if (currentUser == null)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "The User account with this Id does not exist!";

                return response;
            }

            this.context.Users.Remove(currentUser);
            this.context.SaveChanges();

            response.Data = true;
            response.Success = true;
            response.Message = "This User has been deleted successfully";

            return response;
        }


        public async Task<ServiceResponse<List<UserAccountVM>>> GetAllUsers()
        {
            var response = new ServiceResponse<List<UserAccountVM>>();
            try
            {
                var allusers = await this.context.Users.Select(u => new UserAccountVM()
                {
                    UserId = u.Id,
                    Username = u.UserName
                }).ToListAsync().ConfigureAwait(true);

                response.Data = allusers;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
