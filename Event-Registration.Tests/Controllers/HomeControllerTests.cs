using Event_Registration.Controllers;
using Event_Registration.Models;
using Event_Registration.Tests.Setup;
using Microsoft.AspNetCore.Mvc;

namespace Event_Registration.Tests
{
    public class HomeControllerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public HomeControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Index_ReturnsCorrectEvents()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new HomeController(dbContext);
            dbContext.Events.AddRange(new Event
            {
                Id = 1,
                Name = "Event 1",
                Time = DateTime.UtcNow.AddDays(1), // future event
                Location = "Location 1",
                ExtraInformation = "Extra info"
            }, new Event
            {
                Id = 2,
                Name = "Event 2",
                Time = DateTime.UtcNow.AddDays(-1), // past event
                Location = "Location 2",
                ExtraInformation = "Extra info"
            });
            dbContext.SaveChanges();

            var result = controller.Index() as ViewResult;
            var model = result.Model as HomeViewModel;

            Assert.Single(model.UpcomingEvents);
            Assert.Single(model.PastEvents);
        }

        [Fact]
        public void Index_WithNoEvents_ReturnsEmptyLists()
        {
            using var dbContext = _fixture.CreateContext();

            dbContext.Events.RemoveRange(dbContext.Events);
            dbContext.Guests.RemoveRange(dbContext.Guests);
            dbContext.SaveChanges();

            var controller = new HomeController(dbContext);

            var result = controller.Index() as ViewResult;
            var model = result.Model as HomeViewModel;

            Assert.Empty(model.UpcomingEvents);
            Assert.Empty(model.PastEvents);
        }

        [Fact]
        public async Task DeleteEvent_SuccessfullyDeletesEventAndItsGuests()
        {
            using var dbContext = _fixture.CreateContext();

            var controller = new HomeController(dbContext);
            var testEvent = new Event
            {
                Id = 3,
                Name = "Test Event",
                Time = DateTime.UtcNow.AddDays(1),
                Location = "Test Location",
                ExtraInformation = "Extra info"
            };
            testEvent.Guests = new List<Guest>
            {
                new Individual { Id = 1, EventId = 3, FirstName = "John", LastName = "Doe", ExtraInformation = "Extra Info", PersonalIdCode = "39506036025" },
                new Business { Id = 2, EventId = 3, LegalName = "LoremIpsum", ExtraInformation = "Extra Info", RegistrationCode = "3950603" }
            };
            dbContext.Events.Add(testEvent);
            dbContext.SaveChanges();

            var result = await controller.DeleteEvent(3) as RedirectToActionResult;

            Assert.Equal("Index", result.ActionName);
            Assert.False(dbContext.Events.Any(e => e.Id == 3));
            Assert.False(dbContext.Guests.Any(g => g.EventId == 3));
        }

        [Fact]
        public async Task DeleteEvent_ReturnsNotFoundForNonexistentEvent()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new HomeController(dbContext);

            var result = await controller.DeleteEvent(666) as NotFoundResult;

            Assert.NotNull(result);
        }
    }
}
