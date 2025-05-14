using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Agri_Engergy_App.Models
{
    // Model used for displaying or selecting user information (e.g., farmer shortlisting)
    public class UserModel
    {
        public int UserID { get; set; } // Unique identifier for the user

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Surname is Required")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
        public string UserSurname { get; set; }

    }
}
