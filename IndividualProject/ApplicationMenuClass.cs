using System;

namespace IndividualProject
{
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
                    InputOutputAnimationControlClass.CheckWhetherInputIsEscapeToGoTerminate();
                    break;                     
            }
        }
    }
}
