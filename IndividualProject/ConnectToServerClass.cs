using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IndividualProject
{
    static class ConnectToServerClass
    {
        public static bool UserLoginCredentials()
        {
            string username = UserInputControlClass.UsernameInput();
            string passphrase = UserInputControlClass.PassphraseInput();
            
            var connectionString = new SqlConnection("Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin");
            
            while (TestConnectionToSqlServer(connectionString))
            {
                if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                {
                    StoreCurrentLoginCredentialsToDatabase(username, passphrase);
                    Console.WriteLine($"Connection Established! Welcome back {username}!");

                    
                    return true;
                }
                else
                {
                    while (true)
                    {
                        Console.WriteLine($"Invalid Username or Passphrase. Try again.");
                        username = UserInputControlClass.UsernameInput();
                        passphrase = UserInputControlClass.PassphraseInput();
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
            Console.WriteLine("Attempting connection to server...");
            //TODO increase sleap time to 3000, maybe try to find dots blinking
            System.Threading.Thread.Sleep(1000);
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
            //TODO : Check if this is a vulnerability
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

        static void StoreCurrentLoginCredentialsToDatabase(string currentUsername, string currentPassphrase)
        {
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand StoreLoginCredentials = new SqlCommand($"UPDATE CurrentLoginCredentials SET username = '{currentUsername}', passphrase = '{currentPassphrase}'", dbcon);
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
    }
}
