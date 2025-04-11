using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Repository;

public class InMemorySaleRepository : ISaleRepository
{
    private readonly List<Sale> _sales = new();

    public Task AddAsync(Sale sale)
    {
        _sales.Add(sale);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var sale = _sales.FirstOrDefault(s => s.Id == id);
        if (sale != null) _sales.Remove(sale);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Sale>> GetAllAsync() => Task.FromResult(_sales.AsEnumerable());

    public Task<Sale?> GetByIdAsync(Guid id) => Task.FromResult(_sales.FirstOrDefault(s => s.Id == id));

    public Task UpdateAsync(Sale sale)
    {
        var existing = _sales.FirstOrDefault(s => s.Id == sale.Id);
        if (existing != null)
        {
            _sales.Remove(existing);
            _sales.Add(sale);
        }
        return Task.CompletedTask;
    }
}
