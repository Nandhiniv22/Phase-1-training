using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShowBookingApp.DTOs;
using ShowBookingApp.Models;
using ShowBookingApp.Repo;
using ShowBookingApp.Security;
using System.Security.Claims;

namespace ShowBookingApp.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly JWTOptions _jwtOptions;

        public AuthController(IUserRepo userRepo, IOptions<JWTOptions> jwtOptions)
        {
            _userRepo = userRepo;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var existingUser = await _userRepo.GetUserByEmailAsync(dto.Email);
            if (existingUser != null) return BadRequest("Email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = PasswordHelper.HashPassword(dto.Password),
                ContactNumber = dto.ContactNumber,
                Role = Role.User,
                IsApprovedOrganizer = false,
                IsBlocked = false
            };

            await _userRepo.RegisterUserAsync(user);
            return Ok(new { message = "Registration successful", userId = user.UserId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userRepo.GetUserByEmailAsync(dto.Email);
            if (user == null || !PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            if (user.IsBlocked) return Unauthorized("User is blocked");

            var claims = new List<Claim>
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = JwtService.CreateJWTToken(_jwtOptions, claims);

            var response = new AuthResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.Now.AddMinutes(_jwtOptions.ExpireMinutes),
                UserId = user.UserId,
                Name = user.Name,
                Role = user.Role.ToString(),
                IsApprovedOrganizer = user.IsApprovedOrganizer
            };

            return Ok(response);
        }
    }
}
