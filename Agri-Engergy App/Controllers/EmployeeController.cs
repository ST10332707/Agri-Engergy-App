using Agri_Engergy_App.Data;
using Agri_Engergy_App.Models;
using Microsoft.AspNetCore.Mvc;


namespace Agri_Engergy_App.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly AppDbContext _context = new AppDbContext();

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Employee")
                return Unauthorized();

            // Get the name and surname from session
            string userName = HttpContext.Session.GetString("UserName");
            string userSurname = HttpContext.Session.GetString("UserSurname");

            // You can pass it to the view using ViewBag or ViewData
            ViewBag.UserName = userName;
            ViewBag.UserSurname = userSurname;

            return View();
        }

        public IActionResult DisplayProduct()
        {

            if (HttpContext.Session.GetString("UserRole") != "Employee")
                return Unauthorized();

            var farmerProducts = ProductTable.GetAllProductsWithFarmerInfo();
            return View(farmerProducts); // Pass grouped data to view
        }

        public IActionResult ShortlistedFarmers()
        {
            if (HttpContext.Session.GetString("UserRole") != "Employee")
                return Unauthorized();

            var shortlistService = new ShortlistService();
            var shortlisted = shortlistService.GetAllShortlisted();

            // If no shortlisted farmers, fetch all farmers (Role = Farmer)
            if (shortlisted.Count == 0)
            {
                var farmers = new List<UserModel>();
                using var con = _context.GetConnection();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM UserTable WHERE Role = 'Farmer'";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    farmers.Add(new UserModel
                    {
                        UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        UserSurname = reader.GetString(reader.GetOrdinal("UserSurname")),
                    });
                }

                ViewBag.AvailableFarmers = farmers;

            }
            return View(shortlisted);
        }

        [HttpPost]
        public IActionResult AddToShortlist(int farmerUserId, string farmerName, string farmerSurname)
        {
            if (HttpContext.Session.GetString("UserRole") != "Employee")
                return Unauthorized();

            int employeeId = HttpContext.Session.GetInt32("UserID") ?? 0;
            var service = new ShortlistService();
            service.AddToShortlist(employeeId, farmerUserId, farmerName, farmerSurname);
            return RedirectToAction("ShortlistedFarmers");
        }

    }
}
