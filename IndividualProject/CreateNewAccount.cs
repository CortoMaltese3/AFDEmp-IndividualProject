using System;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class CreateNewAccount
    {              
        internal static void CreateNewAccountRequest()
        {
            string currentUsername = "Not Registered";
            try
            {
                OutputControl.QuasarScreen(currentUsername);
                ColorAndAnimationControl.UniversalLoadingOuput("Please wait");
                Console.Write("Registration Form:\r\nChoose your username and password. Both must be limited to 20 characters");
                string username = InputControl.UsernameInput();
                string passphrase = InputControl.PassphraseInput();
                OutputControl.QuasarScreen(currentUsername);
                ColorAndAnimationControl.UniversalLoadingOuput("Check in progress");
                while (ConnectToServer.CheckUsernameAvailabilityInDatabase(username) == false)
                {
                    OutputControl.QuasarScreen(currentUsername);
                    Console.Write("\r\nThis username is already in use. Choose a different one.\r\n(Press any key to continue)");
                    Console.ReadKey();
                    CreateNewAccountRequest();
                }
                CheckUsernameAvailabilityInPendingList(username, passphrase);
            }
            catch (DirectoryNotFoundException d)
            {
                Console.WriteLine(d.Message);
            }
        }

        private static void CheckUsernameAvailabilityInPendingList(string usernameCheck, string passphraseCheck)
        {
            string currentUsername = "Not Registered";
            string pendingUsernameCheck = File.ReadLines(Globals.newUserRequestPath).First();

            if (pendingUsernameCheck == $"username: {usernameCheck}")
            {
                OutputControl.QuasarScreen(currentUsername);
                Console.Write("\r\nYour Account Request is Pending. Please wait for the administrator to grant you access.\n\nPress any key to return to Login Screen");
            }
            else
            {
                NewUsernameRequestToList(usernameCheck, passphraseCheck);
                OutputControl.QuasarScreen(currentUsername);
                Console.WriteLine("\r\nNew account request is registered. Please wait for the administrator to grant you access.\n\nPress any key to return to Login Screen");
            }
            Console.ReadKey();            
            ApplicationMenu.LoginScreen();
        }

        private static void NewUsernameRequestToList(string usernameAdd, string passphraseAdd)
        {            
            File.WriteAllLines(Globals.newUserRequestPath, new string[] { $"username: {usernameAdd}", $"passphrase: {passphraseAdd}" });
        }
    }
}
