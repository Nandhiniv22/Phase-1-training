using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Context;
using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public class BookingRepo : IBookingRepo
    {
        private readonly AppDbContext _context;

        public BookingRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Movie)
                .Include(b => b.Theatre)
                .Include(b => b.Seats)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<List<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _context.Bookings
                .Include(b => b.Movie)
                .Include(b => b.Theatre)
                .Include(b => b.Seats)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingTime)
                .ToListAsync();
        }

        public async Task CancelBookingAsync(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByTheatreIdAsync(int theatreId)
        {
            return await _context.Bookings
                .Include(b => b.Movie)
                .ThenInclude(m => m.Theatre)
                .Include(b => b.Seats)
                .Where(b => b.Movie.TheatreId == theatreId)
                .ToListAsync();
        }

        public async Task<int> GetBookingCountForMovieAsync(int movieId)
        {
            return await _context.Bookings.CountAsync(b => b.MovieId == movieId);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<List<object>> GetBookingsByTheatreAsync(int theatreId)
        {
            return await _context.Bookings
                .Include(b => b.Movie)
                .Include(b => b.User)
                .Where(b => b.Movie.TheatreId == theatreId)
                .Select(b => new
                {
                    BookingId = b.BookingId,
                    UserName = b.User != null ? b.User.Name : "N/A",
                    MovieTitle = b.Movie != null ? b.Movie.Title : "N/A",
                    ShowDate = b.Movie != null ? b.Movie.ShowDate.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue,
                    ShowTime = b.Movie != null ? b.Movie.ShowTime.ToString(@"hh\:mm") : "N/A",
                    TotalPrice = b.TotalPrice,
                })
                .ToListAsync<object>();        }



    }
}
