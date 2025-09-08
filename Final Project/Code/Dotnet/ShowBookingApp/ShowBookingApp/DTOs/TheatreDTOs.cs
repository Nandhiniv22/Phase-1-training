
namespace ShowBookingApp.DTOs
{
    public class CreateTheatreDto
    {
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
    }

    public class TheatreDto
    {
        internal List<MovieDto> movies;
        public int TheatreId { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public int OrganizerId { get; set; }
    }

    public class RecommendedMovieDto : MovieDto
    {
        public int Bookings { get; set; }
    }
    public class RequestOrganizerDto
    {
        public int UserId { get; set; }
    }
}
