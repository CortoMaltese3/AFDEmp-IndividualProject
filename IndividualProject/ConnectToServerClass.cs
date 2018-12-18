using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IndividualProject
{
    static class ConnectToServerClass
    {
        //static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        //static readonly string newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";

        public static bool UserLoginCredentials()
        {
            string username = InputOutputAnimationControlClass.UsernameInput();
            string passphrase = InputOutputAnimationControlClass.PassphraseInput();
            
           var connectionString = new SqlConnection("Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin");
            
            while (TestConnectionToSqlServer(connectionString))
            {
                if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                {
                    StoreCurrentLoginCredentialsToDatabase(username, passphrase);
                    Console.WriteLine($"Connection Established! Welcome back {username}!");
                    System.Threading.Thread.Sleep(1000);
                    return true;
                }
                else
                {
                    while (true)
                    {
                        InputOutputAnimationControlClass.QuasarScreen("Not registered");
                        Console.WriteLine();
                        Console.Write($"Invalid Username or Passphrase. Try again.");
                        username = InputOutputAnimationControlClass.UsernameInput();
                        passphrase = InputOutputAnimationControlClass.PassphraseInput();
                        InputOutputAnimationControlClass.UniversalLoadingOuput("Attempting connection to server");
                        if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                        {
                            StoreCurrentLoginCredentialsToDatabase(username, passphrase);
                            Console.WriteLine($"Connection Established! Welcome back {username}!");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool TestConnectionToSqlServer(this SqlConnection connectionString)
        {
            InputOutputAnimationControlClass.UniversalLoadingOuput("Attempting connection to server");
            try
            {
                connectionString.Open();
                connectionString.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public static bool CheckUsernameAndPasswordMatchInDatabase(string usernameCheck, string passphraseCheck)
        {
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand checkUsername = new SqlCommand($"SELECT COUNT(*) FROM LoginCredentials " +
                    $"                                      WHERE (username = '{usernameCheck}' " +
                    $"                                      AND passphrase = '{passphraseCheck}')", dbcon);
                int UserCount = (int)checkUsername.ExecuteScalar();
                if (UserCount != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static void StoreCurrentLoginCredentialsToDatabase(string currentUsername, string currentPassphrase)
        {
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand StoreLoginCredentials = new SqlCommand($"UPDATE CurrentLoginCredentials SET username = '{currentUsername}', passphrase = '{currentPassphrase}', currentStatus = 'active'", dbcon);
                StoreLoginCredentials.ExecuteScalar();
            }
        }

        public static string RetrieveCurrentLoginCredentialsFromDatabase()
        {
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveLoginCredentials = new SqlCommand($"SELECT username FROM CurrentLoginCredentials", dbcon);
                string currentUsername = (string)RetrieveLoginCredentials.ExecuteScalar();
                return currentUsername;
            }
        }

        public static string RetrieveCurrentUsernameRoleFromDatabase()
        {
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveCurrentUsernameRole = new SqlCommand($"SELECT userRole FROM UserLevelAccess u INNER JOIN CurrentLoginCredentials c ON c.username = u.username", dbcon);
                string currentRole = (string)RetrieveCurrentUsernameRole.ExecuteScalar();
                return currentRole;
            }
        }

        public static string RetrieveCurrentUserStatusFromDatabase()
        {
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveCurrentUserStatus = new SqlCommand($"SELECT currentStatus FROM CurrentLoginCredentials", dbcon);
                string currentUserStatus = (string)RetrieveCurrentUserStatus.ExecuteScalar();
                return currentUserStatus;
            }
        }

        public static void TerminateQuasar()
        {
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand SetStatusToInnactive = new SqlCommand($"UPDATE CurrentLoginCredentials SET username = 'Not Registered', currentStatus = 'inactive'", dbcon);
                SetStatusToInnactive.ExecuteScalar();
            }
            Console.WriteLine();
            Console.WriteLine("Wait for Quasar to shut down");
            InputOutputAnimationControlClass.UniversalLoadingOuput("Terminating");
        }
    }
}
