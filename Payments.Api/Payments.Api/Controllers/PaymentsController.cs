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

    public PaymentsController(PaymentsDbContext db, EventStore eventStore)
    {
        _db = db;
        _eventStore = eventStore;
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

        // Evento de pagamento criado
        _eventStore.Add(new
        {
            Type = "PaymentCreated",
            PaymentId = payment.Id,
            payment.UserId,
            payment.GameId,
            payment.Amount
        });

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
