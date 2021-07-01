#nullable disable

namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        //Navigation Properties
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } // this line is enough to automatically set up a relationship
        public virtual List<FoodProduct_ShoppingCart> FoodProduct_Carts { get; set; }
    }
}
