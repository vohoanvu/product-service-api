#nullable disable
namespace AllSopFoodService.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AllSopFoodService.Model;
    using AllSopFoodService.Repositories.Interfaces;

    public class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
    {
        public PromotionRepository(FoodDBContext context) : base(context)
        {
        }

        public Promotion GetCouponByCode(string voucherCode) => this.context.CouponCodes.FirstOrDefault(p => p.CouponCode == voucherCode);
        public bool CheckVoucherExist(string voucherCode) => this.context.CouponCodes.Any(p => p.CouponCode == voucherCode);
    }
}
