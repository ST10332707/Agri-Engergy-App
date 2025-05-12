using Agri_Engergy_App.Models;
using Microsoft.AspNetCore.Mvc;

////////////////////////////////////////////////////UNK//////////////////////////////////////////////////////////////////////////

namespace Agri_Engergy_App.Controllers
{
    public class SignUpController : Controller
    {
        // Initialize the user model
        public UserTable usertbl = new UserTable();

        // GET: SignUp
        [HttpGet]
        public IActionResult Index()
        {
            return View(usertbl); // Pass an empty model to the view
        }

        // POST: SignUp/Submit
        [HttpPost]
        public IActionResult SignUp(UserTable user)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(user.Role))
            {
                ModelState.AddModelError("Role", "Please select a role.");
                return View("Index", user); // Show form again with validation
            }

            var result = usertbl.InsertUser(user);

            if (result > 0)
            {
                return RedirectToAction("Index", "Home"); // Success
            }
            else
            {
                return View("Error"); // Something went wrong
            }
        }
    }
}
////////////////////////////////////////////////////UNK//////////////////////////////////////////////////////////////////////////
