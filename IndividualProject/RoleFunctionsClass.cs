using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class RoleFunctionsClass
    {
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        static readonly string path = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\NewUserRequest.txt";

        public static void CreateNewUserFromRequestFunction()
        {
            
            string pendingUsername = File.ReadLines(path).First();
            if (pendingUsername == " ")
            {
                Console.WriteLine("\r\nThere are no pending requests");
                InputOutputControlClass.ClearScreen();
                ApplicationMenuClass.LoginScreen();
            }
            else
            {
                pendingUsername = pendingUsername.Remove(0, 10);

                string pendingPassphrase = File.ReadLines(path).Skip(1).Take(1).First();
                pendingPassphrase = pendingPassphrase.Remove(0, 12);

                Console.WriteLine($"\r\nYou are about to create a new username-password entry : {pendingUsername} - {pendingPassphrase}");
                string pendingRole = InputOutputControlClass.SelectUserRole();

                using (SqlConnection dbcon = new SqlConnection(connectionString))
                {
                    dbcon.Open();
                    SqlCommand appendUserToDatabase = new SqlCommand($"INSERT INTO LoginCredentials VALUES ('{pendingUsername}', '{pendingPassphrase}')", dbcon);
                    SqlCommand appendUserRoleToDatabase = new SqlCommand($"INSERT INTO UserLevelAccess VALUES ('{pendingUsername}', '{pendingRole}')", dbcon);
                    appendUserToDatabase.ExecuteScalar();
                    appendUserRoleToDatabase.ExecuteScalar();

                }
                ConsoleOutputAndAnimations.CreatingNewUserOutput();
                Console.WriteLine($"User {pendingUsername} has been created successfully. Status : {pendingRole}");
                File.WriteAllLines(path, new string[] { " " });
                InputOutputControlClass.ClearScreen();
                ApplicationMenuClass.LoginScreen();
            }
            
        }

        public static void DeleteUserFromDatabase()
        {
            Console.WriteLine("\r\nChoose a User from the list and proceed to delete");
            Dictionary<string, string> AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
            
            string username = InputOutputControlClass.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}");
                    username = InputOutputControlClass.UsernameInput();
                }
                else
                {
                    Console.WriteLine("Cannot delete super_admin! Please choose a different user");
                    username = InputOutputControlClass.UsernameInput();
                }
            }

            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand deleteUsername = new SqlCommand($"RemoveUsernameFromDatabase @username = '{username}'", dbcon);
                deleteUsername.ExecuteNonQuery();
            }
            ConsoleOutputAndAnimations.DeletingExistingUserOutput();
            Console.WriteLine($"Username {username} has been successfully deleted from database");
            InputOutputControlClass.ClearScreen();
            ApplicationMenuClass.LoginScreen();
        }

        public static Dictionary<string, string> ShowAvailableUsersFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand ShowUsersFromDatabase = new SqlCommand("SELECT * FROM UserLevelAccess", dbcon);
                using (var reader = ShowUsersFromDatabase.ExecuteReader())
                {
                    Dictionary<string, string> AvailableUsernamesDictionary = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        var username = reader[0];
                        var status = reader[1];
                        AvailableUsernamesDictionary.Add((string)username, (string)status);
                        Console.WriteLine($"username: {username} - status: {status}");
                    }
                    return AvailableUsernamesDictionary;
                }
            } 
        }

        public static void CheckUserNotifications()
        {
            
        }

        public static void CheckAdminNotifications()
        {
            string pendingUsernameCheck = File.ReadLines(path).First();

            if (pendingUsernameCheck == " ")
            {
                Console.WriteLine("\r\nThere are no pending User registrations");
                InputOutputControlClass.ClearScreen();
                ApplicationMenuClass.LoginScreen();
            }
            else
            {
                Console.WriteLine("\r\nYou have 1 pending User registration request. Would you like to create new user?");
                InputOutputControlClass.PromptYesOrNo();
                CreateNewUserFromRequestFunction();
            }
        }
    }
}
