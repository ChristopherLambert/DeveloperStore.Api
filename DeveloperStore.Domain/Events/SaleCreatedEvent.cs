namespace DeveloperStore.Domain.Events;

public class SaleCreatedEvent
{
    public Guid SaleId { get; }

    public SaleCreatedEvent(Guid saleId)
    {
        SaleId = saleId;
    }
}
