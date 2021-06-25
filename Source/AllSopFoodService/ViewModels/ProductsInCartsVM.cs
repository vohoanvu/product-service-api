namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductsInCartsVM
    {
        public string ProductDescription { get; set; } = default!;

        public int QuantityInCart { get; set; }

        public decimal OriginalPrice { get; set; }
    }


}
