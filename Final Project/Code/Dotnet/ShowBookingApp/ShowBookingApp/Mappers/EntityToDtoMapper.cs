using ShowBookingApp.DTOs;
using ShowBookingApp.Models;
using System.Linq;

namespace ShowBookingApp.Mappers
{
    public static class EntityToDtoMapper
    {
        // ✅ User → UserDto
        public static UserDto ToDto(this User user) => new UserDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            IsApprovedOrganizer = user.IsApprovedOrganizer,
            IsBlocked = user.IsBlocked
        };

        // ✅ Theatre → TheatreDto
        public static TheatreDto ToDto(this Theatre theatre) => new TheatreDto
        {
            TheatreId = theatre.TheatreId,
            Name = theatre.Name,
            Location = theatre.Location,
            OrganizerId = theatre.OrganizerId
        };

        // ✅ Movie → MovieDto
        // MovieMapper.cs
        public static MovieDto ToDto(this Movie movie, Theatre theatre, IEnumerable<Seat>? seats = null)
        {
            return new MovieDto
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                ScreenType = movie.ScreenType,
                SeatCategories = seats?.Select(s => $"{s.SeatType} - ₹{s.Price}").ToList() ?? new List<string>(),
                Theatre = new TheatreDto
                {
                    TheatreId = theatre.TheatreId,
                    Name = theatre.Name,
                    Location = theatre.Location
                },
                ShowDate = movie.ShowDate,
                ShowTime = movie.ShowTime
            };
        }

        // ✅ Seat → SeatDto
        public static SeatDto ToDto(this Seat seat) => new SeatDto
        {
            SeatId = seat.SeatId,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType,
            Price = seat.Price,
            IsAvailable = seat.IsAvailable,
            TheatreId = seat.TheatreId
        };

        public static BookingResponseDto ToDto(this Booking booking)
        {
            return new BookingResponseDto
            {
                BookingId = booking.BookingId,
                MovieId = booking.MovieId,
                MovieTitle = booking.Movie?.Title ?? "",
                TheatreId = booking.TheatreId,
                TheatreName = booking.Theatre?.Name ?? "",
                BookingTime = booking.BookingTime,
                TotalPrice = booking.TotalPrice,
                Seats = booking.Seats.Select(s => s.SeatNumber).ToList(),
                PaymentStatus = booking.PaymentStatus.ToString()
            };
        }

        public static RecommendedMovieDto ToRecommendedDto(this Movie movie, Theatre theatre, int bookings, IEnumerable<Seat>? seats = null)
        {
            var dto = movie.ToDto(theatre, seats);
            return new RecommendedMovieDto
            {
                MovieId = dto.MovieId,
                Title = dto.Title,
                ScreenType = dto.ScreenType,
                SeatCategories = dto.SeatCategories,
                Theatre = dto.Theatre,
                Bookings = bookings
            };
        }
    }
}
