#nullable disable
namespace AllSopFoodService.ViewModels.UserAuth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserAccountVM
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        // Potentially include more information associated with each user (email, address,etc)
    }
}
