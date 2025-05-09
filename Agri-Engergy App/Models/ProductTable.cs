using Agri_Engergy_App.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

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

        [Required]
        public int UserID { get; set; } // Foreign key to UserTable

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
                        ProductionDate Date NOT NULL,
                        UserID INTEGER NOT NULL                        
                    );";
                createCmd.ExecuteNonQuery();

                // Insert
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO ProductTable (ProductName, ProductCategory, ProductPrice, UnitOfMeasurement, ProductionDate, UserID)
                    VALUES ($name, $category, $price, $unitOfMeasurement, $date, $userId);";

                cmd.Parameters.AddWithValue("$name", p.ProductName);
                cmd.Parameters.AddWithValue("$category", p.ProductCategory);
                cmd.Parameters.AddWithValue("$price", p.ProductPrice);
                cmd.Parameters.AddWithValue("$unitOfMeasurement", p.UnitOfMeasurement);
                cmd.Parameters.AddWithValue("$date", p.ProductionDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("$userId", p.UserID);

                return cmd.ExecuteNonQuery();
            }
        }

        public static List<ProductTable> GetAllProducts()
        {
            List<ProductTable> products = new List<ProductTable>();
            var _context = new AppDbContext();

            using (var con = _context.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT * FROM ProductTable";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductTable product = new ProductTable
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            ProductCategory = reader.GetString(reader.GetOrdinal("ProductCategory")),
                            ProductPrice = reader.GetInt32(reader.GetOrdinal("ProductPrice")),
                            UnitOfMeasurement = reader.GetString(reader.GetOrdinal("UnitOfMeasurement")),
                            ProductionDate = DateOnly.Parse(reader.GetString(reader.GetOrdinal("ProductionDate")))
                        };
                        products.Add(product);
                    }
                }
            }
            return products;
        }

        public static List<ProductTable> GetProductsByUserId(int userId)
        {
            List<ProductTable> products = new List<ProductTable>();
            var _context = new AppDbContext();

            using (var con = _context.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    SELECT * FROM ProductTable
                    WHERE UserID = $userId";
                cmd.Parameters.AddWithValue("$userId", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ProductTable product = new ProductTable
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            ProductCategory = reader.GetString(reader.GetOrdinal("ProductCategory")),
                            ProductPrice = reader.GetInt32(reader.GetOrdinal("ProductPrice")),
                            UnitOfMeasurement = reader.GetString(reader.GetOrdinal("UnitOfMeasurement")),
                            ProductionDate = DateOnly.Parse(reader.GetString(reader.GetOrdinal("ProductionDate")))
                        };
                        products.Add(product);
                    }
                }
            }
            return products;
        }

    }
}
