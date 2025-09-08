using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public interface IMovieRepo
    {
        Task<IEnumerable<Movie>> GetAllAsync();
        Task<IEnumerable<Movie>> GetMoviesByTheatreAsync(int theatreId);
        Task<Movie?> GetMovieByIdAsync(int movieId);
        Task AddMovieAsync(Movie movie);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(int movieId);

    }
}
