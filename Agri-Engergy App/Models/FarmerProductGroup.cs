using Microsoft.AspNetCore.Mvc;

namespace Agri_Engergy_App.Models
{
    public class FarmerProductGroup
    {
        public int UserID { get; set; }
        public string FarmerName { get; set; }
        public string FarmerSurname { get; set; }
        public List<ProductTable> Products { get; set; }
    }
}
