using Microsoft.AspNetCore.Mvc;
using Agri_Engergy_App.Data;
using Agri_Engergy_App.Models;
using System.Data;

namespace Agri_Engergy_App.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Show login form
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var loginModel = new LoginModel();
            using (var con = _context.GetConnection())
            {
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    SELECT * FROM UserTable
                    WHERE UserEmail = $email
                ";
                cmd.Parameters.AddWithValue("$email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string hashedPassword = reader.GetString(reader.GetOrdinal("Password"));
                        int userId = reader.GetInt32(reader.GetOrdinal("UserID"));
                        string userName = reader.GetString(reader.GetOrdinal("UserName"));
                        string userSurname = reader.GetString(reader.GetOrdinal("UserSurname"));
                        string userRole = reader.GetString(reader.GetOrdinal("Role"));

                        // Verify password
                        if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                        {
                            //// Store user ID in session
                            //HttpContext.Session.SetInt32("UserID", userId);
                            //TempData["UserName"] = userName;

                            // ✅ Store info in session
                            HttpContext.Session.SetInt32("UserID", userId);
                            HttpContext.Session.SetString("UserName", userName);
                            HttpContext.Session.SetString("UserSurname", userSurname);
                            HttpContext.Session.SetString("UserRole", userRole);

                            // Redirect based on role
                            if (userRole == "Farmer")
                                return RedirectToAction("Index", "Farmer");
                            else if (userRole == "Employee")
                                return RedirectToAction("Index", "Employee");
                            else
                                return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }

            // Login failed
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View();
        }
    }
}
