using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    public enum UserRoles
    {
        super_admin,
        administrator,
        moderator,
        user
    }

    public abstract class UserAssignmentClass
    {
        public static string connectionString = "Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        public string Username;
        public string Passphrase;
        public UserRoles LevelOfUserAccess;

        //Να τον βαλω να υλοποιεί την read


        //public User(string username, string passphrase, UserRoles userRole)
        //{
        //    using (SqlConnection dbcon = new SqlConnection(connectionString))
        //    {
        //        dbcon.Open();
        //        SqlCommand getUsername = new SqlCommand($"SELECT username FROM CurrentLoginCredentials", dbcon);
        //        string username = (string)getUsername.ExecuteScalar();
        //    }
        //}
    }

    public class SuperAmdin : UserAssignmentClass
    {
        public static void DeleteUserFromDatabase(string username)
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand deleteUsername = new SqlCommand($"DELETE FROM LoginCredentials WHERE username = '{username}' ", dbcon);
                deleteUsername.ExecuteScalar();
            }
            Console.WriteLine($"Username {username} has been successfully deleted from database");
        }
        
    }
}