using Microsoft.AspNetCore.Mvc;
using Virtualshop.Web.Models;
using Virtualshop.Web.Services.Contracts;

namespace Virtualshop.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICouponService _couponService;

    public CartController(ICartService cartService, ICouponService couponService)
    {
        _cartService = cartService;
        _couponService = couponService;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        CartViewModel? cartVM = await GetCartByUser();
        return View(cartVM);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CartViewModel cartVM)
    {
        if (string.IsNullOrEmpty(cartVM.CartHeader.CouponCode))
        {
            cartVM.CartHeader.CouponCode = "0";
            ModelState.Remove("CartHeader.CouponCode");
        }
        if (ModelState.IsValid)
        {
            var result = await _cartService.CheckoutAsync(cartVM.CartHeader);

            if(result == null)
            {
                return default;
            }

            if (result is not null)
            {
                await _cartService.ClearCarttAsync();
                return RedirectToAction(nameof(CheckoutCompleted));
            }
        }
        return View(cartVM);
    }

    [HttpGet]
    public IActionResult CheckoutCompleted()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartViewModel cartVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _cartService.ApplyCouponAsync(cartVM);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteCoupon()
    {
        var result = await _cartService.RemoveCouponAsync(GetUserId());
        if (result)
        {
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    public async Task<IActionResult> Index()
    {
        CartViewModel? cartVM = await GetCartByUser();
        if (cartVM is null)
        {
            ModelState.AddModelError("CartNotFound", "Does not exist a cart yet...Come on Shopping...");
            return View("/Views/Cart/CartNotFound.cshtml");
        }
        return View(cartVM);
    }



    private async Task<CartViewModel?> GetCartByUser()
    {
        var cart = await _cartService.GetCartByUserAsync();

        if (cart?.CartHeader is not null)
        {
            if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                var coupon = await _couponService.GetDiscountCoupon(cart.CartHeader.CouponCode);
                if (coupon?.CouponCode is not null)
                {
                    cart.CartHeader.Discount = coupon.Discount;
                }
            }
            foreach (var item in cart.CartItems)
            {
                cart.CartHeader.TotalAmount += (item.Product.Price * item.Quantity);
            }
            cart.CartHeader.TotalAmount = cart.CartHeader.TotalAmount - (cart.CartHeader.TotalAmount * cart.CartHeader.Discount) / 100;
        }
        return cart;
    }

    public async Task<IActionResult> RemoveItem(int id)
    {
        var result = await _cartService.RemoveItemFromCartAsync(id);

        if (result)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(id);
    }

    private string GetUserId()
    {
        return "Felipe";
    }
}
