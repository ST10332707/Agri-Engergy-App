using Microsoft.AspNetCore.Mvc;

namespace Agri_Engergy_App.Models
{
    // Model used for displaying or selecting user information (e.g., farmer shortlisting)
    public class UserModel
    {
        public int UserID { get; set; } // Unique identifier for the user
        public string UserName { get; set; }
        public string UserSurname { get; set; }

    }
}
