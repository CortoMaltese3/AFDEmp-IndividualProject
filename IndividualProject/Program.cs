using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationMenuClass.LoginScreen();
        }
    }

    class ApplicationMenuClass
    {
        public static void LoginScreen()
        {
            string login = "Login with your Credentials"; string register = "New Account request"; string quit = "Quit Quasar";

            while (true)
            {
                string LoginRegisterQuit = SelectMenu.Menu(new List<string> {login, register, quit}).NameOfChoice;

                if (LoginRegisterQuit == login)
                {
                    ConnectToServer.UserLoginCredentials();
                }

                else if (LoginRegisterQuit == register)
                {
                    CreateNewAccount.CreateNewAccountRequest();                    
                }

                else if (LoginRegisterQuit == quit)
                {
                    ConnectToServer.TerminateQuasar();
                }
            }          
        }
    }
}
