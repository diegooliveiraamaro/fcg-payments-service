namespace Payments.Api.Application.DTOs;

public class ProcessPaymentRequestDto
{
    public Guid UserId { get; set; }
    public Guid GameId { get; set; }
    public decimal Amount { get; set; }
}
