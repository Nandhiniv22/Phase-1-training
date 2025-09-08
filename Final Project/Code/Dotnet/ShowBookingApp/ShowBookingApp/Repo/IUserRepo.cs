using ShowBookingApp.Models;

namespace ShowBookingApp.Repo
{
    public interface IUserRepo
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllOrganizersAsync();
        Task RegisterUserAsync(User user);
        Task ApproveOrganizerAsync(User user);
        Task RemoveOrganizerAsync(User user);
        Task<IEnumerable<User>> GetUnapprovedOrganizersAsync();
        Task<IEnumerable<User>> GetApprovedOrganizersAsync();
        Task BlockUserAsync(User user);
        Task UnblockUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
