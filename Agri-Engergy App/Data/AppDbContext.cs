using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Agri_Engergy_App.Models;
using Microsoft.Data.Sqlite;

////////////////////////////////////////////////////UNK//////////////////////////////////////////////////////////////////////////

namespace Agri_Engergy_App.Data
{
    // Custom database context class for managing SQLite connection
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        // Constructor initializes the connection string and ensures the database directory exists
        public AppDbContext()
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(folderPath);

            string dbPath = Path.Combine(folderPath, "AgriDatabase.db");
            _connectionString = $"Data Source={dbPath}";
        }

        // Method to get a live SQLite connection
        public SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
////////////////////////////////////////////////////UNK//////////////////////////////////////////////////////////////////////////