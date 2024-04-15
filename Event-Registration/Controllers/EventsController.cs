using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Event_Registration.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_Registration.Controllers
{
    public class EventsController(ApplicationDbContext context) : Controller
    {
        private readonly ApplicationDbContext _context = context;

        public IActionResult AddEvent()
        {
            return View();
        }

        public IActionResult EventDetails(int id)
        {
            var eventItem = _context.Events.Include(e => e.Guests).FirstOrDefault(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            var model = new AttendeesViewModel
            {
                EventId = eventItem.Id,
                Name = eventItem.Name,
                Time = eventItem.Time,
                Location = eventItem.Location,
                Guests = eventItem.Guests.ToList()
            };


            return View(model);
        }

        [HttpPost]
        public IActionResult AddEvent(EventViewModel viewModel)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
            var nowInEstonia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            if (viewModel.Time <= nowInEstonia)
            {
                ModelState.AddModelError("Time", "Ürituse aeg peab olema tulevikus.");
            }


            if (ModelState.IsValid)
            {
                var eventToAdd = new Event
                {
                    Name = viewModel.Name,
                    Time = DateTime.SpecifyKind(viewModel.Time, DateTimeKind.Utc),
                    Location = viewModel.Location,
                    ExtraInformation = viewModel.ExtraInformation
                };

                _context.Events.Add(eventToAdd);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }
    }
}