﻿using DeveloperStore.Domain.Entities;
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

    public async Task<IEnumerable<Sale>> GetSalesPagedAsync(int page, int size, string order)
    {
        var allSales = await _repository.GetAllAsync();

        var ordered = order.ToLower() == "desc"
            ? allSales.OrderByDescending(s => s.Date)
            : allSales.OrderBy(s => s.Date);

        return ordered
            .Skip((page - 1) * size)
            .Take(size);
    }

    public Task<Sale?> GetSaleByIdAsync(Guid id) => _repository.GetByIdAsync(id);

    public async Task CancelSaleItemAsync(Guid saleId, Guid itemId)
    {
        var sale = await _repository.GetByIdAsync(saleId);
        if (sale == null)
            throw new InvalidOperationException("Venda não encontrada");

        var item = sale.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new InvalidOperationException("Item não encontrado");

        // Remove item da venda
        sale.Items.Remove(item);
        await _repository.UpdateAsync(sale);

        // Dispara o evento
        _eventDispatcher.Dispatch(new ItemCancelledEvent(saleId, itemId));
    }

    public async Task UpdateSaleAsync(Sale sale)
    {
        await _repository.UpdateAsync(sale);
        _eventDispatcher.Dispatch(new SaleModifiedEvent(sale.Id));
    }

    public async Task DeleteSaleAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        _eventDispatcher.Dispatch(new SaleCancelledEvent(id));
    }
}
