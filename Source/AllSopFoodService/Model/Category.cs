#nullable disable

namespace AllSopFoodService.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Label { get; set; }

        public bool IsAvailable { get; set; }

        public virtual List<Product> FoodProducts { get; set; }
    }
}
