using Microsoft.AspNetCore.Mvc;

namespace Event_Registration.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Add()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult Add(Event model)
        {
            if (ModelState.IsValid)
            {
                // TODO: Add code for adding event to database
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
