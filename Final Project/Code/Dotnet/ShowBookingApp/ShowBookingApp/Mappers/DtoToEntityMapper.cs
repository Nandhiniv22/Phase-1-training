using ShowBookingApp.DTOs;
using ShowBookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShowBookingApp.Mappers
{
    public static class DtoToEntityMapper
    {
        // ✅ CreateTheatreDto → Theatre
        public static Theatre ToEntity(this CreateTheatreDto dto, int organizerId) =>
            new Theatre
            {
                Name = dto.Name,
                Location = dto.Location,
                OrganizerId = organizerId
            };

        // ✅ CreateMovieDto → Movie
        public static Movie ToEntity(this CreateMovieDto dto, int theatreId) =>
        new Movie
        {
            Title = dto.Title,
            Language = dto.Language,
            Description = dto.Description,
            DurationMinutes = dto.DurationMinutes,
            ScreenType = dto.ScreenType,
            TheatreId = theatreId,
            SeatCategories = new List<string>(),
            ShowDate = dto.ShowDate,
            ShowTime = dto.ShowTime
        };

        // ✅ CreateSeatDto → Seat
        public static Seat ToEntity(this CreateSeatDto dto, int theatreId) => new Seat
        {
            SeatNumber = dto.SeatNumber,
            SeatType = dto.SeatType,
            Price = dto.Price,
            IsAvailable = dto.IsAvailable,
            TheatreId = dto.TheatreId
        };

        public static Booking ToEntity(this BookingRequestDto dto, int userId)
        {
            return new Booking
            {
                UserId = userId,
                MovieId = dto.MovieId,
                TheatreId = dto.TheatreId,
                TotalPrice = dto.TotalPrice,
                BookingTime = DateTime.Now
            };
        }
    }
}
