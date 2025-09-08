using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShowBookingApp.Controllers;
using ShowBookingApp.Models;
using ShowBookingApp.Repo;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShowBookingApp.Tests
{
    [TestFixture]
    public class ControllerTests
    {
        private Mock<IUserRepo> _userRepoMock;
        private Mock<IOrganizerRepo> _organizerRepoMock;
        private Mock<ISeatRepo> _seatRepoMock;
        private Mock<IBookingRepo> _bookingRepoMock;
        private Mock<IMovieRepo> _movieRepoMock;
        private Mock<ITheatreRepo> _theatreRepoMock;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepo>();
            _organizerRepoMock = new Mock<IOrganizerRepo>();
            _seatRepoMock = new Mock<ISeatRepo>();
            _bookingRepoMock = new Mock<IBookingRepo>();
            _movieRepoMock = new Mock<IMovieRepo>();
            _theatreRepoMock = new Mock<ITheatreRepo>();
        }

        [Test]
        public async Task ApproveOrganizer_UserNotFound_ReturnsNotFound()
        {
            _userRepoMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);
            var controller = new AdminController(_userRepoMock.Object);

            var result = await controller.ApproveOrganizer(99);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task BlockUser_ValidUser_ReturnsOk()
        {
            var user = new User { UserId = 1, Name = "TestUser" };
            _userRepoMock.Setup(r => r.GetUserByIdAsync(1)).ReturnsAsync(user);

            var controller = new AdminController(_userRepoMock.Object);
            var result = await controller.BlockUser(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddMovie_ValidTheatre_ReturnsOk()
        {
            // Arrange
            var theatre = new Theatre { TheatreId = 1, OrganizerId = 1 };
            _organizerRepoMock.Setup(r => r.GetTheatreByIdAsync(1)).ReturnsAsync(theatre);
            _organizerRepoMock.Setup(r => r.AddMovieAsync(It.IsAny<Movie>()))
                              .Returns(Task.CompletedTask);

            var controller = new OrganizerController(_organizerRepoMock.Object, _seatRepoMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
        new Claim("UserId", "1"),
        new Claim(ClaimTypes.Role, "Organizer")
    }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var dto = new DTOs.CreateMovieDto
            {
                Title = "Test Movie",
                Language = "English",
                Description = "Sample description",
                DurationMinutes = 120,
                ScreenType = "2D",
                ShowDate = DateOnly.FromDateTime(DateTime.Today),
                ShowTime = new TimeSpan(18, 30, 0)
            };

            var result = await controller.AddMovie(1, dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value.ToString(), Does.Contain("Movie added successfully"));
        }


        [Test]
        public async Task SearchTheatres_NoResults_ReturnsNotFound()
        {
            _theatreRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Theatre>());

            var controller = new UserController(_theatreRepoMock.Object, _movieRepoMock.Object,
                _seatRepoMock.Object, _bookingRepoMock.Object, _userRepoMock.Object);

            var result = await controller.SearchTheatres("NonExistent", null, null, null);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}
