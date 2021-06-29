namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;
    // This Model Entity is used to demonstrate the many-many relationships between ShoppingCart and FoodProduct
    public class FoodProduct_ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product FoodProduct { get; set; } = default!;
        public int QuantityInCart { get; set; }
        public int CartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; } = default!;
    }
}
