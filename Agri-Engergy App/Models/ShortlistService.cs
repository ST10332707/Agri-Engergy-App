using Agri_Engergy_App.Data;
using Microsoft.AspNetCore.Mvc;

namespace Agri_Engergy_App.Models
{
    // Controller-based service for handling farmer shortlisting logic
    public class ShortlistService : Controller
    {
        private readonly AppDbContext _context = new AppDbContext();

        // Ensures the ShortlistedFarmers table exists
        public void EnsureTableCreated()
        {
            using var con = _context.GetConnection();
            con.Open();
            var cmd = con.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS ShortlistedFarmers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    EmployeeID INTEGER,
                    FarmerUserID INTEGER,
                    FarmerName TEXT,
                    FarmerSurname TEXT
                );";
            cmd.ExecuteNonQuery();
        }

        // Adds a farmer to the shortlist for a specific employee
        public void AddToShortlist(int employeeId, int farmerUserId, string farmerName, string farmerSurname)
        {
            EnsureTableCreated();// Make sure table exists before inserting
            using var con = _context.GetConnection();
            var cmd = con.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO ShortlistedFarmers (EmployeeID, FarmerUserID, FarmerName, FarmerSurname)
                VALUES ($employeeId, $farmerUserId, $farmerName, $farmerSurname);";
            cmd.Parameters.AddWithValue("$employeeId", employeeId);
            cmd.Parameters.AddWithValue("$farmerUserId", farmerUserId);
            cmd.Parameters.AddWithValue("$farmerName", farmerName);
            cmd.Parameters.AddWithValue("$farmerSurname", farmerSurname);
            cmd.ExecuteNonQuery();
        }

        // Returns a list of all shortlisted farmers
        public List<ShortlistedFarmer> GetAllShortlisted()
        {
            EnsureTableCreated();
            var results = new List<ShortlistedFarmer>();

            using var con = _context.GetConnection();
            var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM ShortlistedFarmers";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ShortlistedFarmer
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID")),
                    FarmerUserID = reader.GetInt32(reader.GetOrdinal("FarmerUserID")),
                    FarmerName = reader.GetString(reader.GetOrdinal("FarmerName")),
                    FarmerSurname = reader.GetString(reader.GetOrdinal("FarmerSurname"))
                });
            }

            return results;
        }

        // Retrieves all users with the role of 'Farmer' from the UserTable
        public List<UserModel> GetAllFarmers()
        {
            var farmers = new List<UserModel>();
            using var con = _context.GetConnection();
            con.Open();

            var cmd = con.CreateCommand();
            cmd.CommandText = @"
                SELECT UserID, UserName, UserSurname 
                FROM UserTable 
                WHERE Role = 'Farmer'";

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                farmers.Add(new UserModel
                {
                    UserID = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    UserSurname = reader.GetString(2)

                });
            }
            return farmers;
        }

    }
}
