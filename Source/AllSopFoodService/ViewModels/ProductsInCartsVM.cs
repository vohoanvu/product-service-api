namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;

    public class ProductsInCartsVM
    {
        public string ProductDescription { get; set; } = default!;

        public int QuantityInCart { get; set; }

        public decimal OriginalPrice { get; set; }
        // could be cartLabel
        public int CartId { get; set; }
    }

    //public class ProductCartVM
    //{
    //    public FoodProduct_ShoppingCart ProductCart { get; set; } = default!;

    //    public Product Product { get; set; } = default!;
    //    public List<FoodProduct_ShoppingCart> ProductCartList { get; set; } = default!;
    //}
}
