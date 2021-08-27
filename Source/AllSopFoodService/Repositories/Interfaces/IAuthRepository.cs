namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.ViewModels;
    using AllSopFoodService.ViewModels.UserAuth;

    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);

        // Delete A User Account, along with his/her cart
        ServiceResponse<bool> DeleteUserAccount(int userId);
        Task<ServiceResponse<List<UserAccountVM>>> GetAllUsers();
    }
}
