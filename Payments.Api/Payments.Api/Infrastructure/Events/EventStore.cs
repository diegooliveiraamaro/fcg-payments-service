namespace Payments.Api.Infrastructure.Events;

public class EventStore
{
    private readonly List<object> _events = new();

    public void Add(object @event)
    {
        _events.Add(@event);
    }

    public IReadOnlyCollection<object> GetAll()
        => _events.AsReadOnly();
}
