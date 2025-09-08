namespace ShowBookingApp.DTOs
{
    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string? ScreenType { get; set; }
        public List<string>? SeatCategories { get; set; }
        public DateOnly ShowDate { get; set; }   
        public TimeSpan ShowTime { get; set; }
    }

    public class MovieDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public string ScreenType { get; set; } = "2D";
        public DateOnly ShowDate { get; set; }   
        public TimeSpan ShowTime { get; set; }
        public int TheatreId { get; set; }
        public List<string> SeatCategories { get; set; } = new List<string>();
        public TheatreDto? Theatre { get; internal set; }
    }

}
