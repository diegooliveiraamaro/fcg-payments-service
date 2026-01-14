using Payments.Api.Infrastructure.Persistence;
using System.Text.Json;

namespace Payments.Api.Infrastructure.Events;

public class EventStore
{
    private readonly PaymentsDbContext _context;

    public EventStore(PaymentsDbContext context)
    {
        _context = context;
    }

    public async Task SaveAsync(IEvent @event)
    {
        var storedEvent = new StoredEvent
        {
            Type = @event.GetType().Name,
            Data = JsonSerializer.Serialize(@event),
            OccurredOn = DateTime.UtcNow
        };

        _context.StoredEvents.Add(storedEvent);
        await _context.SaveChangesAsync();
    }
}