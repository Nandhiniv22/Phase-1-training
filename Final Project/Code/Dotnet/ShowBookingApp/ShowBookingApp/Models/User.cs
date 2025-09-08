using System.ComponentModel.DataAnnotations;

namespace ShowBookingApp.Models
{
    public enum Role { Admin, Organizer, User, OrganizerPending }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public Role Role { get; set; }
        public bool IsApprovedOrganizer { get; set; } = false;
        public bool IsBlocked { get; set; } = false;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<Theatre> Theatres { get; set; } = new List<Theatre>();
    }
}
