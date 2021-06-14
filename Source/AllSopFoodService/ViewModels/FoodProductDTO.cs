namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FoodProductDTO
    {
        public int FoodId { get; set; }
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool InCart { get; set; }
        public string CategoryName { get; set; } = default!;
        public int CategoryId { get; set; }
    }
}
