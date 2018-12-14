using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IndividualProject
{
    static class ConnectToServerClass
    {
        public static void UserLoginCredentials()
        {
            string username = UserInputControlClass.UsernameInput();
            string passphrase = UserInputControlClass.PassphraseInput();
            var connectionString = new SqlConnection("Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin");

            byte loginFailCount = 0;
            while (TestConnectionToSqlServer(connectionString))
            {
                if (CheckUsernameAvailabilityInDatabase(username, passphrase))
                {
                    Console.WriteLine($"Connection Established! Weclome back {username}!");
                    return;
                }
                else
                {
                    while (loginFailCount < 2)
                    {
                        Console.WriteLine($"Invalid Username or Passphrase. You have {2 - loginFailCount} attempts available");
                        loginFailCount++;
                        username = UserInputControlClass.UsernameInput();
                        passphrase = UserInputControlClass.PassphraseInput();
                        if (CheckUsernameAvailabilityInDatabase(username, passphrase))
                        {
                            Console.WriteLine($"Connection Established! Weclome back {username}!");
                            return;
                        }
                }
                if (loginFailCount == 2)
                {
                    Console.WriteLine("Failed to login for more than 3 times in a row. Program will now terminate");
                    return;
                }
            }
            }
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

        public static bool CheckUsernameAvailabilityInDatabase(string usernameCheck, string passphraseCheck)
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
    }
}
