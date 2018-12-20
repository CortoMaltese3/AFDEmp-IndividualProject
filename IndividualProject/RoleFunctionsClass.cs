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
        static string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();

        public static void CreateNewUserFromRequestFunction()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            string pendingUsername = File.ReadLines(newUserRequestPath).First();
            if (pendingUsername == " ")
            {
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");
                Console.Write("There are no pending requests");
                System.Threading.Thread.Sleep(1500);
                ActiveUserFunctionsClass.ActiveUserProcedures();
            }
            else
            {
                pendingUsername = pendingUsername.Remove(0, 10);

                string pendingPassphrase = File.ReadLines(newUserRequestPath).Skip(1).Take(1).First();
                pendingPassphrase = pendingPassphrase.Remove(0, 12);
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
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
                System.Threading.Thread.Sleep(1500);
                File.WriteAllLines(newUserRequestPath, new string[] {" "});
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                ActiveUserFunctionsClass.ActiveUserProcedures();
            }
        }

        public static void DeleteUserFromDatabase()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
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
            System.Threading.Thread.Sleep(1500);
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            ActiveUserFunctionsClass.ActiveUserProcedures();
        }

        public static Dictionary<string, string> ShowAvailableUsersFromDatabase()
        {
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");
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
                    Console.WriteLine();
                    return AvailableUsernamesDictionary;
                }
            }         
        }

        public static void CheckUserNotifications()
        {
            
        }

        public static void CheckAdminNotifications()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            string pendingUsernameCheck = File.ReadLines(newUserRequestPath).First();

            if (pendingUsernameCheck == " ")
            {
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");
                Console.WriteLine("There are no pending User registrations");
                System.Threading.Thread.Sleep(1500);
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                ActiveUserFunctionsClass.ActiveUserProcedures();
            }
            else
            {
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");
                Console.WriteLine("You have 1 pending User registration request. Would you like to create new user?");
                string option = InputOutputAnimationControlClass.PromptYesOrNo();
                if (option == "Y" || option == "y")
                {
                    CreateNewUserFromRequestFunction();
                }
                else
                {
                    System.Threading.Thread.Sleep(500);
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    ActiveUserFunctionsClass.ActiveUserProcedures();
                }
                
            }
        }

        public static void AlterUserRoleStatus()
        {
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            Dictionary<string, string> AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
            Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");

            string username = InputOutputAnimationControlClass.UsernameInput();
            
            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}");
                    System.Threading.Thread.Sleep(3000);
                    AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
                    Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
                    username = InputOutputAnimationControlClass.UsernameInput();
                }
                else
                {
                    Console.WriteLine("Cannot alter super_admin's Status! Please choose a different user");
                    System.Threading.Thread.Sleep(3000);
                    AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
                    Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
                    username = InputOutputAnimationControlClass.UsernameInput();
                }
            }
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            string userRole = InputOutputAnimationControlClass.SelectUserRole();

            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            //InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");

            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand selectPreviousUserRole = new SqlCommand($"SELECT userRole FROM UserLevelAccess WHERE username = '{username}'", dbcon);
                string previousUserRole = (string)selectPreviousUserRole.ExecuteScalar();
                while (previousUserRole == userRole)
                {
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    Console.WriteLine();
                    Console.WriteLine($"User '{username}' already is {userRole}. Please proceed to choose a different Role Status");
                    System.Threading.Thread.Sleep(3000);
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    userRole = InputOutputAnimationControlClass.SelectUserRole();
                    selectPreviousUserRole = new SqlCommand($"SELECT userRole FROM UserLevelAccess WHERE username = '{username}'", dbcon);
                    previousUserRole = (string)selectPreviousUserRole.ExecuteScalar();
                }

                SqlCommand alterUserRole = new SqlCommand($"UPDATE UserLevelAccess SET userRole = '{userRole}' WHERE username = '{username}'", dbcon);
                SqlCommand selectUserRole = new SqlCommand($"SELECT userRole FROM UserLevelAccess WHERE username = '{username}'", dbcon);
                alterUserRole.ExecuteScalar();
                string newUserRole = (string)selectUserRole.ExecuteScalar();
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                InputOutputAnimationControlClass.UniversalLoadingOuput("Modifying User's role status in progress");
                Console.WriteLine($"Username {username} has been successfully modified as {newUserRole}");
            }
            System.Threading.Thread.Sleep(1500);
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            ActiveUserFunctionsClass.ActiveUserProcedures();
        }
    }
}
