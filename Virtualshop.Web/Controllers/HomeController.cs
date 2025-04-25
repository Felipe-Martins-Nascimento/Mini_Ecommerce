using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Virtualshop.Web.Models;
using Virtualshop.Web.Services.Contracts;

namespace Virtualshop.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(ILogger<HomeController> logger, IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProducts();
        if (products is null)
        {
            return View("Error");
        }
        return View(products);
    }

    [HttpGet]
    public async Task<ActionResult<ProductViewModel>> ProductDetails(int id)
    {
        var product = await _productService.FindProductById(id);
        if (product is null)
            return View("Error");

        return View(product);
    }

    [HttpPost]
    [ActionName("ProductDetails")]
    public async Task<ActionResult<ProductViewModel>> ProductDetailsPost(ProductViewModel productVM)
    {
        CartViewModel cart = new()
        {
            CartHeader = new CartHeaderViewModel
            {
                UserId = "Felipe"
            }
        };

        CartItemViewModel cartItem = new()
        {
            Quantity = productVM.Quantity,
            ProductId = productVM.Id,
            Product = await _productService.FindProductById(productVM.Id)
        };

        List<CartItemViewModel> cartItemsVM = new List<CartItemViewModel>();
        cartItemsVM.Add(cartItem);
        cart.CartItems = cartItemsVM;

        var result = _cartService.AddItemToCartAsync(cart).Result;
        if (result is not null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(productVM);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
