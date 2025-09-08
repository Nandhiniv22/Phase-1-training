namespace ShowBookingApp.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public bool IsApprovedOrganizer { get; set; }
        public bool IsBlocked { get; set; }
    }

    public class AnalyticsDto
    {
        public int TotalBookings { get; set; }
        public List<string> TopMovies { get; set; } = new();
    }
}
