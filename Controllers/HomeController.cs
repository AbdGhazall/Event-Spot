using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestGp.Models;

namespace TestGp.Controllers
{
    public class HomeController : Controller
    {
        private Mydb _context;
        private readonly EmailService _emailService;

        public HomeController(Mydb context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        private readonly ILogger<HomeController> _logger;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Register R)
        {
            var guest = from gs in _context.Registers
                        where gs.Email.Equals(R.Email) || gs.Username.Equals(R.Username)
                        select gs;
            if (guest.Count() >= 1)
            {
                TempData["Error message"] = " the email or username are already exist";
                return View();
            }
            else
            {
                R.ID = GenerateRandomId();
                _context.Registers.Add(R);
                _context.SaveChanges();
                ModelState.Clear();
                var sortedRegisters = _context.Registers.OrderBy(r => r.ID).ToList();
                TempData["Msgg"] = "Inserted Successfully";
                return RedirectToAction("Login2");
            }
        }

        [HttpGet]
        public IActionResult Login2()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login2(Login L)
        {
            var guest = from gs in _context.Registers
                        where gs.Email.Equals(L.Email) && gs.Password.Equals(L.Password)
                        select gs;
            var orgnz = from org in _context.organizerRegisters
                        where org.Email.Equals(L.Email) && org.Password.Equals(L.Password)
                        select org;

            if (guest.Count() >= 1)
            {
                Models.Register gst = guest.FirstOrDefault();
                string Email = gst.Email;
                HttpContext.Session.SetString("s1", Email);

                Models.Register gstt = guest.FirstOrDefault();
                string Pass = gst.Password;
                HttpContext.Session.SetString("s2", Pass);

                Models.Register gstttt = guest.FirstOrDefault();
                string username = gst.Username;
                HttpContext.Session.SetString("s3", username);

                Models.Register fn = guest.FirstOrDefault();
                string fnn = gst.First_Name;
                HttpContext.Session.SetString("s4", fnn);

                Models.Register fnt = guest.FirstOrDefault();
                string lastname = gst.Last_Name;
                HttpContext.Session.SetString("s5", lastname);

                Models.Register fntt = guest.FirstOrDefault();
                int ID = gst.ID;
                HttpContext.Session.SetInt32("s6", ID);

                Models.Register fntt111 = guest.FirstOrDefault();
                int PNUM = gst.phoneNumber;
                HttpContext.Session.SetInt32("s7", ID);

                TempData["Msg"] = "LogedIn ";
                return RedirectToAction("Index");
            }

            if (orgnz.Count() >= 1)
            {
                Models.OrganizerRegister orgnzz = orgnz.FirstOrDefault();
                string Emaill = orgnzz.Ornganization_Name;
                HttpContext.Session.SetString("o1", Emaill);
                Models.OrganizerRegister orgnzzz = orgnz.FirstOrDefault();
                string Passs = orgnzz.Password;
                HttpContext.Session.SetString("o2", Passs);
                Models.OrganizerRegister orgnzzzz = orgnz.FirstOrDefault();
                string organizername = orgnzz.Ornganization_Name;
                HttpContext.Session.SetString("s3", organizername);

                Models.OrganizerRegister orgnzzzz1 = orgnz.FirstOrDefault();
                int organizerID = orgnzz.ID;
                HttpContext.Session.SetInt32("o4", organizerID);

                TempData["Msg"] = "welcome ";
                return RedirectToAction("Dashboard", "Events");
            }

            if (guest.Count() == 0 || orgnz.Count() == 0)
            {
                TempData["Msg"] = "Email or Password are invalid ";
                return View();
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.Keys.Any())
            {
                HttpContext.Session.Clear();
                TempData["log out"] = " logout done";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["log outt"] = "logout feild";
            }
            return RedirectToAction("Index");
        }

        public IActionResult Booking()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Booking(Booking b, Event e, int tkt, int numtkt, Register r)
        {
            var info = from inf in _context.Events
                       select inf;
            var data = from ee in _context.Bookings
                       select ee;
            var title = from titl in _context.Events
                        where e.UserId.Equals(r.ID) && e.DateAndTime.Equals(r.ID)
                        select titl;
            var titlee = from titl in _context.Events
                         where e.DateAndTime.Equals(r.ID)
                         select titl;
            if (HttpContext.Session.Keys.Any())
            {
                if (info.Count() >= 1)
                {
                    var @event = _context.Events.Find(e.ID);

                    string userEmail = HttpContext.Session.GetString("s1");
                    ViewData["user"] = userEmail;
                    if (@event != null)
                    {
                        var date = e.DateAndTime;
                        if (@event.Tickects >= b.NumberOfTickets)
                        {
                            b.BookingID = GenerateRandomId();
                            b.UserId = (int)HttpContext.Session.GetInt32("s6");
                            b.EmailAdress = HttpContext.Session.GetString("s1");
                            b.First_Name = HttpContext.Session.GetString("s4");
                            b.Last_Name = HttpContext.Session.GetString("s5");
                            b.PhoneNumber = (int)HttpContext.Session.GetInt32("s7");
                            b.EventDate = @event.DateAndTime;
                            b.Event_Title = @event.EventTitle;

                            _context.Bookings.Add(b);
                            _context.SaveChanges();
                            ModelState.Clear();
                            @event.Tickects -= b.NumberOfTickets;
                            _context.Events.Update(@event);
                            _context.SaveChanges();
                            string username = HttpContext.Session.GetString("s3");
                            int BookingID = b.BookingID;
                            int Tnum = b.NumberOfTickets;
                            _emailService.SendBookingConfirmationEmail(userEmail, r, username, b, BookingID, Tnum);
                            TempData["Msgg"] = "Inserted Successfully";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Not enough available tickets.";
                            return RedirectToAction("Booking", "Home");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Not enough available tickets.";
                        return RedirectToAction("Booking", "Home");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Event or Booking not found.";
                    return RedirectToAction("Login2", "Home");
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult EditProfile()

        {
            return View();
        }

        [HttpPost]
        public IActionResult EditProfile(Register r)
        {
            var updatedGinfo = _context.Registers.Find(r.ID);

            if (updatedGinfo.Username == null || updatedGinfo.First_Name == null || updatedGinfo.phoneNumber == 0 || updatedGinfo.Last_Name == null || updatedGinfo.Password == null || updatedGinfo.Email == null || updatedGinfo.Birth_Date == null)
            {
                return View();
            }
            else
            {
                // Assuming 'e' is the existing event fetched from the database
                var existing = _context.Registers.Find(updatedGinfo.ID);

                if (existing == null)
                {
                    // Handle the case where the event with the specified ID is not found
                    return NotFound();
                }

                // Update only non-null values
                if (updatedGinfo.Username != null)
                {
                    updatedGinfo.Username = updatedGinfo.Username;
                }

                if (updatedGinfo.First_Name != null)
                {
                    updatedGinfo.First_Name = updatedGinfo.First_Name;
                }

                if (updatedGinfo.phoneNumber != 0)
                {
                    updatedGinfo.phoneNumber = updatedGinfo.phoneNumber;
                }

                if (updatedGinfo.Last_Name != null)
                {
                    updatedGinfo.Last_Name = updatedGinfo.Last_Name;
                }

                if (updatedGinfo.Password != null)
                {
                    updatedGinfo.Password = updatedGinfo.Password;
                }
                if (updatedGinfo.Email != null)
                {
                    updatedGinfo.Email = updatedGinfo.Email;
                }
                if (updatedGinfo.Birth_Date != null)
                {
                    updatedGinfo.Birth_Date = updatedGinfo.Birth_Date;
                }
                _context.Registers.Update(existing);
                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            return View();
        }

        public IActionResult Tickets()
        {
            return View();
        }

        public IActionResult BookingsList(Booking b, Register r)
        {
            var userId = HttpContext.Session.GetInt32("s6");
            if (userId != null)
            {
                var userBookings = _context.Bookings

                                    .Where(b => b.UserId == userId)
                                    .ToList();

                return View(userBookings);
            }
            return View();
        }

        public IActionResult Orgnizerpage()
        {
            return View();
        }

        public IActionResult Experience()
        {
            var data = from ee in _context.Events
                       select ee;
            var tripEvents = _context.Events.Where(e => e.Type == Types.experience).ToList();

            return View(tripEvents);
        }

        public IActionResult Eventt(Event e)
        {
            var events = _context.Events.Where(e => e.Type == Types.Consert).ToList();
            return View(events);
        }

        public IActionResult Discover()
        {
            var data = from ee in _context.Events
                       select ee;
            List<Event> events = data.ToList();
            return View();
        }

        public IActionResult Aqaba()
        {
            return View();
        }

        public IActionResult Workshop()
        {
            var workshops = _context.Events.Where(e => e.Type == Types.Workshop).ToList();
            return View(workshops);
        }

        public IActionResult ticket(Event e)
        {
            var @event = _context.Events.Find(e.ID);
            return View(@event);
        }

        public IActionResult Signup()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int GenerateRandomId()
        {
            var random = new Random();
            return random.Next(1, 9999); // Adjust the range as needed
        }
    }
}