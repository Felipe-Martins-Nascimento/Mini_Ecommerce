using VShop.CartApi.DTOs;
using VShop.CartApi.Models;

namespace VShop.CartApi.Repositores;

public interface ICartRepository
{
    Task<CartDTO> GetCartByUserIdAsync(string userId);
    Task<CartDTO> GetCartByUserAsync();
    Task<CartDTO> UpdateCartAsync(CartDTO cart);
    Task<bool> CleanCartAsync(string userId);
    Task<bool> CleanCarttAsync();
    Task<bool> DeleteItemCartAsync(int cartItemId);
    Task<bool> DeleteItemmCartAsync();
    Task<bool> ApplyCouponAsync(string userId, string couponCode);
    Task<bool> DeleteCouponAsync(string userId);
    Task UpdateProductStockAsync(ProductDTO product);
    Task<Product> GetProductByIdAsync(int productId);
}
