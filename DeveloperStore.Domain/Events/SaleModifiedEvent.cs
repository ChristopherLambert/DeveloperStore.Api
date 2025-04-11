namespace DeveloperStore.Domain.Events;

public class SaleModifiedEvent
{
    public Guid SaleId { get; }

    public SaleModifiedEvent(Guid saleId)
    {
        SaleId = saleId;
    }
}
