using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class CreateNewAccountClass
    {
        static readonly string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        static readonly string newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";

        internal static void CreateNewAccountRequest()
        {
            try
            {
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                Console.Write("\r\nRegistration Form:\r\nChoose your username and password. Both must be limited to 20 characters");
                string username = InputOutputAnimationControlClass.UsernameInput();
                string passphrase = InputOutputAnimationControlClass.PassphraseInput();
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                InputOutputAnimationControlClass.UniversalLoadingOuput("Check in progress");
                while (CheckUsernameAvailabilityInDatabase(username) == false)
                {
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    Console.Write("\r\nThis username is already in use. Choose a different one");
                    System.Threading.Thread.Sleep(1500);
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    Console.Write("\r\nRegistration Form:\r\nChoose your username and password. Both must be limited to 20 characters");
                    username = InputOutputAnimationControlClass.UsernameInput();
                    passphrase = InputOutputAnimationControlClass.PassphraseInput();
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    InputOutputAnimationControlClass.UniversalLoadingOuput("Check in progress");
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
                SqlCommand checkUsername = new SqlCommand($"SELECT COUNT(*) FROM LoginCredentials WHERE (username = '{usernameCheck}')", dbcon);
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
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                Console.Write("\r\nYour Account Request is Pending. Please wait for the administrator to grant you access.\r\nPress any key to return to Login Screen");
            }
            else
            {
                NewUsernameRequestToList(usernameCheck, passphraseCheck);
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                Console.WriteLine("\r\nNew account request is registered. Please wait for the administrator to grant you access.\r\nPress any key to return to Login Screen");
            }
            Console.ReadKey();
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
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
