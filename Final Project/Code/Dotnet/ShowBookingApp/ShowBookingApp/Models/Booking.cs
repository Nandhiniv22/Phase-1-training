using System.ComponentModel.DataAnnotations;

namespace ShowBookingApp.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        // User reference
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Movie reference
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;

        // Theatre reference
        public int TheatreId { get; set; }
        public Theatre Theatre { get; set; } = null!;

        // Booking details
        public DateTime BookingTime { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        // Seats
        public List<Seat> Seats { get; set; } = new List<Seat>();
    }
}
