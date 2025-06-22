using Agri_Engergy_App.Data;
using Agri_Engergy_App.Models;
using Microsoft.AspNetCore.Mvc;

////////////////////////////////////////////////////UNK//////////////////////////////////////////////////////////////////////////

namespace Agri_Engergy_App.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly AppDbContext _context = new AppDbContext();// Database context for database operations

        // Landing page for Employee users
        public IActionResult Index()
        {
            //Verify that user is an employee
            if (HttpContext.Session.GetString("UserRole") != "Employee")
                return Unauthorized();

            // Get the name and surname from session
            string userName = HttpContext.Session.GetString("UserName");
            string userSurname = HttpContext.Session.GetString("UserSurname");

            // Send user name and surname to the view
            ViewBag.UserName = userName;
            ViewBag.UserSurname = userSurname;

            return View();
        }

        // Display all farmer products
        public IActionResult DisplayProduct()
        {
            //Verify that user is an employee
            if (HttpContext.Session.GetString("UserRole") != "Employee")
                return Unauthorized();

            var farmerProducts = ProductTable.GetAllProductsWithFarmerInfo();
            return View(farmerProducts); // Pass grouped data to view
        }

        //// Display the list of shortlisted farmers (max 10) for specific employee
        //public IActionResult ShortlistedFarmers(string category, decimal? minPrice, decimal? maxPrice)
        //{
        //    //Verify that user is an employee
        //    if (HttpContext.Session.GetString("UserRole") != "Employee")
        //        return Unauthorized();

        //    // Get employee ID from session
        //    int? employeeId = HttpContext.Session.GetInt32("UserID");

        //    if (employeeId == null)
        //        return RedirectToAction("Login", "Login");

        //    // Retrieve existing shortlisted farmers for this employee
        //    var shortlisted = new List<ShortlistedFarmer>();
        //    using (var con = _context.GetConnection())
        //    {
        //        con.Open();
        //        var cmd = con.CreateCommand();
        //        cmd.CommandText = "SELECT * FROM ShortlistedFarmers WHERE EmployeeID = $empId";
        //        cmd.Parameters.AddWithValue("$empId", employeeId);

        //        using var reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            shortlisted.Add(new ShortlistedFarmer
        //            {
        //                Id = reader.GetInt32(reader.GetOrdinal("ID")),
        //                EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
        //                FarmerUserID = reader.GetInt32(reader.GetOrdinal("FarmerUserID")),
        //                FarmerName = reader.GetString(reader.GetOrdinal("FarmerName")),
        //                FarmerSurname = reader.GetString(reader.GetOrdinal("FarmerSurname"))

        //            });
        //        }
        //    }

        //    // filter through products (per farmer)
        //    if (!string.IsNullOrEmpty(category) || minPrice.HasValue || maxPrice.HasValue)
        //    {
        //        shortlisted = shortlisted.Where(farmer =>
        //        {
        //            var products = ProductTable.GetProductsByUserId(farmer.FarmerUserID);

        //            return products.Any(p =>
        //                (string.IsNullOrEmpty(category) || p.ProductCategory == category) &&
        //                (!minPrice.HasValue || p.ProductPrice >= minPrice.Value) &&
        //                (!maxPrice.HasValue || p.ProductPrice <= maxPrice.Value)
        //            );
        //        }).ToList();
        //    }

        //    // If fewer than 10 shortlisted, fetch available farmers not already shortlisted
        //    if (shortlisted.Count < 10) ////< 10
        //    {
        //        var availableFarmers = new List<UserModel>();
        //        using var con = _context.GetConnection();
        //        con.Open(); //////
        //        var cmd = con.CreateCommand();
        //        cmd.CommandText = @"
        //            SELECT * FROM UserTable 
        //            WHERE Role = 'Farmer' AND UserID NOT IN 
        //                (SELECT FarmerUserID FROM ShortlistedFarmers WHERE EmployeeID = $empId)";
        //        cmd.Parameters.AddWithValue("$empId", employeeId);
        //        using var reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            availableFarmers.Add(new UserModel
        //            {
        //                UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
        //                UserName = reader.GetString(reader.GetOrdinal("UserName")),
        //                UserSurname = reader.GetString(reader.GetOrdinal("UserSurname")),

        //            });
        //        }
        //        ViewBag.AvailableFarmers = availableFarmers;
        //    }
        //        return View(shortlisted);
        //}

        // POST: Add a farmer to the current employee's shortlist
        [HttpPost]
        public IActionResult AddToShortlist(int farmerUserId, string farmerName, string farmerSurname)
        {
            //Verify that user is an employee
            if (HttpContext.Session.GetString("UserRole") != "Employee")
               return Unauthorized();

            // Get employee ID from session
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

        public IActionResult FilteredProducts(string category, int? minPrice, int? maxPrice)
{
    // Verify employee access
    if (HttpContext.Session.GetString("UserRole") != "Employee")
        return Unauthorized();

    var filteredProducts = ProductTable.FilterProducts(category, minPrice, maxPrice);
    return View(filteredProducts);
}

    }
}
////////////////////////////////////////////////////UNK//////////////////////////////////////////////////////////////////////////
