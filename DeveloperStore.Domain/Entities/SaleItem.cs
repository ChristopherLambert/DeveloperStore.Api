namespace DeveloperStore.Domain.Entities;

public class SaleItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Discount { get; set; }
    public decimal Total => (UnitPrice * Quantity) - Discount;

    public Guid SaleId { get; set; }              // 🔗 Chave estrangeira para Sale
    public Sale Sale { get; set; } = null!;       // 🔁 Propriedade de navegação
}
