using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowBookingApp.DTOs;
using ShowBookingApp.Mappers;
using ShowBookingApp.Models;
using ShowBookingApp.Repo;
using System.Collections.Generic;

namespace ShowBookingApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "User,Organizer,Admin")]
    public class UserController : ControllerBase
    {
        private readonly ITheatreRepo _theatreRepo;
        private readonly IMovieRepo _movieRepo;
        private readonly ISeatRepo _seatRepo;
        private readonly IBookingRepo _bookingRepo;
        private readonly IUserRepo _userRepo;

        public UserController(ITheatreRepo theatreRepo, IMovieRepo movieRepo, ISeatRepo seatRepo, IBookingRepo bookingRepo, IUserRepo userRepo)
        {
            _theatreRepo = theatreRepo;
            _movieRepo = movieRepo;
            _seatRepo = seatRepo;
            _bookingRepo = bookingRepo;
            _userRepo = userRepo;
        }

        [HttpGet("theatre/{theatreId}/movie/{movieId}")]
        public async Task<IActionResult> GetSeats(int theatreId, int movieId)
        {
            var seats = await _seatRepo.GetSeatsByMovieAsync(movieId, theatreId);
            return Ok(seats);
        }

        [HttpPost("request-organizer")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RequestOrganizer([FromBody] RequestOrganizerDto dto)
        {
            var user = await _userRepo.GetUserByIdAsync(dto.UserId);
            if (user == null) return NotFound();

            user.Role = Role.OrganizerPending; 
            await _userRepo.UpdateUserAsync(user);

            return Ok(new { message = "Organizer request submitted." });
        }


        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommendedMovies()
        {
            try
            {
                // Get ALL movies without filtering
                var allMovies = await _movieRepo.GetAllAsync();

                var recommendedList = new List<RecommendedMovieDto>();

                foreach (var movie in allMovies)
                {
                    int bookingsCount = await _bookingRepo.GetBookingCountForMovieAsync(movie.MovieId);

                    recommendedList.Add(new RecommendedMovieDto
                    {
                        MovieId = movie.MovieId,
                        Title = movie.Title,
                        ScreenType = movie.ScreenType ?? "2D",
                        SeatCategories = movie.Seats?
                            .GroupBy(s => new { s.SeatType, s.Price })
                            .Select(g => $"{g.Key.SeatType} - ₹{g.Key.Price} ({g.Count()} seats)")
                            .ToList() ?? new List<string>(),

                        Bookings = bookingsCount,
                        Theatre = new TheatreDto
                        {
                            TheatreId = movie.Theatre.TheatreId,
                            Name = movie.Theatre.Name,
                            Location = movie.Theatre.Location
                        },

                        ShowDate = movie.ShowDate,
                        ShowTime = movie.ShowTime
                    });
                }

                // Return ALL movies, sorted by bookings first
                var sortedList = recommendedList
                    .OrderByDescending(m => m.Bookings)
                    .ThenBy(m => m.ShowDate)
                    .ThenBy(m => m.ShowTime)
                    .ToList();

                return Ok(sortedList);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetRecommendedMovies: " + ex.Message);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("theatres/{organizerId}")]
        public async Task<IActionResult> GetTheatres(int organizerId)
        {
            var theatres = await _theatreRepo.GetTheatresByOrganizerAsync(organizerId);
            return Ok(theatres.Select(t => t.ToDto()));
        }

        [HttpGet("theatre/{theatreId}/movies")]
        public async Task<IActionResult> GetMovies(int theatreId)
        {
            var theatre = await _theatreRepo.GetTheatreByIdAsync(theatreId);
            if (theatre == null) return NotFound("Theatre not found");

            var movies = await _movieRepo.GetMoviesByTheatreAsync(theatreId);
            return Ok(movies.Select(m => m.ToDto(theatre))); 
        }

        [HttpGet("theatre/{theatreId}/seats")]
        public async Task<IActionResult> GetSeats(int theatreId)
        {
            var seats = await _seatRepo.GetSeatsByTheatreAsync(theatreId);
            return Ok(seats.Select(s => s.ToDto()));
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTheatres(
        [FromQuery] string? location,
        [FromQuery] string? movieName,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
        {
            var theatres = await _theatreRepo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(location))
                theatres = theatres.Where(t => t.Location.Contains(location, StringComparison.OrdinalIgnoreCase));

            var results = new List<TheatreSearchResultDto>();

            foreach (var theatre in theatres)
            {
                var movies = await _movieRepo.GetMoviesByTheatreAsync(theatre.TheatreId);

                if (!string.IsNullOrWhiteSpace(movieName))
                    movies = movies.Where(m => m.Title.Contains(movieName, StringComparison.OrdinalIgnoreCase));

                var filteredMovies = new List<MovieDto>();

                foreach (var movie in movies)
                {
                    var seats = movie.Seats.AsQueryable();

                    if (minPrice.HasValue)
                        seats = seats.Where(s => s.Price >= minPrice.Value);
                    if (maxPrice.HasValue)
                        seats = seats.Where(s => s.Price <= maxPrice.Value);

                    filteredMovies.Add(new MovieDto
                    {
                        MovieId = movie.MovieId,
                        Title = movie.Title,
                        ScreenType = movie.ScreenType ?? "2D",
                        SeatCategories = seats
                            .GroupBy(s => new { s.SeatType, s.Price })
                            .Select(g => $"{g.Key.SeatType} - ₹{g.Key.Price} ({g.Count()} seats)")
                            .ToList(),
                        Theatre = new TheatreDto
                        {
                            TheatreId = theatre.TheatreId,
                            Name = theatre.Name,
                            Location = theatre.Location
                        },
                        ShowDate = movie.ShowDate,
                        ShowTime = movie.ShowTime
                    });
                }

                if (filteredMovies.Any())
                {
                    results.Add(new TheatreSearchResultDto
                    {
                        TheatreId = theatre.TheatreId,
                        Name = theatre.Name,
                        Location = theatre.Location,
                        Movies = filteredMovies
                    });
                }
            }

            if (!results.Any())
                return NotFound("No theatres or movies found with given filters");

            return Ok(results);
        }

        
    }
}
