using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowBookingApp.DTOs;
using ShowBookingApp.Mappers;
using ShowBookingApp.Models;
using ShowBookingApp.Repo;

namespace ShowBookingApp.Controllers
{
    [ApiController]
    [Route("api/booking")]
    [Authorize(Roles = "User")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepo _bookingRepo;
        private readonly IMovieRepo _movieRepo;
        private readonly ISeatRepo _seatRepo;

        public BookingController(IBookingRepo bookingRepo, IMovieRepo movieRepo, ISeatRepo seatRepo)
        {
            _bookingRepo = bookingRepo;
            _movieRepo = movieRepo;
            _seatRepo = seatRepo;
        }

        [HttpPost("create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateBooking([FromBody] BookingRequestDto dto)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
                if (userId == 0) return Unauthorized("Invalid user");

                if (dto.SeatIds == null || dto.SeatIds.Count == 0)
                    return BadRequest("No seats selected");

                var movie = await _movieRepo.GetMovieByIdAsync(dto.MovieId);
                if (movie == null) return NotFound("Movie not found");

                var seats = await _seatRepo.GetSeatsByIdsAsync(dto.SeatIds);
                if (seats.Count != dto.SeatIds.Count)
                    return BadRequest("Some seats not found");
                if (seats.Any(s => !s.IsAvailable))
                    return BadRequest("Some seats are already booked");

                var booking = new Booking
                {
                    UserId = userId,
                    MovieId = movie.MovieId,
                    TheatreId = movie.TheatreId,
                    BookingTime = DateTime.UtcNow,
                    TotalPrice = seats.Sum(s => s.Price),
                    PaymentStatus = PaymentStatus.Pending,
                    Seats = seats
                };

                // Mark seats as unavailable
                foreach (var seat in seats) seat.IsAvailable = false;

                await _bookingRepo.AddBookingAsync(booking);

                return Ok(new
                {
                    bookingId = booking.BookingId,
                    totalPrice = booking.TotalPrice
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Booking creation error: " + ex);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("theatre/{theatreId}/bookings")]
        public async Task<IActionResult> GetBookingsByTheatre(int theatreId)
        {
            var bookings = await _bookingRepo.GetBookingsByTheatreAsync(theatreId);
            return Ok(bookings);
        }

        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var bookings = await _bookingRepo.GetBookingsByUserIdAsync(userId);
            return Ok(bookings.Select(b => b.ToDto()));
        }

        [HttpPost("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null || booking.UserId != userId) return Unauthorized("Cannot cancel this booking");

            // Mark seats available again
            foreach (var seat in booking.Seats) seat.IsAvailable = true;

            // If payment is pending, mark as cancelled
            if (booking.PaymentStatus == PaymentStatus.Pending)
                booking.PaymentStatus = PaymentStatus.Cancelled;

            await _bookingRepo.CancelBookingAsync(booking);

            return Ok(new { message = "Booking cancelled successfully" });
        }
    }
}
