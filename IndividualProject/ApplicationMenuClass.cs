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
            Console.WriteLine("Welcome to ITCrowd CRM Program");
            Console.Write("Press '1' to login with your credentials or '2' to create a new account: ");
            ConsoleKey loginOrRegisterInput = Console.ReadKey().Key;
            while (loginOrRegisterInput != ConsoleKey.Escape)
            {
                //make input be 1 or 2
                switch (loginOrRegisterInput)
                {
                    case ConsoleKey.D1:
                        ActiveUserLoggedIncs.ActiveUserProcedures();

                        break;



                    case ConsoleKey.D2:
                        CreateNewAccountClass.CreateNewAccountRequest();
                        break;
                }
                loginOrRegisterInput = UserInputControlClass.TerminateProgramCommand();
            }

        }

    }
}
