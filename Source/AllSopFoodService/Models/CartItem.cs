#nullable disable
namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    // for the sake of this assignment, we will assume this store has only 1 cart
    public class CartItem
    {
        //public string CartId { get; set; }
        [Key]
        public int ItemId { get; set; }

        public int Quantity { get; set; }
        public string Description { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual FoodProduct Product { get; set; }
    }
}
