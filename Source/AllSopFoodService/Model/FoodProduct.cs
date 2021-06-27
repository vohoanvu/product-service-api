#nullable disable

namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    public partial class FoodProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        //Navigation Properties
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<FoodProduct_ShoppingCart> FoodProduct_Carts { get; set; }
    }
}
