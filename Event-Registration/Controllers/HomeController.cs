using Event_Registration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Event_Registration.Controllers
{
    public class HomeController(ApplicationDbContext context) : Controller
    {

        private readonly ApplicationDbContext _context = context;

        public IActionResult Index()
        {
            var events = _context.Events
                .Select(e => new EventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Time = e.Time
                })
                .ToList();

            
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
            var nowInEstonia = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);

            var upcomingEvents = events.Where(e => e.Time >= nowInEstonia).OrderBy(e => e.Time).ToList();
            var pastEvents = events.Where(e => e.Time < nowInEstonia).OrderByDescending(e => e.Time).ToList();


            var model = new HomeViewModel
            {
                UpcomingEvents = upcomingEvents,
                PastEvents = pastEvents
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventToDelete = _context.Events.Include(e => e.Guests).FirstOrDefault(e => e.Id == id);
            if (eventToDelete == null)
            {
                return NotFound();
            }

            // I think you could purge all the guests automatically with some Entity Framework setup but oh well
            if (eventToDelete.Guests != null)
            {
                foreach (var guest in eventToDelete.Guests)
                {
                    _context.Guests.Remove(guest);
                }
            }
         
            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
 
    }
}
