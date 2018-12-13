using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    static class ConnectToServerClass
    {
        //public static void LoginOrRegisterAccount()
        //{
        //    Console.Write("Press '1' to login with your credentials or '2' to create a new account: ");
        //    ConsoleKey loginOrRegisterInput = Console.ReadKey().Key;

        //    switch (loginOrRegisterInput)
        //    {
        //        case ConsoleKey.D1:
        //            UserLoginCredentials();
        //            break;

        //        case ConsoleKey.D2:

        //            break;
        //    }
        //}
        public static void UserLoginCredentials()
        {
            string username = UsernameInput();
            string passphrase = PassphraseInput();
            var connectionString = new SqlConnection($"Server=localhost; Database = Project1_Individual; User Id = {username}; Password = {passphrase}");
            byte loginFailCount = 0;
            while (loginFailCount < 2)
            {
                if (TestConnectionToSqlServer(connectionString))
                {
                    Console.WriteLine($"Connection Established! Weclome back {username}!");
                    return;
                }
                else
                {
                    Console.WriteLine($"You have {2 - loginFailCount} attempts available");
                    loginFailCount++;
                    username = UsernameInput();
                    passphrase = PassphraseInput();
                    connectionString = new SqlConnection($"Server=localhost; Database = Project1_Individual; User Id = {username}; Password = {passphrase}");
                }
            }
            if (loginFailCount == 2)
            {
                Console.WriteLine("Failed to login for more than 3 times in a row. Program will now terminate");
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

        public static string UsernameInput()
        {
            Console.Write("\r\nusername: ");
            string usernameInput = Console.ReadLine();
            while (usernameInput.Length > 20)
            {
                Console.Write("username cannot be longer than 20 characters");
                usernameInput = Console.ReadLine();
            }
            return usernameInput;
        }

        public static string PassphraseInput()
        {
            Console.Write("passphrase: ");
            string passphraseInput = Console.ReadLine();
            while (passphraseInput.Length > 20)
            {
                Console.Write("passphrase cannot be longer than 20 characters");
                passphraseInput = Console.ReadLine();
            }
            return passphraseInput;
        }
    }
}
