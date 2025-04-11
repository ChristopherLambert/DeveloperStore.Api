using DeveloperStore.Domain.Interfaces;

namespace DeveloperStore.Services.Infrastructure;

public class ConsoleDomainEventDispatcher : IDomainEventDispatcher
{
    public void Dispatch(object domainEvent)
    {
        Console.WriteLine($"[EVENT DISPACTH] => {domainEvent.GetType().Name}: {System.Text.Json.JsonSerializer.Serialize(domainEvent)}");
    }
}
