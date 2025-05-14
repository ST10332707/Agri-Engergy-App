using Agri_Engergy_App.Data;
using Microsoft.Data.Sqlite;// for database
using System.ComponentModel.DataAnnotations;

namespace Agri_Engergy_App.Models
{
    // Main user entity class that maps to the UserTable in the database
    public class UserTable
    {        
        [Key]
        public int UserID { get; set; } // Primary key for the user table


        [Required(ErrorMessage ="Name is Required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Surname is Required")]
        [StringLength(50, ErrorMessage = "Surname cannot exceed 50 characters")]
        public string UserSurname { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [StringLength(25, ErrorMessage = "Email cannot exceed 25 characters")]
        public string UserEmail { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        // Local database context for performing DB operations
        private readonly AppDbContext _context = new AppDbContext();

        // Inserts a new user into the UserTable
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

                // Insert data into table
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
