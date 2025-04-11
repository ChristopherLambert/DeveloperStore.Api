namespace DeveloperStore.Services.DTOs;

public class SaleDto
{
    public Guid Id { get; set; }
    public string SaleNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string BranchName { get; set; } = string.Empty;
    public bool Cancelled { get; set; }

    public List<SaleItemDto> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Total);
}
