using Event_Registration.Controllers;
using Event_Registration.Models;
using Event_Registration.Tests.Setup;
using Microsoft.AspNetCore.Mvc;

namespace Event_Registration.Tests
{
    public class EventsControllerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public EventsControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AddEvent_AddsEventAndRedirects_WhenModelStateIsValid()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new EventsController(dbContext);
            var newEvent = new EventViewModel
            {
                Id = 1,
                Name = "New Event",
                Time = DateTime.UtcNow.AddDays(10),
                Location = "New Location",
                ExtraInformation = "Extra Information"
            };

            var result = controller.AddEvent(newEvent) as RedirectToActionResult;

            Assert.Equal("Index", result.ActionName);
            Assert.Contains(dbContext.Events, e => e.Name == "New Event");
        }

        [Fact]
        public void EventDetails_ReturnsViewForExistingEvent()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new EventsController(dbContext);
            var newEvent = new Event { Id = 1, Name = "Event 1", Time = DateTime.UtcNow.AddDays(1), Location = "Location 1", ExtraInformation = "Info" };
            dbContext.Events.Add(newEvent);
            dbContext.SaveChanges();

            var result = controller.EventDetails(1) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as AttendeesViewModel;
            Assert.Equal("Event 1", model.Name);
        }

        [Fact]
        public void EventDetails_ReturnsNotFoundForNonexistentEvent()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new EventsController(dbContext);

            var result = controller.EventDetails(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void AddEvent_FailsWhenEventIsInThePast()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new EventsController(dbContext);
            var pastEvent = new EventViewModel
            {
                Name = "Past Event",
                Time = DateTime.UtcNow.AddDays(-1), // Past time
                Location = "Old Location",
                ExtraInformation = "Info"
            };

            var result = controller.AddEvent(pastEvent) as ViewResult;

            Assert.False(controller.ModelState.IsValid);
            Assert.Equal("Ürituse aeg peab olema tulevikus.", controller.ModelState["Time"].Errors[0].ErrorMessage);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddEvent_ReturnsViewWhenModelStateIsInvalid()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new EventsController(dbContext);
            controller.ModelState.AddModelError("Name", "Required");

            var newEvent = new EventViewModel
            {
                Time = DateTime.UtcNow.AddDays(10), // Valid time
                Location = "New Location",
                ExtraInformation = "Info"
            };

            var result = controller.AddEvent(newEvent) as ViewResult;

            Assert.False(controller.ModelState.IsValid);
            Assert.IsType<ViewResult>(result);
        }
    }
}
