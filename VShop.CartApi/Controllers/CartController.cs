﻿using Microsoft.AspNetCore.Mvc;
using VShop.CartApi.DTOs;
using VShop.CartApi.Repositores;

namespace VShop.CartApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartRepository _repository;

    public CartController(ICartRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderDTO>> Checkout(CheckoutHeaderDTO checkoutDto)
    {
        var cart = await _repository.GetCartByUserAsync();
        if (cart is null)
        {
            return NotFound($"Cart Not found for {checkoutDto.UserId}");
        }
        checkoutDto.CartItems = cart.CartItems;
        checkoutDto.DateTime = DateTime.Now;

        return Ok(checkoutDto);
    }

    [HttpPost("applycoupon")]
    public async Task<ActionResult<CartDTO>> ApplyCoupon(CartDTO cartDto)
    {
        var result = await _repository.ApplyCouponAsync(cartDto.CartHeader.UserId, cartDto.CartHeader.CouponCode);
        if (!result)
        {
            return NotFound($"CartHeader not found for userId = {cartDto.CartHeader.UserId}");
        }
        return Ok(result);
    }

    [HttpDelete("deletecoupon/{userId}")]
    public async Task<ActionResult<CartDTO>> DeleteCoupon(string userId)
    {
        var result = await _repository.DeleteCouponAsync(userId);
        if (!result)
        {
            return NotFound($"Discount Coupon not found for userId = {userId}");
        }
        return Ok(result);
    }

    [HttpGet("getcart")]
    public async Task<ActionResult<CartDTO>> GetByUserId()
    {
        var cartDto = await _repository.GetCartByUserAsync();
        if (cartDto is null) return NotFound();

        return Ok(cartDto);
    }


    [HttpPost("addcart")]
    public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDto)
    {
        foreach (var item in cartDto.CartItems)
        {
            long quantity = item.Quantity;
            long stock = item.Product.Stock;
            if (stock < quantity)
            {
                throw new Exception($"Estoque insuficiente para o produto '{item.Product.Name}'. Estoque disponível: {stock}, quantidade solicitada: {quantity}.");
            }
        }
        var cart = await _repository.UpdateCartAsync(cartDto);

        if (cart is null) return NotFound();
        return Ok(cart);
    }

    [HttpPut("updatecart")]
    public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDto)
    {
        var cart = await _repository.UpdateCartAsync(cartDto);
        if (cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpDelete("deletecart/{id}")]
    public async Task<ActionResult<bool>> DeleteCart(int id)
    {
        var status = await _repository.DeleteItemCartAsync(id);
        if (!status) return BadRequest();

        return Ok(status);
    }

    [HttpDelete("clearcart/{userId}")]
    public async Task<ActionResult> ClearCart(string userId)
    {
        var result = await _repository.CleanCartAsync(userId);

        if (!result) return Ok();

        return Ok();
    }

    [HttpDelete("clearcart")]
    public async Task<ActionResult> ClearCartt()
    {
        var status = await _repository.DeleteItemmCartAsync();
        if (!status) return BadRequest();

        return Ok(status);
    }
}
