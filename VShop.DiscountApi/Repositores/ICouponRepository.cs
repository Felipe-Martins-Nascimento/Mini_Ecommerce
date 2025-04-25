using VShop.DiscountApi.DTOs;

namespace VShop.DiscountApi.Repositores
{
    public interface ICouponRepository
    {
        Task<CouponDTO> GetCouponByCode(string couponCode);
    }
}
