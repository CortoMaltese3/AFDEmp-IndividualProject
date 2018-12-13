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

        public static bool UsernameAndPassphraseCheck()
        {
            Console.WriteLine("Press '1' to login with your credentials or '2' to create a new account: ");
            ConsoleKey loginOrRegisterInput = Console.ReadKey().Key;

            switch (loginOrRegisterInput)
            {
                case ConsoleKey.D1:

                    break;


            }

            
        }

        public static void 


        public static void check()
        {
            //TODO : Check if server must be localhost
            var connection = new SqlConnection($"Server=localhost; Database = Project1_Individual; User Id = {username}; Password = {passphrase}");
            if (connection.SqlServerAvailable())
            {
                Console.WriteLine("Connection Established!");
            }
        }
    }
}
