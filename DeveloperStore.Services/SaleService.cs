using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Events;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Services.Interfaces;

namespace DeveloperStore.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _repository; 
    private readonly IDomainEventDispatcher _eventDispatcher;

    public SaleService(ISaleRepository repository, IDomainEventDispatcher eventDispatcher)
    {
        _repository = repository;
        _eventDispatcher = eventDispatcher;
    }

    public async Task<Sale> CreateSaleAsync(Sale sale)
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
        _eventDispatcher.Dispatch(new SaleCreatedEvent(sale.Id));

        return sale;
    }

    public Task<IEnumerable<Sale>> GetAllSalesAsync() => _repository.GetAllAsync();

    public Task<Sale?> GetSaleByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task UpdateSaleAsync(Sale sale)
    {
        await _repository.UpdateAsync(sale);
        _eventDispatcher.Dispatch(new SaleModifiedEvent(sale.Id));
    }

    public async Task DeleteSaleAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        _eventDispatcher.Dispatch(new SaleCancelledEvent(id));
    };
}
