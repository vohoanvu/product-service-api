#nullable disable

namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    [Index(nameof(CategoryId), Name = "IX_FoodProducts_CategoryId")]
    public partial class FoodProduct
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsInCart { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("FoodProducts")]
        public virtual Category Category { get; set; }
    }
}
