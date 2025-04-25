using Virtualshop.Web.Models;

namespace Virtualshop.Web.Services.Contracts;

public interface ICartService
{
    Task<CartViewModel> GetCartByUserIdAsync(string userId);
    Task<CartViewModel> GetCartByUserAsync();
    Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM);
    Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM);
    Task<bool> RemoveItemFromCartAsync(int cartId);
    Task<bool> ApplyCouponAsync(CartViewModel cartVM);
    Task<bool> RemoveCouponAsync(string userId);
    Task<bool> ClearCartAsync(string userId);
    Task<bool> ClearCarttAsync();
    Task<CartHeaderViewModel> CheckoutAsync(CartHeaderViewModel cartHeader);
}
