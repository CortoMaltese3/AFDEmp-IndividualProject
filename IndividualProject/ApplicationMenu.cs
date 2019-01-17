using System.Collections.Generic;

namespace IndividualProject
{
    class ApplicationMenu
    {
        public static void LoginScreen()
        {
            var _db = new ConnectToServer();

            string login = "Login with your Credentials";
            string register = "New Account request";
            string quit = "Quit Quasar";
            string currentUser = "Not Registered";
            string loginMsg = "\r\nWelcome to Quasar! Choose one of the following options to continue:\r\n";

            while (true)
            {
                string LoginRegisterQuit = SelectMenu.MenuColumn(new List<string> { login, register, quit }, currentUser, loginMsg).option;

                if (LoginRegisterQuit == login)
                {
                    _db.UserLoginCredentials();
                }
                else if (LoginRegisterQuit == register)
                {
                    CreateNewAccount.CreateNewAccountRequest();
                }
                else if (LoginRegisterQuit == quit)
                {
                    _db.TerminateQuasar();
                }
            }
        }
    }
}