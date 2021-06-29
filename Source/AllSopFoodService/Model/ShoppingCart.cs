namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    public class ShoppingCart
    {
        public int Id { get; set; }
        public string CartLabel { get; set; } = default!;
        public string UserName { get; set; } = "Default User";
        public bool IsDiscounted { get; set; } = default!;
        // Navigation Properties
        public int? VoucherId { get; set; }
        public Promotion? VoucherCode { get; set; } //assuming each Cart can only claim one Voucher at a time
        public List<FoodProduct_ShoppingCart> FoodProduct_Carts { get; set; } = default!;
    }
}
