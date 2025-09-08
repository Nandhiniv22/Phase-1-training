namespace ShowBookingApp.DTOs
{
    public class TheatreSearchResultDto
    {
        public int TheatreId { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public List<MovieDto> Movies { get; set; } = new();
    }
}
