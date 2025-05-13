using Microsoft.AspNetCore.Mvc;

namespace Agri_Engergy_App.Models
{
    // Model that groups a farmer's basic info and their product list
    public class FarmerProductGroup
    {
        public int UserID { get; set; } // Farmer's unique identifier
        public string FarmerName { get; set; }
        public string FarmerSurname { get; set; }
        public List<ProductTable> Products { get; set; }
    }
}
