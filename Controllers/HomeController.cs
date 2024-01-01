

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using TestGp.Models;



namespace TestGp.Controllers
{
    public class HomeController : Controller
    { private Mydb _context;
      
        public HomeController(Mydb context)
        {
            _context = context;
        }
        
        private readonly ILogger<HomeController> _logger;

        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Login L)
        {
            var guest = from gs in _context.Registers
                        where gs.Email.Equals(L.Email) && gs.Password.Equals(L.Password)
                       select gs; // to check the database if there is a corresponding username/passsword

            // if user info is wrong or empty , give error message

            if (guest.Count() >= 1)
            {
               
               
                 Models.Register gst = guest.FirstOrDefault();
                 string Email = gst.Email;
                HttpContext.Session.SetString("s1", Email);
                Models.Register gstt = guest.FirstOrDefault();
                 string Pass = gst.Password;
                 HttpContext.Session.SetString("s1", Pass);
                TempData["Msg"] = "LogedIn ";
            
                return RedirectToAction("Index");
               
            }else  
           
            {

                TempData["Msg"] = "UnSuccessfull";

                return View();


            }

        }jhjh
      
       
        public IActionResult Logout()
        {
            if (HttpContext.Session.Keys.Any())
            {
                HttpContext.Session.Clear();
                TempData["log out"] = " logout done";
                return RedirectToAction("Index");
            }
            else {
                TempData["log outt"] = "logout feild";
                //return RedirectToAction("Index");
  
            } return RedirectToAction("Index");
         
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
                if (guest.Count()>=1)
                {
                    TempData["Error message"] = " the email or username are already exist";
                    return View();
                }
                else
                {
                    // Process registration logic here
                    R.ID = GenerateRandomId();
                    _context.Registers.Add(R);
                    _context.SaveChanges(); // This code to insert to the table. 
                    ModelState.Clear();
                    var sortedRegisters = _context.Registers.OrderBy(r => r.ID).ToList();// TO CLEAR INFO AFTER SUBMIT
                    TempData["Msgg"] = "Inserted Successfully"; // go to signup view and add it 

                    return RedirectToAction("Login");
                }
                // Redirect to a success page or another action
                
            

       
            

           
            
        }



        public IActionResult Addevent()
        {
            return View();
        }
        public IActionResult Orgnizerpage()
        {
            return View();
        }
        public IActionResult Dasboard()
        {
            return View();
        }


        public IActionResult Discover()
        {
            return View();
        }

        public IActionResult Experience()
        {
            return View();
        }
        public IActionResult Aqaba()
        {
            return View();
        }


        public IActionResult Payment()
        {
            return View();
        }
        [HttpGet]
       public IActionResult Booking()
       {
            return View();
       }
        [HttpPost]
        public IActionResult Booking(Booking b)
        {
            if (ModelState.IsValid)
            {
                // Process registration logic here
                b.BookingID = GenerateRandomId();
                _context.Bookings.Add(b);
                _context.SaveChanges(); // This code to insert to the table. 
                ModelState.Clear();
                var sortedBookings = _context.Bookings.OrderBy(r => r.BookingID).ToList();// TO CLEAR INFO AFTER SUBMIT
                TempData["Msgg"] = "Inserted Successfully"; // go to signup view and add it 
                return RedirectToAction("Payment");
                // Redirect to a success page or another action

            }


            return View();
        }
       // private int GenerateRandomBookinggId()
        //{
        //    var random = new Random();
        //    return random.Next(1, 999999); // Adjust the range as needed
      //  }

        public IActionResult Events()
        {
            return View();
        }
        public IActionResult Ticket()
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
