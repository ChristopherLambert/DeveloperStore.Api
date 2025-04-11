using Bogus;
using NSubstitute;
using Xunit;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Services;

namespace DeveloperStore.Tests.Services;

public class SaleServiceTests
{
    private readonly Faker _faker = new();
    private readonly ISaleRepository _repositoryMock;
    private readonly IDomainEventDispatcher _eventMock;
    private readonly SaleService _service;

    public SaleServiceTests()
    {
        _repositoryMock = Substitute.For<ISaleRepository>();
        _eventMock = Substitute.For<IDomainEventDispatcher>();
        _service = new SaleService(_repositoryMock, _eventMock);
    }

    [Fact]
    public async Task Should_Apply_10Percent_Discount_For_Quantity_Between_4_And_9()
    {
        // Arrange
        var unitPrice = 100m;
        var quantity = 5;
        var expectedDiscount = unitPrice * quantity * 0.10m;

        var sale = new Sale
        {
            SaleNumber = _faker.Random.Guid().ToString(),
            CustomerName = _faker.Name.FullName(),
            BranchName = "Loja Teste",
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductName = "Produto Teste",
                    Quantity = quantity,
                    UnitPrice = unitPrice
                }
            }
        };

        // Act
        await _service.CreateSaleAsync(sale);

        // Assert
        var item = sale.Items.First();
        Assert.Equal(expectedDiscount, item.Discount);
    }

    [Fact]
    public async Task Should_Apply_20Percent_Discount_For_Quantity_Between_10_And_20()
    {
        var sale = new Sale
        {
            SaleNumber = _faker.Random.Guid().ToString(),
            CustomerName = _faker.Name.FullName(),
            BranchName = "Loja Teste",
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductName = "Produto Teste",
                    Quantity = 15,
                    UnitPrice = 50
                }
            }
        };

        await _service.CreateSaleAsync(sale);

        var item = sale.Items.First();
        Assert.Equal(15 * 50 * 0.20m, item.Discount);
    }

    [Fact]
    public async Task Should_Not_Apply_Discount_For_Quantity_Less_Than_4()
    {
        var sale = new Sale
        {
            SaleNumber = _faker.Random.Guid().ToString(),
            CustomerName = _faker.Name.FullName(),
            BranchName = "Loja Teste",
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductName = "Produto Teste",
                    Quantity = 2,
                    UnitPrice = 100
                }
            }
        };

        await _service.CreateSaleAsync(sale);

        var item = sale.Items.First();
        Assert.Equal(0, item.Discount);
    }

    [Fact]
    public async Task Should_Throw_When_Quantity_Greater_Than_20()
    {
        var sale = new Sale
        {
            SaleNumber = _faker.Random.Guid().ToString(),
            CustomerName = _faker.Name.FullName(),
            BranchName = "Loja Teste",
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    ProductName = "Produto Teste",
                    Quantity = 25,
                    UnitPrice = 200
                }
            }
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateSaleAsync(sale));
        Assert.Equal("Cannot sell more than 20 identical items", ex.Message);
    }
}
