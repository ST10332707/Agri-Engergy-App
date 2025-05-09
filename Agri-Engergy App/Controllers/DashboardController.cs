using Microsoft.AspNetCore.Mvc;

namespace Agri_Engergy_App.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult FarmerDashboard()
        {
            return Content("Welcome, Farmer!");
        }

        public IActionResult EmployeeDashboard()
        {
            return Content("Welcome, Employee!");
        }
    }
}
