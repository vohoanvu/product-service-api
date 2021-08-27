namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Threading.Tasks;

    [Table("Vouchers")]
    public class Promotion
    {
        public int Id { get; set; }
        [Column("VoucherCode")]
        public string CouponCode { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsClaimed { get; set; }
        public virtual List<ShoppingCart>? DiscountedCarts { get; set; } = default!;
    }
}
