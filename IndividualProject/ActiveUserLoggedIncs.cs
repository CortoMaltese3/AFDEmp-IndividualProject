using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    class ActiveUserLoggedIncs
    {
        public static void ActiveUserProcedures()
        {
            if (ConnectToServerClass.UserLoginCredentials())
            {
                string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
                string currentUsernameRole = ConnectToServerClass.RetrieveCurrentUsernameRoleFromDatabase();
                switch (currentUsernameRole)
                {
                    case "super_admin":
                        Console.WriteLine("\r\nChoose one of the followin functions:");
                        Console.WriteLine("1: Create new username/password from requests");
                        Console.WriteLine("2: View the transacted data between users");
                        Console.WriteLine("3: Edit the transacted data between users");
                        Console.WriteLine("4: Delete the transacted data between users");
                        Console.WriteLine("5: Delete an active username from Database");
                        ConsoleKey function = Console.ReadKey().Key; 
                        switch (function)
                        {
                            case ConsoleKey.D1:
                                string path = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\NewUserRequest.txt";
                                string pendingUsername = File.ReadLines(path).First();
                                pendingUsername=  pendingUsername.Remove(0, 10);
                                //pendingUsername.Substring(10);

                                string pendingPassphrase = File.ReadLines(path).Skip(1).Take(1).First();
                                //pendingPassphrase.Substring(12);
                                pendingPassphrase = pendingPassphrase.Remove(0, 12);
                                string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";

                                Console.WriteLine($"You are about to create a new username-password entry : {pendingUsername} - {pendingPassphrase}. Please select User's role :");
                                string pendingRole = UserInputControlClass.SelectUserRole();

                                using (SqlConnection dbcon = new SqlConnection(connectionString))
                                {
                                    dbcon.Open();
                                    SqlCommand appendUserToDatabase = new SqlCommand($"INSERT INTO LoginCredentials VALUES ('{pendingUsername}', '{pendingPassphrase}')", dbcon);
                                    SqlCommand appendUserRoleToDatabase = new SqlCommand($"INSERT INTO UserLevelAccess VALUES ('{pendingUsername}', '{pendingRole}')", dbcon);
                                    appendUserToDatabase.ExecuteScalar();
                                    appendUserRoleToDatabase.ExecuteScalar();

                                }

                                Console.WriteLine($"User {pendingUsername} has been created successfully. Status : {pendingRole}");
                                File.WriteAllLines(path, new string[] {" "});
                                UserInputControlClass.ClearScreen();
                                ApplicationMenuClass.LoginScreen();
                                break;

                            case ConsoleKey.D5:
                                Console.WriteLine("select a user to remove from database");
                                SuperAmdin.DeleteUserFromDatabase(UserInputControlClass.UsernameInput());
                                UserInputControlClass.ClearScreen();
                                ApplicationMenuClass.LoginScreen();
                                break;
                        }
                        break;

                    case "administrator":

                        break;

                    case "moderator":

                        break;

                    case "user":

                        break;
                }
            }
        }
    }
}
