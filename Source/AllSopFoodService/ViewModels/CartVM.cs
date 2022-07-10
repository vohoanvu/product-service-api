#nullable disable
namespace AllSopFoodService.ViewModels
{
    using System.Collections.Generic;

    public class CartVM
    {
        public int CartId { get; set; }
        public string CartLabel { get; set; } = default!;
        public bool IsDiscounted { get; set; }
        public int UserId { get; set; }
        public List<ProductsInCartsVM> ProductsInCart { get; set; }
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
