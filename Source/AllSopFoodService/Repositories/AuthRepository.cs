#nullable disable
namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Model;
    using ViewModels;
    using ViewModels.UserAuth;

    public class AuthRepository : IAuthRepository
    {
        private readonly FoodDbContext context;
        private readonly IConfiguration configuration;

        public AuthRepository(FoodDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }


        public async Task<ServiceResponse<string>> LoginAsync(string username, string password)
        {
            var response = new ServiceResponse<string>();

            var user = await this.context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower(CultureInfo.CurrentCulture).Equals(username.ToLower(CultureInfo.CurrentCulture), StringComparison.Ordinal))
                    .ConfigureAwait(true);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found!";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
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

        public async Task<ServiceResponse<int>> RegisterAsync(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if (await this.UserExistsAsync(user.UserName).ConfigureAwait(true))
            {
                response.Success = false;
                response.Message = "User already exists!";
                return response;
            }

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync().ConfigureAwait(true);
            response.Data = user.Id;

            return response;
        }
        public async Task<bool> UserExistsAsync(string username)
        {
            if (await this.context.Users.AnyAsync(x => x.UserName.ToLower(CultureInfo.CurrentCulture).Equals(username.ToLower(CultureInfo.CurrentCulture), StringComparison.Ordinal))
                    .ConfigureAwait(true))
            {
                return true;
            }

            return false;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
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
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString(CultureInfo.CurrentCulture)),
                new(ClaimTypes.Name, user.UserName)
            };
            //Symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
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


        public async Task<ServiceResponse<List<UserAccountVm>>> GetAllUsersAsync()
        {
            var response = new ServiceResponse<List<UserAccountVm>>();
            try
            {
                var allusers = await this.context.Users.Select(u => new UserAccountVm
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
