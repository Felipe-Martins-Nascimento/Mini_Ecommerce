using System.ComponentModel.DataAnnotations;

namespace Virtualshop.Web.Models;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Nome")]
    public string? Name { get; set; }

    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Required]
    [Range(1, 9999)]
    [Display(Name = "Preço")]
    public decimal Price { get; set; }

    [Required]
    [Display(Name = "Imagem URL")]
    public string? ImageURL { get; set; }

    [Required]
    [Range(1, 9999)]
    [Display(Name = "Estoque")]
    public long Stock { get; set; }

    [Display(Name = "Nome da Categoria")]
    public string? CategoryName { get; set; }

    [Range(1, 100)]
    [Display(Name = "Quantidade")]
    public int Quantity { get; set; } = 1;

    [Display(Name = "Categoria")]
    public int CategoryId { get; set; }
}
