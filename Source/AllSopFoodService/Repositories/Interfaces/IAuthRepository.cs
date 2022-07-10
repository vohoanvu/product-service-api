namespace AllSopFoodService.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model;
    using ViewModels;
    using ViewModels.UserAuth;

    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> RegisterAsync(User user, string password);
        Task<ServiceResponse<string>> LoginAsync(string username, string password);
        Task<bool> UserExistsAsync(string username);

        // Delete A User Account, along with his/her cart
        ServiceResponse<bool> DeleteUserAccount(int userId);
        Task<ServiceResponse<List<UserAccountVm>>> GetAllUsersAsync();
    }
}
