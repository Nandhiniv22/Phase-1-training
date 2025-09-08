using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Context;
using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public class MovieRepo : IMovieRepo
    {
        private readonly AppDbContext _context;
        public MovieRepo(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies.Include(m => m.Theatre).ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int movieId)
        {
            return await _context.Movies
                .Include(m => m.Seats)
                .FirstOrDefaultAsync(m => m.MovieId == movieId);
        }

        public async Task<IEnumerable<Movie>> GetMoviesByTheatreAsync(int theatreId)
        {
            return await _context.Movies
                .Include(m => m.Seats)   
                .Where(m => m.TheatreId == theatreId)
                .ToListAsync();
        }

        public async Task AddMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(int movieId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }

    }
}
