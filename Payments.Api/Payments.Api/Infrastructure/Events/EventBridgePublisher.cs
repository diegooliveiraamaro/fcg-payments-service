using Amazon.EventBridge;
using Amazon.EventBridge.Model;
using System.Text.Json;

namespace Payments.Api.Infrastructure.Events;

public class EventBridgePublisher
{
    private readonly IAmazonEventBridge _eventBridge;

    public EventBridgePublisher(IAmazonEventBridge eventBridge)
    {
        _eventBridge = eventBridge;
    }
    public async Task PublishAsync(PaymentCreatedEvent evt)
    {
        var request = new PutEventsRequest
        {
            Entries = new List<PutEventsRequestEntry>
            {
                new()
                {
                    Source = "fcg.payments",
                    DetailType ="PaymentCreated",                  
                    EventBusName = "fcg-event-bus",
                     Detail = JsonSerializer.Serialize(new
                    {
                        evt.PaymentId,
                        evt.UserId,
                        evt.GameId,
                        evt.Amount
                    })
                }
            }
        };

        var response = await _eventBridge.PutEventsAsync(request);

        if (response.FailedEntryCount > 0)
        {
            throw new Exception(
                $"Erro ao publicar evento: {JsonSerializer.Serialize(response.Entries)}"
            );
        }
    }
}
