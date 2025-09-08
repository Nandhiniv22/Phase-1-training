using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Context;
using ShowBookingApp.DTOs;
using ShowBookingApp.Models;
using ShowBookingApp.Security;
using System.Text;

namespace ShowBookingApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Authorize] // any logged-in user (Admin, Organizer, User)
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET: api/auth/profile
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found");

            var dto = new ProfileDto
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(dto);
        }

        // ✅ PUT: api/auth/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null)
                return NotFound("User not found");

            // update name
            user.Name = dto.Name;

            // update password only if provided
            if (!string.IsNullOrEmpty(dto.Password))
            {
                user.PasswordHash = PasswordHelper.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }
    }
}
