#nullable disable
namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class CartItem
    {
        //used for cart sessions
        //public string CartId { get; set; }
        [Key]
        public string ItemId { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public DateTime DateCreated { get; set; }

        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual FoodProduct Product { get; set; }
    }
}