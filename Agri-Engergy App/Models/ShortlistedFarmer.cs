using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Agri_Engergy_App.Models
{
    public class ShortlistedFarmer
    {
        [Key]
        public int Id { get; set; }

        public int EmployeeID { get; set; }

        public int FarmerUserID { get; set;}

        public string FarmerName { get; set; }

        public string FarmerSurname { get; set; }


    }
}
