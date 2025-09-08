using System.ComponentModel.DataAnnotations;

namespace ShowBookingApp.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        public string Title { get; set; } = "";
        public string Language { get; set; } = "";
        public string Description { get; set; } = "";
        public int DurationMinutes { get; set; }
        public string ScreenType { get; set; } = "2D";          
        public List<string> SeatCategories { get; set; } = new List<string>();

        public DateOnly ShowDate { get; set; }   
        public TimeSpan ShowTime { get; set; }

        public int TheatreId { get; set; }
        public Theatre Theatre { get; set; } = null!;
        public List<Seat> Seats { get; set; } = new List<Seat>();
    }
}
