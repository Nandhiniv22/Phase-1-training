namespace ShowBookingApp.DTOs
{
    public class CreateSeatDto
    {
        public string SeatNumber { get; set; } = "";
        public string SeatType { get; set; } = "Regular";
        public decimal Price { get; set; }
        public bool IsAvailable { get; internal set; }
        public int TheatreId { get; internal set; }
    }

    public class SeatDto
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; } = "";
        public string SeatType { get; set; } = "";
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int TheatreId { get; set; }
        public int MovieId { get; set; }
    }
}
