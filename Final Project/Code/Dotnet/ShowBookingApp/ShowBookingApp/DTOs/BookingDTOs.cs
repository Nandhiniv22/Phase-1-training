namespace ShowBookingApp.DTOs
{
    public class BookingRequestDto
    {
        public int MovieId { get; set; }
        public int TheatreId { get; set; }
        public List<int> SeatIds { get; set; } = new List<int>();
        public decimal TotalPrice { get; set; }
    }

    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public int TheatreId { get; set; }
        public string TheatreName { get; set; } = string.Empty;
        public DateTime BookingTime { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public List<string> Seats { get; set; } = new List<string>();
    }
}
