namespace AllSopFoodService.Model
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vouchers")]
    public class Promotion
    {
        public int Id { get; set; }

        [Column("VoucherCode")]
        public string CouponCode { get; set; } = default!;

        [Required]
        public string Description { get; set; } = default!;

        [Required]
        public bool IsClaimed { get; set; }

        public virtual List<ShoppingCart>? DiscountedCarts { get; set; } = default!;
    }
}
