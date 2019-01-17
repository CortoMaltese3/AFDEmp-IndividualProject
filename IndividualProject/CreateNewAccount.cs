using System;
using System.IO;

namespace IndividualProject
{
    class CreateNewAccount
    {
        public static void CreateNewAccountRequest()
        {
            try
            {
                OutputControl.QuasarScreen("Not Registered");
                ColorAndAnimationControl.UniversalLoadingOuput("Please wait");
                Console.Write("Registration Form:\r\nChoose your username and password. Both must be limited to 20 characters");
                string username = InputControl.UsernameInput();
                string passphrase = InputControl.PassphraseInput();                
                var _db = new ConnectToServer();

                while (_db.CheckUsernameAvailabilityInDatabase(username) == false)
                {
                    OutputControl.QuasarScreen("Not Registered");
                    Console.Write("\r\nThis username is already in use. Choose a different one.\r\n(Press any key to continue)");
                    Console.ReadKey();
                    CreateNewAccountRequest();
                }
                CheckUsernameAvailabilityInPendingList(username, passphrase);
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private static void CheckUsernameAvailabilityInPendingList(string usernameCheck, string passphraseCheck)
        {
            var _text = new DataToTextFile();
            string pendingUsernameCheck = _text.GetPendingUsername();

            if (pendingUsernameCheck == $"username: {usernameCheck}")
            {
                OutputControl.QuasarScreen("Not Registered");
                Console.Write("\r\nYour Account Request is Pending. Please wait for the administrator to grant you access.\n\nPress any key to return to Login Screen");
            }
            else
            {
                _text.NewUsernameRequestToList(usernameCheck, passphraseCheck);
                OutputControl.QuasarScreen("Not Registered");
                Console.WriteLine("\r\nNew account request is registered. Please wait for the administrator to grant you access.\n\nPress any key to return to Login Screen");
            }
            Console.ReadKey();
            ApplicationMenu.LoginScreen();
        }
    }
}
