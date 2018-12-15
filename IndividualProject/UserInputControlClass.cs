using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    static class UserInputControlClass
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
            Console.WriteLine("Press any key to continue or escape to terminate the program");
            ConsoleKey escape = Console.ReadKey().Key;
            return escape;
        }

    }
}
