using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class CreateNewAccount
    {              
        static readonly string currentUsername = "Not Registered";

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
                    CreateNewAccountRequest();
                }

                CheckUsernameAvailabilityInPendingList(username, passphrase);
            }
            catch (DirectoryNotFoundException d)
            {
                Console.WriteLine(d.Message);
            }
        }

        private static bool CheckUsernameAvailabilityInDatabase(string usernameCheck)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand checkUsername = new SqlCommand($"EXECUTE CheckUniqueUsername '{usernameCheck}'", dbcon);
                int UserCount = (int)checkUsername.ExecuteScalar();
                if (UserCount != 0)
                {
                    return false;
                }
                return true;
            }
        }

        private static void CheckUsernameAvailabilityInPendingList(string usernameCheck, string passphraseCheck)
        {
            
            string pendingUsernameCheck = File.ReadLines(Globals.newUserRequestPath).First();

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

        private static void NewUsernameRequestToList(string usernameAdd, string passphraseAdd)
        {            
            {                
                File.WriteAllLines(Globals.newUserRequestPath, new string[] { $"username: {usernameAdd}", $"passphrase: {passphraseAdd}" });
            }
        }
    }
}
