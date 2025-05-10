using Microsoft.AspNetCore.Mvc;
using Agri_Engergy_App.Models;

namespace Agri_Engergy_App.Controllers
{
    public class FarmerController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Farmer")
                return Unauthorized();

            // Get the UserID, name and surname from session
            string userName = HttpContext.Session.GetString("UserName");
            string userSurname = HttpContext.Session.GetString("UserSurname");

            // You can pass it to the view using ViewBag or ViewData
            ViewBag.UserName = userName;
            ViewBag.UserSurname = userSurname;

            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(ProductTable p)
        {
            //if (HttpContext.Session.GetString("UserRole") != "Farmer")
            //    return Unauthorized();

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Login");

            if (ModelState.IsValid)
            {
                p.UserID = userId.Value;

                var result = p.InsertProduct(p);
                if (result > 0)
                {
                    TempData["Success"] = "Product added successfully.";
                    return RedirectToAction("DisplayProduct");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add product.");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            if (HttpContext.Session.GetString("UserRole") != "Farmer")
                return Unauthorized();
            return View(); // This should render Views/Farmer/AddProduct.cshtml
        }

        public IActionResult DisplayProduct()
        {
            if (HttpContext.Session.GetString("UserRole") != "Farmer")
                return Unauthorized();

            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
                return RedirectToAction("Login", "Login");

            var products = ProductTable.GetProductsByUserId(userId.Value);
            return View(products);
        }
    }
}
