using AutoMapper;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Services.DTOs;
using DeveloperStore.Services.Interfaces;
using System.Linq.Dynamic.Core;

public class ProductService : IProductService
{
    private readonly ISaleRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(ISaleRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(
        string? order, string? title, string? category, decimal? price,
        decimal? minPrice, decimal? maxPrice)
    {
        var sales = await _repository.GetAllAsync();
        var items = sales.SelectMany(s => s.Items).ToList();

        var groupedProducts = items
            .GroupBy(i => i.ProductName)
            .Select(g =>
            {
                var representative = g.First();
                var dto = _mapper.Map<ProductDto>(representative);

                dto.Rating = new RatingDto
                {
                    Rate = Math.Round(Random.Shared.NextDouble() * 2 + 3, 1), // Avaliação Aleatoria, implementar no futuro
                    Count = g.Sum(x => x.Quantity)
                };

                return dto;
            });

        var baseProducts = groupedProducts.AsQueryable();

        // FILTROS
        if (!string.IsNullOrWhiteSpace(title))
        {
            baseProducts = title.Contains("*")
                ? baseProducts.Where(p => p.Title.Contains(title.Trim('*'), StringComparison.OrdinalIgnoreCase))
                : baseProducts.Where(p => p.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            baseProducts = category.Contains("*")
                ? baseProducts.Where(p => p.Category.Contains(category.Trim('*'), StringComparison.OrdinalIgnoreCase))
                : baseProducts.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        if (price.HasValue)
            baseProducts = baseProducts.Where(p => p.Price == price.Value);

        if (minPrice.HasValue)
            baseProducts = baseProducts.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            baseProducts = baseProducts.Where(p => p.Price <= maxPrice.Value);

        // ORDENAR
        baseProducts = ApplyOrdering(order, baseProducts);
        return baseProducts.ToList();
    }

    private static IQueryable<ProductDto> ApplyOrdering(string? order, IQueryable<ProductDto> query)
    {
        if (string.IsNullOrWhiteSpace(order))
            return query.OrderBy("title"); // default

        try
        {
            return query.OrderBy(order);
        }
        catch
        {
            return query.OrderBy("title"); 
        }
    }
}
