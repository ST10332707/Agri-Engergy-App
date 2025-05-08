using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Agri_Engergy_App.Models;

namespace Agri_Engergy_App.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserTable> Users { get; set; }

        private readonly string _dbPath;

        public AppDbContext()
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            Directory.CreateDirectory(folderPath); // Ensures folder exists
            _dbPath = Path.Combine(folderPath, "myDatabase.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPath}");
        }
    }
}
