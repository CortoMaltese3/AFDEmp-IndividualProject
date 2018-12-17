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
            ConsoleKey loginOrRegisterInput = InputOutputControlClass.LoginScreenOptions();
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
                loginOrRegisterInput = InputOutputControlClass.TerminateProgramCommand();
            }

        }

    }
}
