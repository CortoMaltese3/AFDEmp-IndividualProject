using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class CreateNewAccountClass
    {
        internal static void CreateNewAccountRequest()
        {
            //string path = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\NewUserRequest.txt";
            try
            {
                Console.WriteLine("\r\nChoose your username and password. Both must be limited to 20 characters");
                string username = UserInputControlClass.UsernameInput();
                string passphrase = UserInputControlClass.PassphraseInput();
                while (CheckUsernameAvailabilityInDatabase(username) == false)
                {
                    username = UserInputControlClass.UsernameInput();
                    passphrase = UserInputControlClass.PassphraseInput();
                }

                CheckUsernameAvailabilityInPendingList(username, passphrase);
            }
            catch (DirectoryNotFoundException d)
            {
                Console.WriteLine(d.Message);
            }
        }

        public static bool CheckUsernameAvailabilityInDatabase(string usernameCheck)
        {
            //TODO : Check if this is a vulnerability
            string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand checkUsername = new SqlCommand($"SELECT COUNT(*) FROM LoginCredentials WHERE (username = '{usernameCheck}')", dbcon);
                int UserCount = (int)checkUsername.ExecuteScalar();
                if (UserCount != 0)
                {
                    Console.WriteLine("This username is already in use. Choose a different one");
                    return false;
                }
                return true;
            }
        }

        public static void CheckUsernameAvailabilityInPendingList(string usernameCheck, string passphraseCheck)
        {
            var path = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\NewUserRequest.txt";
            string pendingUsernameCheck = File.ReadLines(path).First();

            if (pendingUsernameCheck == $"username: {usernameCheck}")
            {
                Console.WriteLine("Your Account Request is Pending. Please wait for the administrator to grant you access.");
            }
            else
            {
                NewUsernameRequestToList(usernameCheck, passphraseCheck);
            }
        }

        public static void NewUsernameRequestToList(string usernameAdd, string passphraseAdd)
        {
            var path = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\NewUserRequest.txt";
            {
                //creates a file and writes collection of string (array) and closes the File - //check why catch fails, it creates a new file if it doesnt exist
                File.WriteAllLines(path, new string[] { $"username: {usernameAdd}", $"passphrase: {passphraseAdd}" });
                Console.WriteLine("New account request is registered. Please wait for the administrator to grant you access.");
            }
        }
    }
}
