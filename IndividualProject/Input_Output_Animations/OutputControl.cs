using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class OutputControl
    {
        public static string TicketComment()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
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
        public static void QuasarScreen(string currentUser)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            CenterText("Quasar CRM Program - V2.4.1");
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

        public static void WriteAt(int column, int row)
        {
            Console.SetCursorPosition(column, row);
        }

        public static void CenterText(string text)
        {
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth + text.Length) / 2 + "}", text));
        }

        public static void WriteBottomLine(string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 2;
            CenterText(text);
        }
    }
}
