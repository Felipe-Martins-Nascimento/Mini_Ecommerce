using System.Text;
using System.Text.Json;
using Virtualshop.Web.Models;
using Virtualshop.Web.Services.Contracts;

namespace Virtualshop.Web.Services;

public class CartService : ICartService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions? _options;
    private const string apiEndpoint = "/api/cart";
    private CartViewModel cartVM = new CartViewModel();
    private CartHeaderViewModel cartHeaderVM = new CartHeaderViewModel();

    public CartService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public async Task<CartViewModel> GetCartByUserIdAsync(string userId)
    {
        var client = _clientFactory.CreateClient("CartApi");
        using (var response = await client.GetAsync($"{apiEndpoint}/getcart/{userId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartVM = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return cartVM;
    }

    public async Task<CartViewModel> GetCartByUserAsync()
    {
        var client = _clientFactory.CreateClient("CartApi");

        using (var response = await client.GetAsync($"{apiEndpoint}/getcart/"))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartVM = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return cartVM;
    }

    public async Task<CartViewModel> AddItemToCartAsync(CartViewModel cartVM)
    {
        var client = _clientFactory.CreateClient("CartApi");
        StringContent content = new StringContent(JsonSerializer.Serialize(cartVM), Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync($"{apiEndpoint}/addcart/", content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartVM = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return cartVM;
    }

    public async Task<CartViewModel> UpdateCartAsync(CartViewModel cartVM)
    {
        var client = _clientFactory.CreateClient("CartApi");
        CartViewModel cartUpdated = new CartViewModel();

        using (var response = await client.PutAsJsonAsync($"{apiEndpoint}/updatecart/", cartVM))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartUpdated = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return cartUpdated;
    }

    public async Task<bool> RemoveItemFromCartAsync(int cartId)
    {
        var client = _clientFactory.CreateClient("CartApi");

        using (var response = await client.DeleteAsync($"{apiEndpoint}/deletecart/" + cartId))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }

    public async Task<bool> ClearCartAsync(string userId)
    {
        var client = _clientFactory.CreateClient("CartApi");

        using (var response = await client.DeleteAsync($"{apiEndpoint}/clearcart/{userId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }

    public async Task<bool> ClearCarttAsync()
    {
        var client = _clientFactory.CreateClient("CartApi");

        using (var response = await client.GetAsync($"{apiEndpoint}/getcart/"))
        {
            if (!response.IsSuccessStatusCode) return false;

            var apiResponse = await response.Content.ReadAsStreamAsync();
            var cart = await JsonSerializer.DeserializeAsync<CartViewModel>(apiResponse, _options);

            if (cart?.CartItems == null || !cart.CartItems.Any()) return true; 

            foreach (var item in cart.CartItems)
            {
                var deleteResponse = await client.DeleteAsync($"{apiEndpoint}/deletecart/{item.Id}");

                if (!deleteResponse.IsSuccessStatusCode) return false; 
            }
            return true;
        }
    }
    public async Task<bool> ApplyCouponAsync(CartViewModel cartVM)
    {
        var client = _clientFactory.CreateClient("CartApi");
        StringContent content = new StringContent(JsonSerializer.Serialize(cartVM), Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync($"{apiEndpoint}/applycoupon/", content))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }

    public async Task<bool> RemoveCouponAsync(string userId)
    {
        var client = _clientFactory.CreateClient("CartApi");

        using (var response = await client.DeleteAsync($"{apiEndpoint}/deletecoupon/{userId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
        }
        return false;
    }

    public async Task<CartHeaderViewModel> CheckoutAsync(CartHeaderViewModel cartHeaderVM)
    {
        var client = _clientFactory.CreateClient("CartApi");
        StringContent content = new StringContent(JsonSerializer.Serialize(cartHeaderVM), Encoding.UTF8, "application/json");

        using (var response = await client.PostAsync($"{apiEndpoint}/checkout/", content))
        {
            if (response.IsSuccessStatusCode)
            {
                var apiResponse = await response.Content.ReadAsStreamAsync();
                cartHeaderVM = await JsonSerializer.DeserializeAsync<CartHeaderViewModel>(apiResponse, _options);
            }
            else
            {
                return null;
            }
        }
        return cartHeaderVM;
    }
}
