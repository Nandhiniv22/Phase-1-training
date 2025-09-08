using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShowBookingApp.Mappers;
using ShowBookingApp.Models;
using ShowBookingApp.Repo;

namespace ShowBookingApp.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public AdminController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return Ok(users.Select(u => u.ToDto()));
        }

        [HttpGet("organizers")]
        public async Task<IActionResult> GetOrganizers()
        {
            var allUsers = await _userRepo.GetAllUsersAsync();

            var unapproved = allUsers.Where(u => u.Role == Role.OrganizerPending).ToList();
            var approved = allUsers.Where(u => u.Role == Role.Organizer).ToList();

            return Ok(new
            {
                Unapproved = unapproved.Select(u => new { u.UserId, u.Name, u.Email }),
                Approved = approved.Select(u => new { u.UserId, u.Name, u.Email })
            });
        }

        [HttpPost("approve-organizer/{userId}")]
        public async Task<IActionResult> ApproveOrganizer(int userId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            await _userRepo.ApproveOrganizerAsync(user);
            return Ok(new { message = "User approved as organizer" });
        }

        [HttpPost("remove-organizer/{userId}")]
        public async Task<IActionResult> RemoveOrganizer(int userId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            await _userRepo.RemoveOrganizerAsync(user);
            return Ok(new { message = "Organizer removed successfully" });
        }

        [HttpGet("approved-organizers")]
        public async Task<IActionResult> GetApprovedOrganizers()
        {
            var organizers = await _userRepo.GetApprovedOrganizersAsync();
            return Ok(organizers.Select(u => u.ToDto()));
        }

        [HttpGet("unapproved-organizers")]
        public async Task<IActionResult> GetUnapprovedOrganizers()
        {
            var organizers = await _userRepo.GetUnapprovedOrganizersAsync();
            return Ok(organizers.Select(u => u.ToDto()));
        }


        [HttpPost("block/{userId}")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            await _userRepo.BlockUserAsync(user);
            return Ok(new { message = "User blocked successfully" });
        }

        [HttpPost("unblock/{userId}")]
        public async Task<IActionResult> UnblockUser(int userId)
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            await _userRepo.UnblockUserAsync(user);
            return Ok(new { message = "User unblocked successfully" });
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var users = await _userRepo.GetAllUsersAsync();
            var organizers = await _userRepo.GetAllOrganizersAsync();

            var stats = new
            {
                totalUsers = users.Count(),
                approvedOrganizers = organizers.Count(o => o.IsApprovedOrganizer),
                pendingOrganizers = users.Count(u => u.Role == Role.OrganizerPending && !u.IsApprovedOrganizer),
                blockedUsers = users.Count(u => u.IsBlocked)
            };

            return Ok(stats);
        }

    }
}
