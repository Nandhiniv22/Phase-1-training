using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public interface ITheatreRepo
    {
        Task<IEnumerable<Theatre>> GetTheatresByOrganizerAsync(int organizerId);
        Task AddTheatreAsync(Theatre theatre);
        Task<Theatre?> GetTheatreByIdAsync(int theatreId);
        Task<IEnumerable<Theatre>> GetAllAsync();
        Task<IEnumerable<Seat>> GetSeatsByTheatreAsync(int theatreId);
    }
}
