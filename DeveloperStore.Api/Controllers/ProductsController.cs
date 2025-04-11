using DeveloperStore.Services.DTOs;
using DeveloperStore.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Get(
        [FromQuery(Name = "_order")] string? order = null,
        [FromQuery] string? title = null,
        [FromQuery] string? category = null,
        [FromQuery] decimal? price = null,
        [FromQuery(Name = "_minPrice")] decimal? minPrice = null,
        [FromQuery(Name = "_maxPrice")] decimal? maxPrice = null)
    {
        var result = await _productService.GetProductsAsync(order, title, category, price, minPrice, maxPrice);
        return Ok(result);
    }
}
