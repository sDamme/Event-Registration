using Event_Registration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Event_Registration.Controllers
{
    public class AttendeesController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult EditAttendee(int guestId)
        {
            // Determine if guest is an Individual or Business and fetch details
            var guest = _context.Guests.Find(guestId);
            if (guest is Individual individual)
            {
                var model = new AttendeeDetailsViewModel
                {
                    GuestId = individual.Id,
                    EventId = (int)individual.EventId,
                    AttendeeType = "Individual",
                    Individual = individual
                };

                return View("~/Views/Events/AttendeeDetails.cshtml", model);
            }
            else if (guest is Business business)
            {

                var model = new AttendeeDetailsViewModel
                {
                    GuestId = business.Id,
                    EventId = (int)business.EventId,
                    AttendeeType = "Business",
                    Business = business
                };

                return View("~/Views/Events/AttendeeDetails.cshtml", model);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> AddIndividualAttendee(Individual model, int eventId)
        {
            if (!ModelState.IsValid)
            {
                // Prepare the data needed for the view
                var eventItem = _context.Events.Include(e => e.Guests).FirstOrDefault(e => e.Id == eventId);
                if (eventItem == null)
                {
                    return NotFound();
                }

                var viewModel = new AttendeesViewModel
                {
                    EventId = eventItem.Id,
                    Name = eventItem.Name,
                    Time = eventItem.Time,
                    Location = eventItem.Location,
                    Guests = eventItem.Guests.ToList(),
                    AttendeeType = "Individual" // Mark that the individual form was submitted
                };

                // Pass the individual model to the view to keep form data
                ViewBag.IndividualModel = model;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View("~/Views/Events/Attendees.cshtml", viewModel);
            }

            // Create and save the Individual entity
            var individual = new Individual
            {
                EventId = eventId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PersonalIdCode = model.PersonalIdCode,
                PaymentType = model.PaymentType,
                ExtraInformation = model.ExtraInformation
            };
            _context.Add(individual);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> AddBusinessAttendee(Business model, int eventId)
        {
            if (!ModelState.IsValid)
            {
                // Return the form with validation messages
                return View("~/Views/Events/Attendees.cshtml", model);
            }

            // Create and save the Business entity
            var business = new Business
            {
                EventId = eventId,
                LegalName = model.LegalName,
                RegistrationCode = model.RegistrationCode,
                NumberOfAttendees = model.NumberOfAttendees,
                PaymentType = model.PaymentType,
                ExtraInformation = model.ExtraInformation
                // Set other properties
            };
            _context.Add(business);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


        

        [HttpPost]
        public async Task<IActionResult> EditIndividualAttendee(Individual model)
        {
            if (!ModelState.IsValid)
            {
                var eventItem = _context.Events.Include(e => e.Guests).FirstOrDefault(e => e.Id == model.EventId);
                if (eventItem == null)
                {
                    return NotFound();
                }

                var viewModel = new AttendeeDetailsViewModel
                {
                    GuestId = model.Id,
                    EventId = (int)model.EventId,
                    AttendeeType = "Individual",
                    Individual = model
                };

                // Pass the individual model to the view to keep form data
                ViewBag.IndividualModel = model;
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return View("~/Views/Events/AttendeeDetails.cshtml", viewModel);
            }

            var individual = _context.Individuals.Find(model.Id);
            if (individual != null)
            {
                individual.FirstName = model.FirstName;
                individual.LastName = model.LastName;
                individual.PersonalIdCode = model.PersonalIdCode;
                individual.PaymentType = model.PaymentType;
                individual.ExtraInformation = model.ExtraInformation;

                _context.Update(individual);
                await _context.SaveChangesAsync();

                return RedirectToAction("EventDetails", "Events", new { id = model.EventId });
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditBusinessAttendee(Business model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Events/AttendeeDetails.cshtml", model);
            }

            var business = _context.Businesses.Find(model.Id);
            if (business != null)
            {
                business.LegalName = model.LegalName;
                business.RegistrationCode = model.RegistrationCode;
                business.NumberOfAttendees = model.NumberOfAttendees;
                business.PaymentType = model.PaymentType;
                business.ExtraInformation = model.ExtraInformation;

                _context.Update(business);
                await _context.SaveChangesAsync();

                return RedirectToAction("EventDetails", "Events", new { id = model.EventId });
            }

            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> DeleteAttendee(int guestId, int eventId)
        {
            var guest = _context.Guests.FirstOrDefault(g => g.Id == guestId && g.EventId == eventId);
            if (guest == null)
            {
                return NotFound();
            }

            _context.Guests.Remove(guest);
            await _context.SaveChangesAsync();
            return RedirectToAction("EventDetails", "Events", new { id = eventId });
        }
    }
}
