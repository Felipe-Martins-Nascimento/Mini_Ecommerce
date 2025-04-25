using Virtualshop.Web.Models;

namespace Virtualshop.Web.Services.Contracts;

public interface ICouponService
{
    Task<CouponViewModel> GetDiscountCoupon(string couponCode);
}
