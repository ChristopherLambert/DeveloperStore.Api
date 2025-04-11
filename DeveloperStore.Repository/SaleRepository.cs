using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Repository;

public class SaleRepository : ISaleRepository
{
    private readonly DeveloperStoreContext _context;

    public SaleRepository(DeveloperStoreContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Sale sale)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var sale = await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
        if (sale != null)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Sale>> GetAllAsync()
    {
        return await _context.Sales.Include(s => s.Items).ToListAsync();
    }

    public async Task<Sale?> GetByIdAsync(Guid id)
    {
        return await _context.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task UpdateAsync(Sale sale)
    {
        _context.Sales.Update(sale);
        await _context.SaveChangesAsync();
    }
}
