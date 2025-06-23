using Microsoft.AspNetCore.Mvc; 
using System.ComponentModel.DataAnnotations;

namespace Agri_Engergy_App.Models
{
    // Model representing a farmer shortlisted by an employee
    public class ShortlistedFarmer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public int FarmerUserID { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string FarmerName { get; set; }

        [Required(ErrorMessage = "Surname is Required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string FarmerSurname { get; set; }

    }
}
