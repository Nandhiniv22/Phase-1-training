using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowBookingApp.Models;
using ShowBookingApp.Repo;

[ApiController]
[Route("api/payment")]
[Authorize(Roles = "User")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentRepo _paymentRepo;

    public PaymentController(IPaymentRepo paymentRepo)
    {
        _paymentRepo = paymentRepo;
    }

    [HttpPost("initiate")]
    public async Task<IActionResult> InitiatePayment([FromBody] PaymentRequestDto dto)
    {
        var booking = await _paymentRepo.GetPaymentDetailsByBookingIdAsync(dto.BookingId);
        if (booking == null)
            return NotFound("Booking not found");

        var payment = new Payment
        {
            BookingId = dto.BookingId,
            Amount = booking.Amount,   // <-- always use booking total price
            Status = PaymentStatus.Pending
        };

        await _paymentRepo.CreatePaymentAsync(payment);

        // Simulate a payment URL for QR or gateway
        var paymentUrl = $"https://fakepayment.com/pay/{payment.PaymentId}";

        return Ok(new { payment.PaymentId, paymentUrl, payment.Amount });
    }

    [HttpGet("details/{bookingId}")]
    [AllowAnonymous] // or keep Authorize depending on your requirements
    public async Task<IActionResult> GetPaymentDetails(int bookingId)
    {
        var dto = await _paymentRepo.GetPaymentDetailsByBookingIdAsync(bookingId);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpGet("{paymentId}")]
    public async Task<IActionResult> GetPaymentDetailsByPaymentId(string paymentId)
    {
        var dto = await _paymentRepo.GetPaymentDetailsByPaymentIdAsync(paymentId);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetBookingsByUser(int userId)
    {
        var bookings = await _paymentRepo.GetBookingsByUserIdAsync(userId);
        if (bookings == null || !bookings.Any()) return NotFound();

        return Ok(bookings);
    }



    [HttpGet("status/{paymentId}")]
    public async Task<IActionResult> GetPaymentStatus(string paymentId)
    {
        var payment = await _paymentRepo.GetPaymentByIdAsync(paymentId);
        if (payment == null) return NotFound();

        return Ok(new { payment.PaymentId, payment.Status });
    }

    [HttpPost("complete/{paymentId}")]
    public async Task<IActionResult> CompletePayment(string paymentId)
    {
        var payment = await _paymentRepo.GetPaymentByIdAsync(paymentId);
        if (payment == null) return NotFound();

        await _paymentRepo.UpdatePaymentStatusAsync(paymentId, PaymentStatus.Paid);
        await _paymentRepo.UpdateBookingPaymentStatusAsync(payment.BookingId, PaymentStatus.Paid);

        return Ok(new { message = "Payment completed successfully" });
    }

    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdatePaymentDto dto)
    {
        if (!Enum.IsDefined(typeof(PaymentStatus), dto.Status))
            return BadRequest("Invalid payment status");

        var payment = await _paymentRepo.GetPaymentByIdAsync(dto.PaymentId);
        if (payment == null)
            return NotFound("Payment not found");

        await _paymentRepo.UpdatePaymentStatusAsync(payment, (PaymentStatus)dto.Status);

        return Ok(new
        {
            payment.PaymentId,
            payment.BookingId,
            payment.Amount,
            Status = payment.Status.ToString()
        });
    }
}

public class PaymentRequestDto
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
}

public class UpdatePaymentDto
{
    public string PaymentId { get; set; } = null!;
    public int Status { get; set; }  // 'Paid' / 'Cancelled'
}