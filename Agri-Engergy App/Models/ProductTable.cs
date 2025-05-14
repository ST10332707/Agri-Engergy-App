using Agri_Engergy_App.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;

namespace Agri_Engergy_App.Models
{
    public class ProductTable
    {
        [Key]
        public int ProductID { get; set; } // Unique ID for each product

        [Required(ErrorMessage = "Product Name is Required")]
        [StringLength(50)]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Prodcut Category is Required")]
        [StringLength(50)]
        public string ProductCategory { get; set; }

        [Required(ErrorMessage = "Product Price is required")]
        [Range(1,int.MaxValue, ErrorMessage ="Price must be bigger then 0")]
        public int ProductPrice { get; set; }

        [Required(ErrorMessage = "Unit required")]
        public string UnitOfMeasurement { get; set; }

        [Required(ErrorMessage = "Product date is Required")]
        public DateOnly ProductionDate { get; set; }

        [Required]
        public int UserID { get; set; } //Foreign key to identify the farmer (UserTable)

        private readonly AppDbContext _context = new AppDbContext();

        // Inserts a product into the database
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

                // Insert product data
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO ProductTable (ProductName, ProductCategory, ProductPrice, UnitOfMeasurement, ProductionDate, UserID)
                    VALUES ($name, $category, $price, $unitOfMeasurement, $date, $userId);";

                // Parameter assignment for SQL injection prevention
                cmd.Parameters.AddWithValue("$name", p.ProductName);
                cmd.Parameters.AddWithValue("$category", p.ProductCategory);
                cmd.Parameters.AddWithValue("$price", p.ProductPrice);
                cmd.Parameters.AddWithValue("$unitOfMeasurement", p.UnitOfMeasurement);
                cmd.Parameters.AddWithValue("$date", p.ProductionDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("$userId", p.UserID);

                return cmd.ExecuteNonQuery();
            }
        }

        //Get all products from database
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

        // Retrieves products specific to a given user (Farmer) using userId
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

        // Retrieves all products grouped by farmer info
        public static List<FarmerProductGroup> GetAllProductsWithFarmerInfo()
        {
            var farmerGroups = new List<FarmerProductGroup>();
            var _context = new AppDbContext();

            using (var con = _context.GetConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    SELECT u.UserID, u.UserName, u.UserSurname,
                           p.ProductID, p.ProductName, p.ProductCategory, 
                           p.ProductPrice, p.UnitOfMeasurement, p.ProductionDate
                    FROM ProductTable p
                    JOIN UserTable u ON p.UserID = u.UserID
                    ORDER BY u.UserID;
                ";

                using (var reader = cmd.ExecuteReader())
                {
                    // Dictionary for grouping products by farmer
                    Dictionary<int, FarmerProductGroup> lookup = new Dictionary<int, FarmerProductGroup>();

                    while (reader.Read())
                    {
                        int userId = reader.GetInt32(reader.GetOrdinal("UserID"));

                        if (!lookup.ContainsKey(userId))
                        {
                            lookup[userId] = new FarmerProductGroup
                            {
                                UserID = userId,
                                FarmerName = reader.GetString(reader.GetOrdinal("UserName")),
                                FarmerSurname = reader.GetString(reader.GetOrdinal("UserSurname")),
                                Products = new List<ProductTable>()

                            };
                        }

                        //Add products to that farmers group
                        var product = new ProductTable
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            ProductCategory = reader.GetString(reader.GetOrdinal("ProductCategory")),
                            ProductPrice = reader.GetInt32(reader.GetOrdinal("ProductPrice")),
                            UnitOfMeasurement = reader.GetString(reader.GetOrdinal("UnitOfMeasurement")),
                            ProductionDate = DateOnly.Parse(reader.GetString(reader.GetOrdinal("ProductionDate")))

                        };

                        lookup[userId].Products.Add(product);
                    }

                    farmerGroups = lookup.Values.ToList();
                }
            }
            return farmerGroups;
        }



    }
}
