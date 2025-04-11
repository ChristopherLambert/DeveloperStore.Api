using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Services;

public class SaleService
{
    private readonly ISaleRepository _repository;

    public SaleService(ISaleRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateSaleAsync(Sale sale)
    {
        foreach (var item in sale.Items)
        {
            if (item.Quantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 identical items");

            if (item.Quantity >= 10)
                item.Discount = item.UnitPrice * item.Quantity * 0.20m;
            else if (item.Quantity >= 4)
                item.Discount = item.UnitPrice * item.Quantity * 0.10m;
            else
                item.Discount = 0;
        }

        Console.WriteLine("Event: SaleCreated");
        await _repository.AddAsync(sale);
    }

    public Task<IEnumerable<Sale>> GetAllAsync() => _repository.GetAllAsync();
    public Task<Sale?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);
    public Task UpdateAsync(Sale sale) => _repository.UpdateAsync(sale);
    public Task DeleteAsync(Guid id) => _repository.DeleteAsync(id);
}
