using System;

namespace IndividualProject
{
    class InputControl
    {
        private static OutputControl print = new OutputControl();

        public static string UsernameInput()
        {
            Console.Write("\r\nusername: ");
            string usernameInput = Console.ReadLine();
            while (usernameInput.Length > 20)
            {
                print.QuasarScreen("Not registered");
                Console.WriteLine("\r\nusername cannot be longer than 20 characters. Please try again");
                Console.Write("username: ");
                usernameInput = Console.ReadLine();
            }
            return usernameInput;
        }

        public static string PassphraseInput()
        {
            Console.Write("passphrase: ");
            string passphrase = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    passphrase += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && passphrase.Length > 0)
                    {
                        passphrase = passphrase.Substring(0, (passphrase.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        break;
                    }
                }
            } while (true);

            while (passphrase.Length > 20)
            {
                print.QuasarScreen("Not registered");
                Console.WriteLine("\r\npassphrase cannot be longer than 20 characters. Please try again");
                Console.Write("passphrase: ");
                passphrase = Console.ReadLine();
            }
            return passphrase;
        }
    }
}
