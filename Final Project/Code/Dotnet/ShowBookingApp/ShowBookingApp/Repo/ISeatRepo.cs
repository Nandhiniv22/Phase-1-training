using ShowBookingApp.DTOs;
using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public interface ISeatRepo
    {
        Task<IEnumerable<Seat>> GetSeatsByTheatreAsync(int theatreId);
        Task<Seat?> GetSeatByIdAsync(int seatId);
        Task AddSeatsAsync(int theatreId, int movieId, List<Seat> seats);
        Task<List<Seat>> GetSeatsByMovieAsync(int movieId, int theatreId);
        Task UpdateSeatsAsync(List<Seat> seats);
        Task<List<Seat>> GetSeatsByIdsAsync(List<int> seatIds);
    }
}
