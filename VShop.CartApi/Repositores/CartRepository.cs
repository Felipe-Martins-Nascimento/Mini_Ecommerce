﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VShop.CartApi.Context;
using VShop.CartApi.DTOs;
using VShop.CartApi.Models;

namespace VShop.CartApi.Repositores;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    private IMapper _mapper;

    public CartRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> CleanCartAsync(string userId)
    {
        var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
        if (cartHeader is not null)
        {
            _context.CartItems.RemoveRange(_context.CartItems.Where(c => c.CartHeaderId == cartHeader.Id));
            _context.CartHeaders.Remove(cartHeader);
            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }

    public async Task<bool> CleanCarttAsync()
    {
        try
        {
            int cartItemId;
            CartItem cartItem = await _context.CartItems.FirstOrDefaultAsync();
            int total = _context.CartItems.Where(c => c.CartHeaderId == cartItem.CartHeaderId).Count();
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            if (total == 1)
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == cartItem.CartHeaderId);
                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<CartDTO> GetCartByUserAsync()
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(),
        };
        cart.CartItems = _context.CartItems.Where(c => c.CartHeaderId == cart.CartHeader.Id).Include(c => c.Product);

        return _mapper.Map<CartDTO>(cart);
    }

    public async Task<CartDTO> GetCartByUserIdAsync(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId),
        };
        cart.CartItems = _context.CartItems.Where(c => c.CartHeaderId == cart.CartHeader.Id).Include(c => c.Product);

        return _mapper.Map<CartDTO>(cart);
    }

    public async Task<bool> DeleteItemmCartAsync()
    {
        try
        {
            int cartItemId;
            CartItem cartItem = await _context.CartItems.FirstOrDefaultAsync();
            int total = _context.CartItems.Where(c => c.CartHeaderId == cartItem.CartHeaderId).Count();
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            if (total == 1)
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == cartItem.CartHeaderId);
                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<CartDTO> UpdateCartAsync(CartDTO cartDto)
    {
        Cart cart = _mapper.Map<Cart>(cartDto);
        await SaveProductInDataBase(cartDto, cart);
        var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

        if (cartHeader is null)
        {
            await CreateCartHeaderAndItems(cart);
        }
        else
        {
            await UpdateQuantityAndItems(cartDto, cart, cartHeader);
        }
        return _mapper.Map<CartDTO>(cart);
    }

    private async Task UpdateQuantityAndItems(CartDTO cartDto, Cart cart, CartHeader? cartHeader)
    {
        var cartItem = await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == cartDto.CartItems.FirstOrDefault()
                               .ProductId && p.CartHeaderId == cartHeader.Id);

        if (cartItem is null)
        {
            cart.CartItems.FirstOrDefault().CartHeaderId = cartHeader.Id;
            cart.CartItems.FirstOrDefault().Product = null;
            _context.CartItems.Add(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
        else
        {
            cart.CartItems.FirstOrDefault().Product = null;
            cart.CartItems.FirstOrDefault().Quantity += cartItem.Quantity;
            cart.CartItems.FirstOrDefault().Id = cartItem.Id;
            cart.CartItems.FirstOrDefault().CartHeaderId = cartItem.CartHeaderId;
            _context.CartItems.Update(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
    }

    private async Task CreateCartHeaderAndItems(Cart cart)
    {
        _context.CartHeaders.Add(cart.CartHeader);
        await _context.SaveChangesAsync();

        cart.CartItems.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
        cart.CartItems.FirstOrDefault().Product = null;

        _context.CartItems.Add(cart.CartItems.FirstOrDefault());
        await _context.SaveChangesAsync();
    }

    private async Task SaveProductInDataBase(CartDTO cartDto, Cart cart)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartDto.CartItems.FirstOrDefault().ProductId);

        if (product is null)
        {
            _context.Products.Add(cart.CartItems.FirstOrDefault().Product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        var cartHeaderApplyCoupon = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderApplyCoupon is not null)
        {
            cartHeaderApplyCoupon.CouponCode = couponCode;
            _context.CartHeaders.Update(cartHeaderApplyCoupon);
            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }

    public async Task<bool> DeleteCouponAsync(string userId)
    {
        var cartHeaderDeleteCoupon = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderDeleteCoupon is not null)
        {
            cartHeaderDeleteCoupon.CouponCode = "";
            _context.CartHeaders.Update(cartHeaderDeleteCoupon);
            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }
    public async Task UpdateProductStockAsync(ProductDTO product)
    {
        var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

        if (existingProduct != null)
        {
            existingProduct.Stock = product.Stock;  
            _context.Products.Update(existingProduct);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Produto com ID {product.Id} não encontrado.");
        }
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
    }

    public async Task<bool> DeleteItemCartAsync(int cartItemId)
    {
        try
        {
            CartItem cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);
            int total = _context.CartItems.Where(c => c.CartHeaderId == cartItem.CartHeaderId).Count();
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            if (total == 1)
            {
                var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == cartItem.CartHeaderId);
                _context.CartHeaders.Remove(cartHeader);
                await _context.SaveChangesAsync();
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
