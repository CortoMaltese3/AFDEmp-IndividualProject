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
        static readonly string newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";

        public static void CreateNewUserFromRequestFunction()
        {
            string pendingUsername = File.ReadLines(newUserRequestPath).First();
            if (pendingUsername == " ")
            {
                InputOutputAnimationControlClass.QuasarScreen();
                InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");
                Console.Write("There are no pending requests");
                ActiveUserFunctionsClass.ActiveUserProcedures();
                //ApplicationMenuClass.LoginScreen();
            }
            else
            {
                pendingUsername = pendingUsername.Remove(0, 10);

                string pendingPassphrase = File.ReadLines(newUserRequestPath).Skip(1).Take(1).First();
                pendingPassphrase = pendingPassphrase.Remove(0, 12);
                InputOutputAnimationControlClass.QuasarScreen();
                Console.WriteLine($"\r\nYou are about to create a new username-password entry : {pendingUsername} - {pendingPassphrase}");
                string pendingRole = InputOutputAnimationControlClass.SelectUserRole();

                using (SqlConnection dbcon = new SqlConnection(connectionString))
                {
                    dbcon.Open();
                    SqlCommand appendUserToDatabase = new SqlCommand($"INSERT INTO LoginCredentials VALUES ('{pendingUsername}', '{pendingPassphrase}')", dbcon);
                    SqlCommand appendUserRoleToDatabase = new SqlCommand($"INSERT INTO UserLevelAccess VALUES ('{pendingUsername}', '{pendingRole}')", dbcon);
                    appendUserToDatabase.ExecuteScalar();
                    appendUserRoleToDatabase.ExecuteScalar();

                }
                InputOutputAnimationControlClass.UniversalLoadingOuput("Creating new user in progress");
                Console.WriteLine($"User {pendingUsername} has been created successfully. Status : {pendingRole}");
                File.WriteAllLines(newUserRequestPath, new string[] { " " });
                InputOutputAnimationControlClass.ClearScreen();
                ApplicationMenuClass.LoginScreen();
            }
        }

        public static void DeleteUserFromDatabase()
        {
            Console.WriteLine("\r\nChoose a User from the list and proceed to delete");
            Dictionary<string, string> AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
            
            string username = InputOutputAnimationControlClass.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}");
                    username = InputOutputAnimationControlClass.UsernameInput();
                }
                else
                {
                    Console.WriteLine("Cannot delete super_admin! Please choose a different user");
                    username = InputOutputAnimationControlClass.UsernameInput();
                }
            }

            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand deleteUsername = new SqlCommand($"RemoveUsernameFromDatabase @username = '{username}'", dbcon);
                deleteUsername.ExecuteNonQuery();
            }
            InputOutputAnimationControlClass.UniversalLoadingOuput("Deleting existing user in progress");
            Console.WriteLine($"Username {username} has been successfully deleted from database");
            InputOutputAnimationControlClass.ClearScreen();
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
            string pendingUsernameCheck = File.ReadLines(newUserRequestPath).First();

            if (pendingUsernameCheck == " ")
            {
                InputOutputAnimationControlClass.QuasarScreen();
                InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");
                Console.WriteLine("There are no pending User registrations");
                InputOutputAnimationControlClass.QuasarScreen();
                ActiveUserFunctionsClass.ActiveUserProcedures();
                //ApplicationMenuClass.LoginScreen();
            }
            else
            {
                InputOutputAnimationControlClass.QuasarScreen();
                InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");
                Console.WriteLine("You have 1 pending User registration request. Would you like to create new user?");
                InputOutputAnimationControlClass.PromptYesOrNo();
                CreateNewUserFromRequestFunction();
            }
        }

        public static void AlterUserRoleStatus()
        {
            Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
            Dictionary<string, string> AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();

            string username = InputOutputAnimationControlClass.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}");
                    username = InputOutputAnimationControlClass.UsernameInput();
                }
                else
                {
                    Console.WriteLine("Cannot alter super_admin's Status! Please choose a different user");
                    username = InputOutputAnimationControlClass.UsernameInput();
                }
            }
            string userRole = InputOutputAnimationControlClass.SelectUserRole();

            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand alterUserRole = new SqlCommand($"UPDATE UserLevelAccess SET userRole = '{userRole}' WHERE username = '{username}'", dbcon);
                SqlCommand selectUserRole = new SqlCommand($"SELECT userRole FROM UserLevelAccess WHERE username = '{username}'", dbcon);
                alterUserRole.ExecuteScalar();
                string newUserRole = (string)selectUserRole.ExecuteScalar();
                InputOutputAnimationControlClass.UniversalLoadingOuput("Modifying User's role status in progress");
                Console.WriteLine($"Username {username} has been successfully modified as {newUserRole}");
            }
            InputOutputAnimationControlClass.ClearScreen();
            ApplicationMenuClass.LoginScreen();
        }
    }
}
