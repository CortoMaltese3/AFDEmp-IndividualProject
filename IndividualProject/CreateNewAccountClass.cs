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
            try
            {
                Console.WriteLine("\r\nChoose your username and password. Both must be limited to 20 characters");
                string username = InputOutputAnimationControlClass.UsernameInput();
                string passphrase = InputOutputAnimationControlClass.PassphraseInput();
                while (CheckUsernameAvailabilityInDatabase(username) == false)
                {
                    username = InputOutputAnimationControlClass.UsernameInput();
                    passphrase = InputOutputAnimationControlClass.PassphraseInput();
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
            var newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";
            string pendingUsernameCheck = File.ReadLines(newUserRequestPath).First();

            if (pendingUsernameCheck == $"username: {usernameCheck}")
            {
                Console.WriteLine("Your Account Request is Pending. Please wait for the administrator to grant you access.");
                InputOutputAnimationControlClass.ClearScreen();
                ApplicationMenuClass.LoginScreen();
            }
            else
            {
                NewUsernameRequestToList(usernameCheck, passphraseCheck);
                InputOutputAnimationControlClass.ClearScreen();
                ApplicationMenuClass.LoginScreen();
            }
        }

        public static void NewUsernameRequestToList(string usernameAdd, string passphraseAdd)
        {
            var newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";
            {
                //creates a file and writes collection of string (array) and closes the File - //check why catch fails, it creates a new file if it doesnt exist
                File.WriteAllLines(newUserRequestPath, new string[] { $"username: {usernameAdd}", $"passphrase: {passphraseAdd}" });
                Console.WriteLine("New account request is registered. Please wait for the administrator to grant you access.");
            }
        }
    }
}
