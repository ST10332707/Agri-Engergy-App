using Agri_Engergy_App.Data;
using Microsoft.Data.Sqlite;// for database
using System.ComponentModel.DataAnnotations;

namespace Agri_Engergy_App.Models
{
    public class UserTable
    {
        
        [Key]
        public int UserID { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserSurname { get; set; }

        [Required]
        public string UserEmail { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        private readonly AppDbContext _context = new AppDbContext();

        public int InsertUser(UserTable user)
        {
            using (var con = _context.GetConnection())
            {
                // Create table if not exists
                var createCmd = con.CreateCommand();
                createCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS UserTable (
                        UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserName TEXT NOT NULL,
                        UserSurname TEXT NOT NULL,
                        UserEmail TEXT NOT NULL,
                        Role TEXT NOT NULL,
                        Password TEXT NOT NULL
                    );";
                createCmd.ExecuteNonQuery();

                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                // Insert
                var cmd = con.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO UserTable (UserName, UserSurname, UserEmail, Role, Password)
                    VALUES ($name, $surname, $email, $role, $password);";

                cmd.Parameters.AddWithValue("$name", user.UserName);
                cmd.Parameters.AddWithValue("$surname", user.UserSurname);
                cmd.Parameters.AddWithValue("$email", user.UserEmail);
                cmd.Parameters.AddWithValue("$role", user.Role);
                cmd.Parameters.AddWithValue("$password", hashedPassword);

                return cmd.ExecuteNonQuery();
            }
        }
    }
}
