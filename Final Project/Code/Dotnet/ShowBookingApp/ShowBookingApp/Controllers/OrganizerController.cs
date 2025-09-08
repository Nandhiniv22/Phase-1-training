using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowBookingApp.DTOs;
using ShowBookingApp.Mappers;
using ShowBookingApp.Models;
using ShowBookingApp.Repo;

namespace ShowBookingApp.Controllers
{
    [ApiController]
    [Route("api/organizer")]
    [Authorize(Roles = "Organizer")]
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerRepo _organizerRepo;
        private readonly ISeatRepo _seatRepo;

        public OrganizerController(IOrganizerRepo organizerRepo, ISeatRepo seatRepo)
        {
            _organizerRepo = organizerRepo;
            _seatRepo = seatRepo;
        }

        [HttpPost("theatre/{organizerId}")]
        public async Task<IActionResult> AddTheatre(int organizerId, [FromBody] CreateTheatreDto dto)
        {
            if (dto == null) return BadRequest("Invalid theatre data.");

            var theatre = new Theatre
            {
                Name = dto.Name,
                Location = dto.Location,
                OrganizerId = organizerId
            };

            await _organizerRepo.AddTheatreAsync(theatre);
            return Ok(new { message = "Theatre added successfully!", theatreId = theatre.TheatreId });
        }

        // ✅ Get Theatres by Organizer
        [HttpGet("my-theatres/{organizerId}")]
        public async Task<IActionResult> GetMyTheatres(int organizerId)
        {
            var theatres = await _organizerRepo.GetTheatresByOrganizerAsync(organizerId);
            return Ok(theatres.Select(t => new TheatreDto
            {
                TheatreId = t.TheatreId,
                Name = t.Name,
                Location = t.Location,
                OrganizerId = t.OrganizerId
            }));
        }

        [HttpPost("theatre/{theatreId}/movie")]
        public async Task<IActionResult> AddMovie(int theatreId, [FromBody] CreateMovieDto dto)
        {
            int organizerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var theatre = await _organizerRepo.GetTheatreByIdAsync(theatreId);
            if (theatre == null || theatre.OrganizerId != organizerId)
                return Unauthorized("You do not own this theatre");

            var movie = dto.ToEntity(theatreId);
            movie.ScreenType ??= "2D";

            await _organizerRepo.AddMovieAsync(movie);

            return Ok(new
            {
                message = "Movie added successfully",
                movieId = movie.MovieId,
                showDate = movie.ShowDate.ToShortDateString(),
                showTime = movie.ShowTime.ToString(@"hh\:mm")
            });
        }

        [HttpGet("theatre/{theatreId}/movies")]
        public async Task<IActionResult> GetMoviesByTheatre(int theatreId)
        {
            var theatre = await _organizerRepo.GetTheatreByIdAsync(theatreId);
            if (theatre == null) return NotFound("Theatre not found");

            int organizerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (theatre.OrganizerId != organizerId)
                return Unauthorized("You do not own this theatre");

            var movies = await _organizerRepo.GetMoviesByTheatreAsync(theatreId);

            return Ok(movies.Select(m => new MovieDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Language = m.Language,
                Description = m.Description,
                DurationMinutes = m.DurationMinutes,
                ScreenType = m.ScreenType,
                ShowDate = m.ShowDate,
                ShowTime = m.ShowTime
            }));
        }


        [HttpPost("movies/{movieId}/seats")]
        public async Task<IActionResult> AddSeats(int movieId, [FromBody] List<SeatDto> seatsDto)
        {
            if (!seatsDto.Any())
                return BadRequest("No seats provided");

            // Get theatreId from the first seat (or modify DTO to include movieId + theatreId)
            int theatreId = seatsDto.First().TheatreId;

            var seats = seatsDto.Select(s => new Seat
            {
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType,
                Price = s.Price,
                IsAvailable = s.IsAvailable,
                TheatreId = theatreId,
                MovieId = movieId
            }).ToList();

            await _seatRepo.AddSeatsAsync(theatreId, movieId, seats);

            return Ok(new { message = $"{seats.Count} seats added successfully!" });
        }

        [HttpDelete("movie/{movieId}")]
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            int organizerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var movie = await _organizerRepo.GetMovieByIdAsync(movieId);

            if (movie == null) return NotFound("Movie not found");
            if (movie.Theatre.OrganizerId != organizerId) return Unauthorized("You do not own this theatre");

            await _organizerRepo.DeleteMovieAsync(movieId);
            return Ok(new { message = "Movie deleted successfully" });
        }

        [HttpPut("movie/{movieId}")]
        public async Task<IActionResult> EditMovie(int movieId, [FromBody] CreateMovieDto dto)
        {
            int organizerId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var movie = await _organizerRepo.GetMovieByIdAsync(movieId);

            if (movie == null) return NotFound("Movie not found");
            if (movie.Theatre.OrganizerId != organizerId) return Unauthorized("You do not own this theatre");

            movie.Title = dto.Title;
            movie.Language = dto.Language;
            movie.Description = dto.Description;
            movie.DurationMinutes = dto.DurationMinutes;
            movie.ScreenType = dto.ScreenType;
            movie.ShowDate = dto.ShowDate;
            movie.ShowTime = dto.ShowTime;

            await _organizerRepo.UpdateMovieAsync(movie);
            return Ok(new { message = "Movie updated successfully" });
        }


        [HttpGet("theatre/{theatreId}/bookings")]
        public async Task<IActionResult> GetTheatreBookings(int theatreId, [FromServices] IBookingRepo bookingRepo)
        {
            var bookings = await bookingRepo.GetBookingsByTheatreIdAsync(theatreId);
            return Ok(bookings.Select(b => b.ToDto()));
        }

    }
}
