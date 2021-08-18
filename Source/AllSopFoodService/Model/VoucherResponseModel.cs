namespace AllSopFoodService.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class VoucherResponseModel
    {
        public bool Applied { get; set; }

        public string FailedMessage { get; set; } = default!;

        public decimal DiscountedCartPrice { get; set; }
    }
}
