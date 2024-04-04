using Microsoft.AspNetCore.Mvc;

namespace Event_Registration.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Event model)
        {

            // TODO: transform formatted input into DateTime that is accepted by DB
            // model.Time = DateTime.Parse(model.Time.ToString("yyyy-MM-dd HH:mm:ss"));

            if (model.Time <= DateTime.Now)
            {
                ModelState.AddModelError("Time", "Ürituse aeg peab olema tulevikus.");
            }

            if (ModelState.IsValid)
            {
                _context.Events.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
