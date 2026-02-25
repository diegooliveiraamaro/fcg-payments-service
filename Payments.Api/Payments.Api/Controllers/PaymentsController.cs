using Microsoft.AspNetCore.Mvc;
using Payments.Api.Application.DTOs;
using Payments.Api.Domain;
using Payments.Api.Infrastructure.Events;
using Payments.Api.Infrastructure.Persistence;

namespace Payments.Api.Controllers;

[ApiController]
[Route("payments")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentsDbContext _db;
    private readonly EventStore _eventStore;
    // private readonly EventBridgePublisher _publisher;
    private readonly RabbitPublisher _publisher;

    public PaymentsController(PaymentsDbContext db, EventStore eventStore, RabbitPublisher publisher)//, EventBridgePublisher publisher)
    {
        _db = db;
        _eventStore = eventStore;
        _publisher = publisher;
        //  _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment(ProcessPaymentRequestDto dto)
    {
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            UserId = dto.UserId,
            GameId = dto.GameId,
            Amount = dto.Amount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        var paymentCreatedEvent = new PaymentCreatedEvent
        {
            PaymentId = payment.Id,
            UserId = payment.UserId,
            GameId = payment.GameId,
            Amount = payment.Amount
        };

        await _eventStore.SaveAsync(paymentCreatedEvent);
        _publisher.Publish(paymentCreatedEvent);
        // await _publisher.PublishAsync(paymentCreatedEvent);

        // Simulação de aprovação
        payment.Status = PaymentStatus.Approved;
        await _db.SaveChangesAsync();

        return Ok(payment);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var payment = await _db.Payments.FindAsync(id);
        if (payment == null) return NotFound();

        return Ok(payment);
    }
}
