namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShoppingCart
    {
        public int Id { get; set; }
        public string CartLabel { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public bool IsDiscounted { get; set; } = default!;
        // Navigation Properties
        public List<FoodProduct_ShoppingCart>? FoodProduct_Carts { get; set; }
    }
}
