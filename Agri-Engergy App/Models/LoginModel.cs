using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Agri_Engergy_App.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } //for role-based login (Employee or Farmer)

    }
}
