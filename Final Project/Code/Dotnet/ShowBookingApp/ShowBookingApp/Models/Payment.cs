using System.ComponentModel.DataAnnotations;

namespace ShowBookingApp.Models
{
    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        Cancelled = 2
    }
    public class Payment
    {
        [Key]
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // DTOs
    public class PaymentDetailsDto
    {
        public string? PaymentId { get; set; }         // GUID string or null if not created yet
        public int BookingId { get; set; }
        public string MovieTitle { get; set; } = "";
        public string TheatreName { get; set; } = "";
        public List<string> Seats { get; set; } = new();
        public DateTime BookingTime { get; set; }
        public decimal Amount { get; set; }            // payment amount
        public PaymentStatus PaymentStatus { get; set; }
    }

}
