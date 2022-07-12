#nullable disable
namespace AllSopFoodService.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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

        public virtual List<FoodProductInShoppingCart> FoodProductInCarts { get; set; }
    }
}
