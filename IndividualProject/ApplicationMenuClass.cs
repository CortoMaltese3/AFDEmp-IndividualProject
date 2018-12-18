using System;
using System.Linq;
using System.IO;

namespace IndividualProject
{
    class ApplicationMenuClass
    {
        public static void LoginScreen()
        {
            ConsoleKey loginOrRegisterInput = InputOutputAnimationControlClass.LoginScreenOptions();
            while (loginOrRegisterInput != ConsoleKey.Escape)
            {
                switch (loginOrRegisterInput)
                {
                    case ConsoleKey.D1:
                        ActiveUserFunctionsClass.ActiveUserProcedures();

                        break;

                    case ConsoleKey.D2:
                        CreateNewAccountClass.CreateNewAccountRequest();
                        break;
                }
                loginOrRegisterInput = InputOutputAnimationControlClass.LoginScreenOptions();
            }
            ConnectToServerClass.TerminateQuasar();
        }


    }
}
