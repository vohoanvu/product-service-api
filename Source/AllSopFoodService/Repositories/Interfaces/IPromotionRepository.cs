namespace AllSopFoodService.Repositories.Interfaces
{
    using Model;

    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        Promotion GetCouponByCode(string voucherCode);
        bool CheckVoucherExist(string voucherCode);
    }
}
