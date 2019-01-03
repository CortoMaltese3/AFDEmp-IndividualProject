using System.Collections.Generic;

namespace IndividualProject
{
    class Program
    {
        static void Main(string[] args)
        {         
            ApplicationMenu.LoginScreen();
        }
    }

    class ApplicationMenu
    {
        public static void LoginScreen()
        {            
            string login = "Login with your Credentials", register = "New Account request", quit = "Quit Quasar", currentUser = "Not Registered",
                            loginMsg = "\r\nWelcome to Quasar! Choose one of the following options to continue:\r\n";
            while (true)
            { 
                string LoginRegisterQuit = SelectMenu.MenuColumn(new List<string> {login, register, quit}, currentUser, loginMsg).option;
                
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
