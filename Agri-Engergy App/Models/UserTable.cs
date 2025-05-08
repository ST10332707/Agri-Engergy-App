using Microsoft.Data.Sqlite;// for database
using System.ComponentModel.DataAnnotations;

namespace Agri_Engergy_App.Models
{
    public class UserTable
    {
        private static string connectionString = "Data Source=App_Data/mydatabase.db";

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

        public int InsertUser(UserTable user)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var createTableCmd = connection.CreateCommand();
                createTableCmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS UserTable (
                        UserID INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserName TEXT NOT NULL,
                        UserSurname TEXT NOT NULL,
                        UserEmail TEXT NOT NULL,
                        Role TEXT NOT NULL,
                        Password TEXT NOT NULL
                    );";
                createTableCmd.ExecuteNonQuery();

                // Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"
                    INSERT INTO UserTable (UserName, UserSurname, UserEmail, Role, Password)
                    VALUES ($name, $surname, $email, $role, $password);";
                insertCmd.Parameters.AddWithValue("$name", user.UserName);
                insertCmd.Parameters.AddWithValue("$surname", user.UserSurname);
                insertCmd.Parameters.AddWithValue("$email", user.UserEmail);
                insertCmd.Parameters.AddWithValue("$role", user.Role);
                insertCmd.Parameters.AddWithValue("$password", hashedPassword);//hashed password saved

                int result = insertCmd.ExecuteNonQuery();
                connection.Close();

                return result;
            }
        }
    }
}
