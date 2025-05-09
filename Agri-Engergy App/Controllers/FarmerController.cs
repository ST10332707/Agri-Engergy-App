using Microsoft.AspNetCore.Mvc;
using Agri_Engergy_App.Models;

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

        [HttpPost]
        public IActionResult AddProduct(ProductTable p)
        {
            if (ModelState.IsValid)
            {
                var result = p.InsertProduct(p);
                if (result > 0)
                {
                    TempData["Success"] = "Product added successfully.";
                    return RedirectToAction("Index");
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
            return View(); // This should render Views/Farmer/AddProduct.cshtml
        }
    }
}
