using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IndividualProject
{
    class ApplicationMenuClass
    {
        public static void LoginScreen()
        {
            while (UserInputControlClass.TerminateProgramCommand() != ConsoleKey.Escape)
            {
                Console.WriteLine("Welcome to ITCrowd CRM Program");
                Console.Write("Press '1' to login with your credentials or '2' to create a new account: ");
                ConsoleKey loginOrRegisterInput = Console.ReadKey().Key;

                //make input be 1 or 2
                switch (loginOrRegisterInput)
                {
                    case ConsoleKey.D1:
                        if (ConnectToServerClass.UserLoginCredentials())
                        {
                            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
                            if(currentUsername == "admin")
                            {
                                Console.WriteLine("select a user to remove from database");
                                string deleteUser = UserInputControlClass.UsernameInput();
                                SuperAmdin.DeleteUserFromDatabase(deleteUser);
                            }
                        }


                        break;

                    case ConsoleKey.D2:
                        CreateNewAccountClass.CreateNewAccountRequest();
                        break;
                }
            }
            UserInputControlClass.TerminateProgramCommand();

        }

    }
}
