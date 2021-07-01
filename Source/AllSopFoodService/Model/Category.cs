#nullable disable

namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.EntityFrameworkCore;

    public class Category
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public bool IsAvailable { get; set; } = default!;
        public virtual List<Product> FoodProducts { get; set; }
    }
}
