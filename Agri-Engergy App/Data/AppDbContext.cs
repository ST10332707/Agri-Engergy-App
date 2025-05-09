using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Agri_Engergy_App.Models;
using Microsoft.Data.Sqlite;

namespace Agri_Engergy_App.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext()
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(folderPath); // ensure directory exists

            string dbPath = Path.Combine(folderPath, "AgriDatabase.db");
            _connectionString = $"Data Source={dbPath}";
        }

        public SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
