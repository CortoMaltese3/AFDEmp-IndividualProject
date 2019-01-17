using System;
using System.IO;

namespace IndividualProject
{
    class CreateNewAccount
    {
        private static OutputControl print = new OutputControl();

        public static void CreateNewAccountRequest()
        {
            try
            {
                print.QuasarScreen("Not Registered");
                print.UniversalLoadingOutput("Please wait");
                Console.Write("Registration Form:\r\nChoose your username and password. Both must be limited to 20 characters\r\n");
                string username = InputControl.UsernameInput();
                string passphrase = InputControl.PassphraseInput();                
                var _db = new ConnectToServer();

                while (_db.CheckUsernameAvailabilityInDatabase(username) == false)
                {
                    print.QuasarScreen("Not Registered");
                    print.ColoredText("\r\nThis username is already in use. Choose a different one.\r\n(Press any key to continue)", ConsoleColor.DarkRed);
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
                print.QuasarScreen("Not Registered");
                print.ColoredText("\r\nYour Account Request is Pending. Please wait for the administrator to grant you access.\n\nPress any key to return to Login Screen", ConsoleColor.DarkGreen);
            }
            else
            {
                _text.NewUsernameRequestToList(usernameCheck, passphraseCheck);
                print.QuasarScreen("Not Registered");
                print.ColoredText("\r\nNew account request is registered. Please wait for the administrator to grant you access.\n\nPress any key to return to Login Screen", ConsoleColor.DarkGreen);
            }
            Console.ReadKey();
            ApplicationMenu.LoginScreen();
        }
    }
}
