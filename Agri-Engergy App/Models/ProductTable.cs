using Agri_Engergy_App.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Agri_Engergy_App.Models
{
    public class ProductTable
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductCategory { get; set; }

        [Required]
        public int ProductPrice { get; set; }

        [Required]
        public string UnitOfMeasurement { get; set; }       

        [Required]
        public DateOnly ProductionDate { get; set; }

        private readonly AppDbContext _context = new AppDbContext();

        public int InsertProduct(ProductTable p)
        {
            using (var con = _context.GetConnection())
            {
                // Create table if not exists
                var createCmd = con.CreateCommand();
                createCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS ProductTable (
                        ProductID INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductName TEXT NOT NULL,
                        ProductCategory TEXT NOT NULL,
                        ProductPrice INTEGER NOT NULL,
                        UnitOfMeasurement TEXT NOT NULL,
                        ProductionDate Date NOT NULL
                        
                    );";
                createCmd.ExecuteNonQuery();

                // Insert
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO ProductTable (ProductName, ProductCategory, ProductPrice, UnitOfMeasurement, ProductionDate )
                    VALUES ($name, $category, $price, $unitOfMeasurement, $date);";

                cmd.Parameters.AddWithValue("$name", p.ProductName);
                cmd.Parameters.AddWithValue("$category", p.ProductCategory);
                cmd.Parameters.AddWithValue("$price", p.ProductPrice);
                cmd.Parameters.AddWithValue("$unitOfMeasurement", p.UnitOfMeasurement);
                cmd.Parameters.AddWithValue("$date", p.ProductionDate.ToString("yyyy-MM-dd"));               

                return cmd.ExecuteNonQuery();
            }
        }
    }
}
