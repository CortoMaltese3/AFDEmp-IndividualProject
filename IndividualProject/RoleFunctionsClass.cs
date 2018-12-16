using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    class RoleFunctionsClass
    {
        public static void CreateNewUserFromRequestFunction()
        {
            string path = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\NewUserRequest.txt";
            string pendingUsername = File.ReadLines(path).First();
            pendingUsername = pendingUsername.Remove(0, 10);

            string pendingPassphrase = File.ReadLines(path).Skip(1).Take(1).First();
            pendingPassphrase = pendingPassphrase.Remove(0, 12);
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";

            Console.WriteLine($"You are about to create a new username-password entry : {pendingUsername} - {pendingPassphrase}. Please select User's role :");
            string pendingRole = InputOutputControlClass.SelectUserRole();

            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand appendUserToDatabase = new SqlCommand($"INSERT INTO LoginCredentials VALUES ('{pendingUsername}', '{pendingPassphrase}')", dbcon);
                SqlCommand appendUserRoleToDatabase = new SqlCommand($"INSERT INTO UserLevelAccess VALUES ('{pendingUsername}', '{pendingRole}')", dbcon);
                appendUserToDatabase.ExecuteScalar();
                appendUserRoleToDatabase.ExecuteScalar();

            }
            Console.WriteLine($"User {pendingUsername} has been created successfully. Status : {pendingRole}");
            File.WriteAllLines(path, new string[] { " " });
            InputOutputControlClass.ClearScreen();
            ApplicationMenuClass.LoginScreen();
        }
    }
}
