using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public interface IOrganizerRepo
    {
        Task<IEnumerable<Theatre>> GetTheatresByOrganizerAsync(int organizerId);
        Task AddTheatreAsync(Theatre theatre);
        Task<Theatre?> GetTheatreByIdAsync(int theatreId);

        Task AddMovieAsync(Movie movie);
        Task<Movie?> GetMovieByIdAsync(int movieId);
        Task<IEnumerable<Movie>> GetMoviesByTheatreAsync(int theatreId);

        Task AddSeatsAsync(List<Seat> seats);
        Task<IEnumerable<Seat>> GetSeatsByTheatreAsync(int theatreId);
        Task<IEnumerable<Seat>> GetSeatsByIdsAsync(List<int> seatIds);

        Task DeleteMovieAsync(int movieId);
        Task UpdateMovieAsync(Movie movie);

    }
}
