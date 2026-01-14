namespace Payments.Api.Infrastructure.Events;

public class StoredEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = default!;
    public string Data { get; set; } = default!;
    public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
}

//public class StoredEvent
//{
//    public Guid Id { get; set; }
//    public string Type { get; set; } = default!;
//    public DateTime OccurredAt { get; set; }
//    public string Data { get; set; } = default!;
//}