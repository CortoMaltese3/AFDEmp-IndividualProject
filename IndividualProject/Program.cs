using System;
using System.IO;
using System.Media;

namespace IndividualProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //InputOutputAnimationControlClass.BackGroundMusic();
            ApplicationMenuClass.LoginScreen();
        }
    }

    class ApplicationMenuClass
    {
        public static void LoginScreen()
        {
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
