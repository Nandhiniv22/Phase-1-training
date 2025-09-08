using System.ComponentModel.DataAnnotations;

namespace ShowBookingApp.Models
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }

        public string SeatNumber { get; set; } = "";
        public string SeatType { get; set; } = "Regular";
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;

        public int TheatreId { get; set; }
        public Theatre Theatre { get; set; } = null!;

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
