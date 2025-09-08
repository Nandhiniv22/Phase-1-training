using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public interface IBookingRepo
    {
        Task<Booking> AddBookingAsync(Booking booking);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<List<Booking>> GetBookingsByUserIdAsync(int userId);
        Task UpdateBookingAsync(Booking booking);
        Task CancelBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetBookingsByTheatreIdAsync(int theatreId);
        Task<int> GetBookingCountForMovieAsync(int movieId);

    }
}
