using Agri_Engergy_App.Models;
using Microsoft.AspNetCore.Mvc;


namespace Agri_Engergy_App.Controllers
{
    public class EmployeeController : Controller
    {
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

    }
}
