namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    // This Model Entity is used to demonstrate the many-many relationships between ShoppingCart and FoodProduct
    public class FoodProduct_ShoppingCart
    {
        public int Id { get; set; }
        public int FoodProductId { get; set; }
        public FoodProduct FoodProduct { get; set; } = default!;

        public int QuantityInCart { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = default!;
    }
}
