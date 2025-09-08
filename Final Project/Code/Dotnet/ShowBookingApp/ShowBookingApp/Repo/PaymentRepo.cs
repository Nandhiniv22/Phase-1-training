using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Context;
using ShowBookingApp.Models;

public class PaymentRepo : IPaymentRepo
{
    private readonly AppDbContext _context;

    public PaymentRepo(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Payment?> GetPaymentByIdAsync(string paymentId)
    {
        return await _context.Payments
            .Include(p => p.Booking) // eager load booking info
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }

    public async Task CreatePaymentAsync(Payment payment)
    {
        payment.Status = PaymentStatus.Pending;
        payment.CreatedAt = DateTime.UtcNow;

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }

    public async Task<PaymentDetailsDto?> GetPaymentDetailsByPaymentIdAsync(string paymentId)
    {
        var payment = await _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.Movie)    // load Movie
            .Include(p => p.Booking)
                .ThenInclude(b => b.Theatre)  // load Theatre
            .Include(p => p.Booking)
                .ThenInclude(b => b.Seats)    // load Seats
            .FirstOrDefaultAsync(p => p.PaymentId == paymentId);

        if (payment == null) return null;

        var booking = payment.Booking;

        return new PaymentDetailsDto
        {
            PaymentId = payment.PaymentId,
            BookingId = booking.BookingId,
            MovieTitle = booking.Movie?.Title ?? "N/A",       // null-safe
            TheatreName = booking.Theatre?.Name ?? "N/A",     // null-safe
            Seats = booking.Seats?.Select(s => s.SeatNumber).ToList() ?? new List<string>(),
            BookingTime = booking.BookingTime,
            Amount = payment.Amount,
            PaymentStatus = payment.Status
        };
    }

    public async Task<List<PaymentDetailsDto>> GetBookingsByUserIdAsync(int userId)
    {
        return await _context.Payments
            .Include(p => p.Booking)
                .ThenInclude(b => b.Movie)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Theatre)
            .Include(p => p.Booking)
                .ThenInclude(b => b.Seats)
            .Where(p => p.Booking.UserId == userId)
            .Select(payment => new PaymentDetailsDto
            {
                PaymentId = payment.PaymentId,
                BookingId = payment.BookingId,
                MovieTitle = payment.Booking.Movie.Title ?? "N/A",
                TheatreName = payment.Booking.Theatre.Name ?? "N/A",
                Seats = payment.Booking.Seats.Select(s => s.SeatNumber).ToList(),
                BookingTime = payment.Booking.BookingTime,
                Amount = payment.Amount,
                PaymentStatus = payment.Status
            })
            .ToListAsync();
    }



    public async Task<PaymentDetailsDto?> GetPaymentDetailsByBookingIdAsync(int bookingId)
    {
        // Get booking with related data (adjust navigation names to your models)
        var booking = await _context.Bookings
            .Include(b => b.Movie)        // if Booking has Movie navigation
            .Include(b => b.Theatre)      // if Booking has Theatre navigation
            .Include(b => b.Seats)        // if Booking has Seats navigation (a collection)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null) return null;

        // Find payment if already created
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.BookingId == bookingId);

        var dto = new PaymentDetailsDto
        {
            PaymentId = payment?.PaymentId,      // null if payment not created yet
            BookingId = booking.BookingId,
            MovieTitle = booking.Movie?.Title ?? "Unknown",
            TheatreName = booking.Theatre?.Name ?? "Unknown",
            Seats = booking.Seats?.Select(s => s.SeatNumber).ToList() ?? new List<string>(),
            BookingTime = booking.BookingTime,
            Amount =  booking.TotalPrice,
            PaymentStatus = payment?.Status ?? booking.PaymentStatus
        };

        return dto;
    }


    public async Task UpdatePaymentStatusAsync(string paymentId, PaymentStatus status)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        if (payment == null) return;

        payment.Status = status;
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePaymentStatusAsync(Payment payment, PaymentStatus status)
    {
        payment.Status = status;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBookingPaymentStatusAsync(int bookingId, PaymentStatus status)
    {
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
        if (booking == null) return;

        booking.PaymentStatus = status;
        await _context.SaveChangesAsync();
    }
}
