#nullable disable
namespace AllSopFoodService.Model
{
    using System.Collections.Generic;

    public class ShoppingCart
    {
        public int Id { get; set; }
        public string CartLabel { get; set; } = "Default Cart Label";
        public bool IsDiscounted { get; set; }
        // Navigation Properties
        public int? VoucherId { get; set; }
        public virtual Promotion VoucherCode { get; set; } //assuming each Cart can only claim one Voucher at a time
        public virtual List<FoodProductShoppingCart> FoodProductCarts { get; set; }
        //Navigation props
        public int UserId { get; set; }
        public User CartUser { get; set; }
    }
}
