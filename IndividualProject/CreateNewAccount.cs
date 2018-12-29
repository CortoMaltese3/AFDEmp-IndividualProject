using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class CreateNewAccount
    {
        static readonly string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        static readonly string newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";

        internal static void CreateNewAccountRequest()
        {
            try
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Please wait");
                Console.Write("Registration Form:\r\nChoose your username and password. Both must be limited to 20 characters");
                string username = InputOutputAnimationControl.UsernameInput();
                string passphrase = InputOutputAnimationControl.PassphraseInput();
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Check in progress");
                while (CheckUsernameAvailabilityInDatabase(username) == false)
                {
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    Console.Write("\r\nThis username is already in use. Choose a different one");
                    System.Threading.Thread.Sleep(1000);
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    Console.Write("\r\nRegistration Form:\r\nChoose your username and password. Both must be limited to 20 characters");
                    username = InputOutputAnimationControl.UsernameInput();
                    passphrase = InputOutputAnimationControl.PassphraseInput();
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    InputOutputAnimationControl.UniversalLoadingOuput("Check in progress");
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
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand checkUsername = new SqlCommand($"EXECUTE CheckUniqueUsername '{usernameCheck}')", dbcon);
                int UserCount = (int)checkUsername.ExecuteScalar();
                if (UserCount != 0)
                {
                    return false;
                }
                return true;
            }
        }

        public static void CheckUsernameAvailabilityInPendingList(string usernameCheck, string passphraseCheck)
        {
            
            string pendingUsernameCheck = File.ReadLines(newUserRequestPath).First();

            if (pendingUsernameCheck == $"username: {usernameCheck}")
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                Console.Write("\r\nYour Account Request is Pending. Please wait for the administrator to grant you access.\r\nPress any key to return to Login Screen");
            }
            else
            {
                NewUsernameRequestToList(usernameCheck, passphraseCheck);
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                Console.WriteLine("\r\nNew account request is registered. Please wait for the administrator to grant you access.\r\nPress any key to return to Login Screen");
            }
            Console.ReadKey();
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            ApplicationMenuClass.LoginScreen();
        }

        public static void NewUsernameRequestToList(string usernameAdd, string passphraseAdd)
        {
            var newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";
            {
                //TODO creates a file and writes collection of string (array) and closes the File - //check why catch fails, it creates a new file if it doesnt exist
                File.WriteAllLines(newUserRequestPath, new string[] { $"username: {usernameAdd}", $"passphrase: {passphraseAdd}" });
            }
        }
    }
}
