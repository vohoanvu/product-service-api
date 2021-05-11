#nullable disable

namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Table("categories")]
    public partial class Category
    {
        public Category() => this.FoodProducts = new HashSet<FoodProduct>();

        [Key]
        public int Id { get; set; }
        [Required]
        public string Label { get; set; }
        public bool IsAvailable { get; set; }

        [InverseProperty(nameof(FoodProduct.Category))]
        public virtual ICollection<FoodProduct> FoodProducts { get; set; }
    }
}
