namespace DeveloperStore.Domain.Entities;

public class Sale
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public List<SaleItem> Items { get; set; } = new();
    public bool Cancelled { get; set; }

    public decimal Total => Items.Sum(i => i.Total);
}