namespace DeveloperStore.Domain.Interfaces;

public interface IDomainEventDispatcher
{
    void Dispatch(object domainEvent);
}
