using ShowBookingApp.Models;

public interface IPaymentRepo
{
    Task<Payment?> GetPaymentByIdAsync(string paymentId);
    Task CreatePaymentAsync(Payment payment);
    Task UpdatePaymentStatusAsync(string paymentId, PaymentStatus status);
    Task UpdateBookingPaymentStatusAsync(int bookingId, PaymentStatus status);
    Task<PaymentDetailsDto?> GetPaymentDetailsByBookingIdAsync(int bookingId);
    Task UpdatePaymentStatusAsync(Payment payment, PaymentStatus status);
    Task<PaymentDetailsDto?> GetPaymentDetailsByPaymentIdAsync(string paymentId);
    Task<List<PaymentDetailsDto>> GetBookingsByUserIdAsync(int userId);
}
