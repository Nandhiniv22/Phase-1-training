using Microsoft.AspNetCore.Mvc;
using ShowBookingApp.Repo;

namespace ShowBookingApp.Controllers
{
    [ApiController]
    [Route("api/theatre")]
    public class TheatreController : ControllerBase
    {
        private readonly ITheatreRepo _repo;
        public TheatreController(ITheatreRepo repo)
        {
            _repo = repo;
        }

        [HttpGet("{theatreId}/seats")] 
        public async Task<IActionResult> GetSeats(int theatreId)
        {
            var seats = await _repo.GetSeatsByTheatreAsync(theatreId);
            return Ok(seats);
        }
    }
}
