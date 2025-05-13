using Agri_Engergy_App.Data;
using Agri_Engergy_App.Models;
using Microsoft.AspNetCore.Mvc;

////////////////////////////////////////////////////UNK//////////////////////////////////////////////////////////////////////////

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

            int? employeeId = HttpContext.Session.GetInt32("UserID");

            if (employeeId == null)
                return RedirectToAction("Login", "Login");

            var shortlisted = new List<ShortlistedFarmer>();
            using (var con = _context.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM ShortlistedFarmers WHERE EmployeeID = $empId";
                cmd.Parameters.AddWithValue("$empId", employeeId);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    shortlisted.Add(new ShortlistedFarmer
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("ID")),
                        EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                        FarmerUserID = reader.GetInt32(reader.GetOrdinal("FarmerUserID")),
                        FarmerName = reader.GetString(reader.GetOrdinal("FarmerName")),
                        FarmerSurname = reader.GetString(reader.GetOrdinal("FarmerSurname"))

                    });
                }
            }

            // If empty, fetch all farmers not yet shortlisted
            if (shortlisted.Count < 10) ////< 10
            {
                var availableFarmers = new List<UserModel>();
                using var con = _context.GetConnection();
                con.Open(); //////
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    SELECT * FROM UserTable 
                    WHERE Role = 'Farmer' AND UserID NOT IN 
                        (SELECT FarmerUserID FROM ShortlistedFarmers WHERE EmployeeID = $empId)";
                cmd.Parameters.AddWithValue("$empId", employeeId);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    availableFarmers.Add(new UserModel
                    {
                        UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                        UserName = reader.GetString(reader.GetOrdinal("UserName")),
                        UserSurname = reader.GetString(reader.GetOrdinal("UserSurname")),

                    });
                }
                ViewBag.AvailableFarmers = availableFarmers;
            }
                return View(shortlisted);
        }

        [HttpPost]
        public IActionResult AddToShortlist(int farmerUserId, string farmerName, string farmerSurname)
        {
            if (HttpContext.Session.GetString("UserRole") != "Employee")
               return Unauthorized();

            int? employeeId = HttpContext.Session.GetInt32("UserID");

            if (employeeId == null)
                return RedirectToAction("Login", "Login");

            using (var con = _context.GetConnection())
            {
                con.Open();


                // Count how many Farmers are already shortlisted
                var countCmd = con.CreateCommand();
                countCmd.CommandText = "SELECT COUNT(*) FROM ShortlistedFarmers WHERE EmployeeID = $empId";
                countCmd.Parameters.AddWithValue("$empId", employeeId);
                int count = Convert.ToInt32(countCmd.ExecuteScalar());

                //ensure that 10 or less Farmer can be shortlisted
                if (count >= 10)
                {
                    TempData["Error"] = "You can only shortlist up to 10 farmers.";
                    return RedirectToAction("ShortlistedFarmers");
                }

                // Check if already shortlisted
                var checkCmd = con.CreateCommand();
                checkCmd.CommandText = @"
                         SELECT COUNT(*) FROM ShortlistedFarmers 
                         WHERE EmployeeID = $empId AND FarmerUserID = $farmerId";
                checkCmd.Parameters.AddWithValue("$empId", employeeId);
                checkCmd.Parameters.AddWithValue("$farmerId", farmerUserId);

                


                var exists = Convert.ToInt32(checkCmd.ExecuteScalar()) > 0;
                if (!exists)
                {
                    var cmd = con.CreateCommand();
                    cmd.CommandText = @"
                        INSERT INTO ShortlistedFarmers (EmployeeID, FarmerUserID, FarmerName, FarmerSurname)
                        VALUES ($empId, $farmerId, $name, $surname)";
                    cmd.Parameters.AddWithValue("$empId", employeeId);
                    cmd.Parameters.AddWithValue("$farmerId", farmerUserId);
                    cmd.Parameters.AddWithValue("$name", farmerName);
                    cmd.Parameters.AddWithValue("$surname", farmerSurname);
                    cmd.ExecuteNonQuery();

                }
            }
                return RedirectToAction("ShortlistedFarmers");
        }



    }
}
////////////////////////////////////////////////////UNK//////////////////////////////////////////////////////////////////////////
