using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestGp.Models;

namespace TestGp.Controllers
{
    public class EventsController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailService _emailService;
        private Mydb _context;
        private readonly IWebHostEnvironment _env;

        public EventsController(Mydb context, EmailService emailService, IWebHostEnvironment env)
        {
            _context = context;
            _emailService = emailService;
            _env = env;
        }

        private readonly ILogger<EventsController> _logger;

        public IActionResult OrganizerRegister()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrganizerRegister(OrganizerRegister or)
        {
            var orgnz = from org in _context.organizerRegisters
                        where org.Email.Equals(or.Email)
                        select org;
            if (orgnz.Count() >= 1)
            {
                TempData["Error message"] = " the email or username are already exist";
                return View();
            }
            else
            {
                or.ID = GenerateRandomId();
                _context.organizerRegisters.Add(or);
                _context.SaveChanges();
                ModelState.Clear();
                var sortedRegisters = _context.organizerRegisters.OrderBy(r => r.ID).ToList();
                TempData["Msgg"] = "Inserted Successfully";

                return RedirectToAction("Login2", "Home");
            }
        }

        public IActionResult CreateEvent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateEvent(Event e, IFormFile photo)
        {
            var info = from inf in _context.Events
                       select inf;
            if (photo != null)
            {
                List<Event> EventsList = info.ToList();
                // Save photo to file system
                //string filename = Guid.NewGuid().ToString() + Path.GetExtension(e.Event_Image);
                string fileName = photo.FileName.Split('\\').LastOrDefault().Split('/').LastOrDefault();

                string path = Path.Combine(_env.WebRootPath, "images/", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                // Update model with photo path

                string pathDB = "~/Images/" + fileName;
                e.Event_Image = pathDB;

                e.ID = GenerateRandomId();
                e.UserId = (int)HttpContext.Session.GetInt32("o4");
                e.Organizer = HttpContext.Session.GetString("s3");
                _context.Events.Add(e);

                _context.SaveChanges();

                TempData["Msgg1212"] = "Inserted Successfully";
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public IActionResult EditEvent()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditEvent(Event e)
        {
            var updatedEvent = _context.Events.Find(e.ID);

            if (updatedEvent.EventTitle == null || updatedEvent.Tickects == 0 || updatedEvent.Tags == null)
            {
                return View();
            }
            else
            {
                // Assuming 'e' is the existing event fetched from the database
                var existingEvent = _context.Events.Find(updatedEvent.ID);

                if (existingEvent == null)
                {
                    // Handle the case where the event with the specified ID is not found
                    return NotFound();
                }

                // Update only non-null values
                if (!updatedEvent.EventTitle.IsNullOrEmpty())
                {
                    existingEvent.EventTitle = updatedEvent.EventTitle;
                }

                if (updatedEvent.Tickects != 0)
                {
                    existingEvent.Tickects = updatedEvent.Tickects;
                }

                if (updatedEvent.Tags != null)
                {
                    existingEvent.Tags = updatedEvent.Tags;
                }

                _context.Events.Update(existingEvent);
                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
        }

        [HttpPost]
        public IActionResult Orgeditprof(OrganizerRegister or)
        {
            // Find the organizer information by ID
            var updatedOrinfo = _context.organizerRegisters.Find(or.ID);

            // Check if the organizer information is found
            if (updatedOrinfo == null)
            {
                // Handle the case where the organizer information with the specified ID is not found
                return NotFound();
            }

            // Check if required fields are not null or 0
            if (updatedOrinfo.Ornganization_Name == null || updatedOrinfo.Organization_PhoneNumber == 0 || updatedOrinfo.Email == null || updatedOrinfo.Password == null)
            {
                return View();
            }

            // Assuming 'existing' is the existing organizer information fetched from the database
            var existing = _context.organizerRegisters.Find(updatedOrinfo.ID);

            // Check if the organizer information is found (double-check for safety)
            if (existing == null)
            {
                // Handle the case where the organizer information with the specified ID is not found
                return NotFound();
            }

            // Update only non-null values
            if (updatedOrinfo.Ornganization_Name != null)
            {
                existing.Ornganization_Name = updatedOrinfo.Ornganization_Name;
            }

            if (updatedOrinfo.Organization_PhoneNumber != 0)
            {
                existing.Organization_PhoneNumber = updatedOrinfo.Organization_PhoneNumber;
            }

            if (updatedOrinfo.Email != null)
            {
                existing.Email = updatedOrinfo.Email;
            }

            if (updatedOrinfo.Password != null)
            {
                existing.Password = updatedOrinfo.Password;
            }

            _context.organizerRegisters.Update(existing);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");

            /*var updatedOrinfo = _context.organizerRegisters.Find(or.ID);

            if (updatedOrinfo.Ornganization_Name == null || updatedOrinfo.Organization_PhoneNumber == 0 || updatedOrinfo.Email == null || updatedOrinfo.Password == null)
            {
                return View();
            }
            else
            {
                // Assuming 'e' is the existing event fetched from the database
                var existing = _context.organizerRegisters.Find(updatedOrinfo.ID);

                if (existing == null)
                {
                    // Handle the case where the event with the specified ID is not found
                    return NotFound();
                }

                // Update only non-null values
                if (updatedOrinfo.Ornganization_Name == null)
                {
                    existing.Ornganization_Name = updatedOrinfo.Ornganization_Name;
                }

                if (updatedOrinfo.Organization_PhoneNumber != 0)
                {
                    existing.Organization_PhoneNumber = updatedOrinfo.Organization_PhoneNumber;
                }

                if (updatedOrinfo.Email != null)
                {
                    existing.Email = updatedOrinfo.Email;
                }

                if (updatedOrinfo.Password != null)
                {
                    existing.Password = updatedOrinfo.Password;
                }

                _context.organizerRegisters.Update(existing);
                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
*/
        }

        [HttpPost]
        public IActionResult DeleteEvent(Event e)
        {
            _context.Events.Remove(e);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard(Login L, Event e, OrganizerRegister or)
        {
            var userId = HttpContext.Session.GetInt32("o4");

            var userEvents = _context.Events

                                .Where(ev => e.UserId == or.ID)
                                .ToList();

            return View(userEvents);
        }

        private int GenerateRandomId()
        {
            var random = new Random();
            return random.Next(1, 9999); // Adjust the range as needed
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}