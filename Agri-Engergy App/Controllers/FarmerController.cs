using Microsoft.AspNetCore.Mvc;

namespace Agri_Engergy_App.Controllers
{
    public class FarmerController : Controller
    {
        public IActionResult Index()
        {
            // Get the name and surname from session
            string userName = HttpContext.Session.GetString("UserName");
            string userSurname = HttpContext.Session.GetString("UserSurname");

            // You can pass it to the view using ViewBag or ViewData
            ViewBag.UserName = userName;
            ViewBag.UserSurname = userSurname;

            return View();
        }
    }
}
