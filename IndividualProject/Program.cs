using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.CancelKeyPress += new ConsoleCancelEventHandler(ActiveUserFunctionsClass.MainMenuScreen);
            //InputOutputAnimationControlClass.BackGroundMusic();

            ApplicationMenuClass.LoginScreen();
        }
    }

    class ApplicationMenuClass
    {
        public static void LoginScreen()
        {
            const string login = "Login with your Credentials", register = "New Account request", quit = "Quit Quasar";

            while (true)
            {
                string LoginRegisterQuit = SelectMenu.Menu(new List<string> {login, register, quit}).NameOfChoice;

                if (LoginRegisterQuit == register)
                {
                    CreateNewAccountClass.CreateNewAccountRequest();                    
                }
                else if (LoginRegisterQuit == login)
                {
                    ActiveUserFunctionsClass.ActiveUserProcedures();
                }

                else if (LoginRegisterQuit == quit)
                {
                    ConnectToServerClass.TerminateQuasar();
                }
            }

            


            ConsoleKey loginOrRegisterInput = InputOutputAnimationControlClass.LoginScreenOptions();
            switch (loginOrRegisterInput)
            {
                case ConsoleKey.D1:
                    ActiveUserFunctionsClass.ActiveUserProcedures();
                    break;

                case ConsoleKey.D2:
                    CreateNewAccountClass.CreateNewAccountRequest();
                    break;

                case ConsoleKey.Escape:
                    ConnectToServerClass.TerminateQuasar();
                    break;
            }
        }
    }


}
