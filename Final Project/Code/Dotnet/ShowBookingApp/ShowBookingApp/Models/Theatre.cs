using System.ComponentModel.DataAnnotations;

namespace ShowBookingApp.Models
{
    public class Theatre
    {
        [Key]
        public int TheatreId { get; set; }

        public string Name { get; set; } = "";
        public string Location { get; set; } = "";

        public int OrganizerId { get; set; }
        public User Organizer { get; set; } = null!;

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
        public ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public List<Booking> Bookings { get; set; } = new();
    }
}
