namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CartVM
    {
        public string CartLabel { get; set; } = default!;
    }

    public class CartWithProductsVM
    {
        public string CartLabel { get; set; } = default!;
        public List<string>? ProductNames { get; set; }
    }
}
