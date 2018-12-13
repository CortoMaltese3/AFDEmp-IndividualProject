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
        public static void UserInputCredentials()
        {
            Console.Write("username: ");
            string usernameInput = Console.ReadLine();
            while (usernameInput.Length > 20)
            {
                Console.Write("username cannot be longer than 20 characters");
                usernameInput = Console.ReadLine();
            }

            Console.WriteLine("passphrase: ");
            string passphraseInput = Console.ReadLine();
            while (passphraseInput.Length > 20)
            {
                Console.WriteLine("passphrase cannot be longer than 20 characters");
                passphraseInput = Console.ReadLine();
            }

            if (usernameInput == "admin" && passphraseInput == "admin")
            {
                TestConnectionToServer();
            }
        }

        public static bool SqlServerAvailable(this SqlConnection connection)
        {
            Console.WriteLine("Attempting connection to server...");
            System.Threading.Thread.Sleep(2000);
            try
            {
                connection.Open();
                connection.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public static void TestConnectionToServer()
        {
            //TODO : Check if server must be localhost
            var connection = new SqlConnection($"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin");
            if (connection.SqlServerAvailable())
            {
                Console.WriteLine("Connection Established!");
            }
        }
    }
}
