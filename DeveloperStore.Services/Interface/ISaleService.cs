using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Services.Interfaces;

public interface ISaleService
{
    Task<IEnumerable<Sale>> GetAllSalesAsync();
    Task<Sale?> GetSaleByIdAsync(Guid id);
    Task<Sale> CreateSaleAsync(Sale sale);
    Task UpdateSaleAsync(Sale sale);
    Task DeleteSaleAsync(Guid id);
    Task CancelSaleItemAsync(Guid saleId, Guid itemId);
    Task<IEnumerable<Sale>> GetSalesPagedAsync(int page, int size, string order);
}
