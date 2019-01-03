using System;
using System.Collections.Generic;
using System.Linq;

namespace IndividualProject
{
    class InputOutputAnimationControl
    {
        public static string UsernameInput()
        {
            Console.Write("\r\nusername: ");
            string usernameInput = Console.ReadLine();
            while (usernameInput.Length > 20)
            {
                QuasarScreen("Not registered");
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
                QuasarScreen("Not registered");
                Console.WriteLine("\r\npassphrase cannot be longer than 20 characters. Please try again");
                Console.Write("passphrase: ");
                passphrase = Console.ReadLine();

            }
            return passphrase;
        }

        public static string TicketComment()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            QuasarScreen(currentUsername);
            UniversalLoadingOuput("Loading");
            Console.Write("EDIT TECHNICAL TICKET");
            Console.WriteLine("\r\nCompile a summary of the Customer's issue (limit 500 characters):");
            string ticketComment = Console.ReadLine();

            while (ticketComment.Length > 500 || ticketComment.Length < 20)
            {
                if (ticketComment.Length > 500)
                {
                    QuasarScreen(currentUsername);
                    Console.WriteLine("EDIT TECHNICAL TICKET COMMENT SECTION");
                    Console.WriteLine("\r\nSummary cannot be longer than 500 characters. Compile a summary of the Customer's issue (limit 500 characters): ");
                    ticketComment = Console.ReadLine();
                }
                if (ticketComment.Length < 20)
                {
                    QuasarScreen(currentUsername);
                    Console.WriteLine("FILE NEW TECHNICAL TICKET");
                    Console.WriteLine("\r\nComment section cannot be shorter than 20 characters. Compile a more extensive summary of the Customer's issue (limit 500 characters): ");
                    ticketComment = Console.ReadLine();
                }
            }
            return ticketComment;
        }

        public static int SelectTicketID()
        {
            Console.Write("Select the TicketID of the ticket you want to manage: ");
            while (true)
            {
                try
                {
                    return int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Input needs to be a real number greater than 0");
                }
            }
        }

        public static string SelectUserRole()
        {
            string administrator = "Administrator", moderator = "Moderator", user = "User",
                   selectionMsg = "\r\nChoose one of the following User Roles:\r\n", currentUser = ConnectToServer.RetrieveCurrentUserFromDatabase();
            while (true)
            {
                string SelectUserRoleFromList = SelectMenu.MenuColumn(new List<string> { administrator, moderator, user }, currentUser, selectionMsg).option;

                if (SelectUserRoleFromList == administrator)
                {
                    return administrator;
                }
                else if (SelectUserRoleFromList == moderator)
                {
                    return moderator;
                }
                else if (SelectUserRoleFromList == user)
                {
                    return user;
                }
            }
        }

        public static void UniversalLoadingOuput(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(message);
            DotsBlinking();
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            Console.ResetColor();
        }

        protected static void DotsBlinking()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            for (int blink = 0; blink < 5; blink++)
            {
                switch (blink)
                {
                    case 0: Console.Write("."); break;
                    case 1: Console.Write("."); break;
                    case 2: Console.Write("."); break;
                    case 3: Console.Write("."); break;
                    case 4: Console.Write("."); break;
                }
                System.Threading.Thread.Sleep(200);
                Console.SetCursorPosition(Console.CursorLeft + 0, Console.CursorTop + 0);
            }
            Console.ResetColor();
        }

        public static void WriteBottomLine(string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 2;
            CenterText(text);
        }

        private static void WriteAt(int column, int row)
        {
            Console.SetCursorPosition(column, row);
        }

        private static void CenterText(string text)
        {
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth + text.Length) / 2 + "}", text));
        }

        public static void QuasarScreen(string currentUser)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            CenterText("Quasar CRM Program - V2.1");
            CenterText("-IT Crowd-");
            CenterText($"[{currentUser}]");
            WriteBottomLine("~CB6 Individual Project~");
            Console.ResetColor();
            WriteAt(0, 3);
        }

        public static void SpecialThanksMessage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int blink = 0; blink < 6; blink++)
            {
                if (blink % 2 == 0)
                {
                    WriteBottomLine("~~~~~Special thanks to Afro~~~~~");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    System.Threading.Thread.Sleep(300);
                }
                else
                {
                    WriteBottomLine("~~~~~Special thanks to Afro~~~~~");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    System.Threading.Thread.Sleep(300);
                }
            }
        }
    }
}