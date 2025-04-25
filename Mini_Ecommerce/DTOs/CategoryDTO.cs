using Mini_Ecommerce.Models;
using System.ComponentModel.DataAnnotations;

namespace Mini_Ecommerce.DTOs;

public class CategoryDTO
{
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "The Name is Required")]
    public string? Name { get; set; }
    public ICollection<Product>? Products { get; set; }
}
