#nullable disable
namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;

    public class CartVM
    {
        public int CartId { get; set; }
        public string CartLabel { get; set; } = default!;
        public bool IsDiscounted { get; set; }
        public string User { get; set; } = default!;
        public List<ProductsInCartsVM> ProductsInCart { get; set; }
    }

    public class CartSaves
    {
        public string CartLabel { get; set; } = default!;
        public bool IsDiscounted { get; set; }
        public string User { get; set; } = default!;
    }

    public class ProductsInCartsVM
    {
        public string ProductDescription { get; set; } = default!;

        public int QuantityInCart { get; set; }

        public decimal OriginalPrice { get; set; }
        // could be cartLabel
        public int CartId { get; set; }
    }
}
