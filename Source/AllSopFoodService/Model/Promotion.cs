namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Promotion
    {
        [Key]
        public int Id { get; set; }
        public string CouponCode { get; set; } = default!;

        public bool IsClaimed { get; set; }
    }
}
