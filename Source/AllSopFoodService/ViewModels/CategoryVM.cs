namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CategoryVM
    {
        public string Label { get; set; } = default!;

        public bool IsAvailable { get; set; }
    }

    public class CategoryWithProducts
    {
        public string Label { get; set; } = default!;

        public List<FoodProductVM> Products { get; set; } = default!;
    }

    //public class Product_CartVM
    //{
    //    public string ProductName { get; set; } = default!;
    //    public List<string> ProductCartLabels { get; set; } = default!;
    //}
}
