#nullable disable
namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        // You could include more information in the User Registration Request here....
    }
}
