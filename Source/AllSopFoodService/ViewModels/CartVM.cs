namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CartVM
    {
        public int CartId { get; set; }
        public string CartLabel { get; set; } = default!;
        public bool IsDiscounted { get; set; }
        public string User { get; set; } = default!;
        public List<int> Products { get; set; } = default!;
    }

    public class AddCartDto
    {
        public string CartLabel { get; set; } = default!;
        public bool IsDiscounted { get; set; }
        public string User { get; set; } = default!;
    }

    public class CartWithProductsVM
    {
        public string CartLabel { get; set; } = default!;
        public bool IsDiscounted { get; set; }
        public List<FoodProductVM>? ProductNames { get; set; }
    }
}
