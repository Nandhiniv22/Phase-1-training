using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Context;
using ShowBookingApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowBookingApp.Repo
{
    public class OrganizerRepo : IOrganizerRepo
    {
        private readonly AppDbContext _context;

        public OrganizerRepo(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Theatre
        public async Task AddTheatreAsync(Theatre theatre)
        {
            _context.Theatres.Add(theatre);
            await _context.SaveChangesAsync();
        }

        public async Task<Theatre?> GetTheatreByIdAsync(int theatreId)
        {
            return await _context.Theatres
                .Include(t => t.Movies)
                .Include(t => t.Seats)
                .FirstOrDefaultAsync(t => t.TheatreId == theatreId);
        }

        public async Task<IEnumerable<Theatre>> GetTheatresByOrganizerAsync(int organizerId)
        {
            return await _context.Theatres
                .Include(t => t.Movies)
                .Where(t => t.OrganizerId == organizerId)
                .ToListAsync();
        }

        // ✅ Movie
        public async Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int movieId)
        {
            return await _context.Movies
                .Include(m => m.Seats)
                .Include(m => m.Theatre)
                .FirstOrDefaultAsync(m => m.MovieId == movieId);
        }

        public async Task<IEnumerable<Movie>> GetMoviesByTheatreAsync(int theatreId)
        {
            return await _context.Movies
                .Include(m => m.Seats)
                .Where(m => m.TheatreId == theatreId)
                .ToListAsync();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            var existingMovie = await _context.Movies.FindAsync(movie.MovieId);
            if (existingMovie == null) return;

            existingMovie.Title = movie.Title;
            existingMovie.Language = movie.Language;
            existingMovie.Description = movie.Description;
            existingMovie.DurationMinutes = movie.DurationMinutes;
            existingMovie.ScreenType = movie.ScreenType;
            existingMovie.ShowDate = movie.ShowDate;
            existingMovie.ShowTime = movie.ShowTime;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }

        // ✅ Seats
        public async Task AddSeatsAsync(List<Seat> seats)
        {
            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Seat>> GetSeatsByTheatreAsync(int theatreId)
        {
            return await _context.Seats
                .Where(s => s.TheatreId == theatreId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Seat>> GetSeatsByIdsAsync(List<int> seatIds)
        {
            return await _context.Seats
                .Where(s => seatIds.Contains(s.SeatId))
                .ToListAsync();
        }
    }
}
