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
            string login = "Login with your Credentials", register = "New Account request", quit = "Quit Quasar", currentUser = "Not Registered";
            while (true)
            {
                string loginMsg = "Welcome to Quasar! Choose one of the following options";
                string LoginRegisterQuit = SelectMenu.Menu(new List<string> {login, register, quit}, currentUser, loginMsg).NameOfChoice;
                
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
