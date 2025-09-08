using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Context;
using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public class TheatreRepo : ITheatreRepo
    {
        private readonly AppDbContext _context;
        public TheatreRepo(AppDbContext context) => _context = context;

        public async Task<Theatre?> GetTheatreByIdAsync(int theatreId) =>
            await _context.Theatres.FindAsync(theatreId);

        public async Task<IEnumerable<Theatre>> GetTheatresByOrganizerAsync(int organizerId) =>
            await _context.Theatres.Where(t => t.OrganizerId == organizerId).ToListAsync();

        public async Task AddTheatreAsync(Theatre theatre)
        {
            _context.Theatres.Add(theatre);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Theatre>> GetAllAsync()
        {
            return await _context.Theatres
                .Include(t => t.Movies)
                .Include(t => t.Seats)
                .ToListAsync();
        }

        public async Task<IEnumerable<Seat>> GetSeatsByTheatreAsync(int theatreId) // NEW
        {
            return await _context.Seats
                .Where(s => s.TheatreId == theatreId)
                .ToListAsync();
        }
    }
}
