namespace ShowBookingApp.DTOs
{
    public class ProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; // read-only
        public string Role { get; set; } = string.Empty;  // added role
    }

    public class UpdateProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Password { get; set; } // optional
    }
}
