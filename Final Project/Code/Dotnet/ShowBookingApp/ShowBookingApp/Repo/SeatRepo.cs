using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Context;
using ShowBookingApp.DTOs;
using ShowBookingApp.Models;
using System.Linq;

namespace ShowBookingApp.Repo
{
    public class SeatRepo : ISeatRepo
    {
        private readonly AppDbContext _context;

        public SeatRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Seat>> GetSeatsByTheatreAsync(int theatreId)
        {
            return await _context.Seats
                .Where(s => s.TheatreId == theatreId)
                .ToListAsync();
        }

        public async Task<Seat?> GetSeatByIdAsync(int seatId)
        {
            return await _context.Seats.FindAsync(seatId);
        }

        public async Task AddSeatsAsync(int theatreId, int movieId, List<Seat> seats)
        {
            foreach (var seat in seats)
            {
                seat.TheatreId = theatreId;
                seat.MovieId = movieId;
                seat.IsAvailable = true;
            }

            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Seat>> GetSeatsByMovieAsync(int movieId, int theatreId)
        {
            return await _context.Seats
                .Where(s => s.MovieId == movieId && s.TheatreId == theatreId)
                .ToListAsync();
        }

        public async Task UpdateSeatAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSeatsAsync(List<Seat> seats)
        {
            _context.Seats.UpdateRange(seats);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Seat>> GetSeatsByIdsAsync(List<int> seatIds)
        {
            return await _context.Seats
                .Where(s => seatIds.Contains(s.SeatId))
                .ToListAsync();
        }

    }
}
