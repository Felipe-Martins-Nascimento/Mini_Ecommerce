using System.ComponentModel.DataAnnotations;

namespace Virtualshop.Web.Models;

public class CategoryViewModel
{
    public int CategoryId { get; set; }
    [Required]
    public string? Name { get; set; }
}
