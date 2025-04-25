using Mini_Ecommerce.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Mini_Ecommerce.DTOs;

public class ProductDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The Name is Required")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "The Price is Required")]
    public decimal Price { get; set; }
    public string? Description { get; set; }

    [Required(ErrorMessage = "The Stock is Required")]
    public long Stock { get; set; }
    public string? ImageURL { get; set; }
    public string? CategoryName { get; set; }

    [JsonIgnore]
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}
