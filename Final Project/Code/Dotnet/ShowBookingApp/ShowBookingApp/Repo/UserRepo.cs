using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Context;
using ShowBookingApp.Models;
using ShowBookingApp.Security;
using System.Text;

namespace ShowBookingApp.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


        // ✅ Get user by ID
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        // ✅ Get user by email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // ✅ Get all users (exclude admin role)
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.Role != Role.Admin)
                .ToListAsync();
        }

        // ✅ Get all organizers (exclude admins)
        public async Task<IEnumerable<User>> GetAllOrganizersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == Role.Organizer)
                .ToListAsync();
        }

        // ✅ Register new user
        public async Task RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // ✅ Approve organizer
        public async Task ApproveOrganizerAsync(User user)
        {
            user.IsApprovedOrganizer = true;
            user.Role = Role.Organizer;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveOrganizerAsync(User user)
        {
            user.IsApprovedOrganizer = false;
            user.Role = Role.User;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUnapprovedOrganizersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == Role.User && u.IsApprovedOrganizer == false)
                .ToListAsync();
        }

        // ✅ Get approved organizers
        public async Task<IEnumerable<User>> GetApprovedOrganizersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == Role.Organizer && u.IsApprovedOrganizer == true)
                .ToListAsync();
        }

        // ✅ Block user
        public async Task BlockUserAsync(User user)
        {
            user.IsBlocked = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        // ✅ Unblock user
        public async Task UnblockUserAsync(User user)
        {
            user.IsBlocked = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
