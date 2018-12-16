using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    static class InputOutputControlClass
    {
        public static string UsernameInput()
        {

            Console.Write("\r\nusername: ");
            string usernameInput = Console.ReadLine();
            while (usernameInput.Length > 20)
            {
                Console.Write("username cannot be longer than 20 characters");
                usernameInput = Console.ReadLine();
            }
            return usernameInput;
        }

        public static string PassphraseInput()
        {
            Console.Write("passphrase: ");
            string passphraseInput = Console.ReadLine();
            while (passphraseInput.Length > 20)
            {
                Console.Write("passphrase cannot be longer than 20 characters");
                passphraseInput = Console.ReadLine();
            }
            return passphraseInput;
        }

        public static ConsoleKey TerminateProgramCommand()
        {
            //Console.WriteLine("Press any key to continue or escape to terminate the program");
            ConsoleKey escape = Console.ReadKey().Key;
            return escape;
        }

        public static void ClearScreen()
        {
            Console.WriteLine("press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }

        public static string SelectUserRole()
        {
            Console.WriteLine("Please choose one of the following user roles : Administrator, Moderator, User");
            string pendingRole = Console.ReadLine();
            List<string> roleList = new List<string>();
            roleList.Add("Administrator");
            roleList.Add("Moderator");
            roleList.Add("User");

            bool notInRoleList = roleList.Any(x => x.Contains(pendingRole));

            while (notInRoleList == false)
            {
                Console.Write("Please choose one of the following user roles : Administrator, Moderator, User");
                pendingRole = Console.ReadLine();
            }
            return pendingRole;
        }

        public static ConsoleKey LoginScreenOptions()
        {
            Console.WriteLine("Welcome to ITCrowd CRM Program");
            Console.Write("\r\nPress '1' to login with your credentials or '2' to create a new account: ");
            ConsoleKey loginOrRegisterInput = Console.ReadKey().Key;
            if (loginOrRegisterInput == ConsoleKey.Escape)
            {
                return loginOrRegisterInput;
            }
            while (loginOrRegisterInput != ConsoleKey.D1 && loginOrRegisterInput != ConsoleKey.D2)
            {
                Console.Write("\r\nPress '1' to login with your credentials or '2' to create a new account: ");
                loginOrRegisterInput = Console.ReadKey().Key;
            }
            return loginOrRegisterInput;
        }

    }

    static class ConsoleOutputAndAnimations
    {
        public static void LoadingDots()
        {
            Console.WriteLine("Attempting connection to server...");
            //TODO increase sleap time to 3000, maybe try to find dots blinking
            System.Threading.Thread.Sleep(1000);
        }
    }
}
