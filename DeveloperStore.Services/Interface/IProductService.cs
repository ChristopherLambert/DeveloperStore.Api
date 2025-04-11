using DeveloperStore.Services.DTOs;

namespace DeveloperStore.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProductsAsync(
        string? order, string? title, string? category, decimal? price,
        decimal? minPrice, decimal? maxPrice);
}
