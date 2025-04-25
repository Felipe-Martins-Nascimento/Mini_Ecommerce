using System.ComponentModel.DataAnnotations;

namespace VShop.CartApi.DTOs;

public class CartItemDTO
{
    public int Id { get; set; }
    public ProductDTO Product { get; set; } = new ProductDTO();

    [Range(1, 99999)]
    public int Quantity { get; set; } = 1;
    public int ProductId { get; set; }
    public int CartHeaderId { get; set; }
}
