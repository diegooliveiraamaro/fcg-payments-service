namespace Payments.Api.Infrastructure.Events;

public interface IEvent
{
    Guid Id { get; }
    string Type { get; }
    DateTime OccurredAt { get; }
}
