using Event_Registration.Controllers;
using Event_Registration.Models;
using Event_Registration.Tests.Setup;
using Microsoft.AspNetCore.Mvc;

namespace Event_Registration.Tests
{
    public class AttendeesControllerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public AttendeesControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AddIndividualAttendee_AddsAttendee_WhenModelStateIsValid()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new AttendeesController(dbContext);
            dbContext.Events.Add(new Event { Id = 1, Name = "Event 1", Time = DateTime.UtcNow.AddDays(1), Location = "Location 1", ExtraInformation = "ExtraInformation" });
            dbContext.SaveChanges();

            var newIndividual = new Individual
            {
                FirstName = "John",
                LastName = "Doe",
                PersonalIdCode = "39506036025",
                EventId = 1,
                ExtraInformation = "Extra Information"
            };

            var result = await controller.AddIndividualAttendee(newIndividual, 1) as RedirectToActionResult;

            Assert.Equal("Index", result.ActionName);
            Assert.Equal(1, dbContext.Individuals.Count());
            Assert.Contains(dbContext.Individuals, g => g.FirstName == "John");
        }

        [Fact]
        public async Task AddIndividualAttendee_Fails_WhenExtraInformationIsMissing()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new AttendeesController(dbContext);
            controller.ModelState.AddModelError("ExtraInformation", "Required");
            dbContext.Events.Add(new Event { Id = 2, Name = "Event 2", Time = DateTime.UtcNow.AddDays(1), Location = "Location 1", ExtraInformation = "ExtraInformation" });
            dbContext.SaveChanges();

            var newIndividual = new Individual
            {
                FirstName = "John",
                LastName = "Doe",
                PersonalIdCode = "39506036025",
                EventId = 2
            };

            var result = await controller.AddIndividualAttendee(newIndividual, 2) as ViewResult;

            Assert.False(controller.ModelState.IsValid);
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void EditAttendee_ReturnsViewForExistingIndividual()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new AttendeesController(dbContext);
            var individual = new Individual { Id = 4, FirstName = "John", LastName = "Doe", EventId = 4, ExtraInformation = "Extra Info", PersonalIdCode = "39506036025" };
            dbContext.Individuals.Add(individual);
            dbContext.SaveChanges();

            var result = controller.EditAttendee(4) as ViewResult;

            Assert.NotNull(result);
            var model = result.Model as AttendeeDetailsViewModel;
            Assert.Equal("Individual", model.AttendeeType);
            Assert.Equal(individual.FirstName, model.Individual.FirstName);
        }

        [Fact]
        public void EditAttendee_ReturnsNotFoundForNonExistentGuest()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new AttendeesController(dbContext);

            var result = controller.EditAttendee(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditBusinessAttendee_UpdatesSuccessfully_WhenModelStateIsValid()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new AttendeesController(dbContext);
            var business = new Business { Id = 1, LegalName = "Acme Corp", EventId = 1, ExtraInformation = "Extra Info", RegistrationCode = "39599999" };
            dbContext.Businesses.Add(business);
            dbContext.SaveChanges();

            business.LegalName = "Updated Acme Corp";
            var result = await controller.EditBusinessAttendee(business) as RedirectToActionResult;

            Assert.Equal("EventDetails", result.ActionName);
            var updatedBusiness = dbContext.Businesses.FirstOrDefault(b => b.Id == 1);
            Assert.Equal("Updated Acme Corp", updatedBusiness.LegalName);
        }

        [Fact]
        public async Task EditBusinessAttendee_ReturnsNotFoundForNonExistentBusiness()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new AttendeesController(dbContext);
            var business = new Business { Id = 999, LegalName = "Nonexistent Corp", EventId = 1 };

            var result = await controller.EditBusinessAttendee(business) as NotFoundResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteAttendee_DeletesAttendeeSuccessfully()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new AttendeesController(dbContext);
            var individual = new Individual { Id = 3, EventId = 3, FirstName = "John", LastName = "Doe", ExtraInformation = "Extra Info", PersonalIdCode = "39506036025"};
            dbContext.Individuals.Add(individual);
            dbContext.SaveChanges();

            var result = await controller.DeleteAttendee(3, 3) as RedirectToActionResult;

            Assert.Equal("EventDetails", result.ActionName);
            Assert.False(dbContext.Individuals.Any(i => i.Id == 3));
        }

        [Fact]
        public async Task DeleteAttendee_ReturnsNotFoundForNonExistentAttendee()
        {
            using var dbContext = _fixture.CreateContext();
            var controller = new AttendeesController(dbContext);

            var result = await controller.DeleteAttendee(999, 1) as NotFoundResult;

            Assert.NotNull(result);
        }

    }
}
