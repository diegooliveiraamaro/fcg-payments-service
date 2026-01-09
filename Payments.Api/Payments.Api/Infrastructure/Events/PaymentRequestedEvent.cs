namespace Payments.Api.Infrastructure.Events;

public class PaymentRequestedEvent : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Type => "PaymentRequested";
    public DateTime OccurredAt { get; } = DateTime.UtcNow;

    public Guid PaymentId { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
}
