using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class OutputControl
    {
        public void QuasarScreen(string currentUser)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            CenterText("Quasar CRM Program - V2.7.1");
            CenterText("-IT Crowd-");
            CenterText($"[{currentUser}]");
            WriteBottomLine("~CB6 Individual Project~");
            Console.ResetColor();
            WriteAt(0, 3);
        }

        public void UniversalLoadingOutput(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write(message);
            DotsBlinking();
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
            Console.ResetColor();
        }

        public static void DotsBlinking()
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

        public void ColoredText(string text, ConsoleColor frontColor)
        {
            Console.ForegroundColor = frontColor;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Black;
        }

        private static void WriteAt(int column, int row)
        {
            Console.SetCursorPosition(column, row);
        }

        private static void CenterText(string text)
        {
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth + text.Length) / 2 + "}", text));
        }

        private static void WriteBottomLine(string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight - 2;
            CenterText(text);
        }

        public void SpecialThanksMessage()
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

        public string TicketComment()
        {
            var _db = new ConnectToServer();
            var print = new OutputControl();
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Loading");
            Console.Write("EDIT TECHNICAL TICKET");
            Console.WriteLine("\r\nCompile a summary of the Customer's issue (limit 250 characters):");
            string ticketComment = Console.ReadLine();

            while (ticketComment.Length > 250 || ticketComment.Length < 20)
            {
                print.QuasarScreen(currentUsername);
                if (ticketComment.Length > 250)
                {                    
                    Console.WriteLine("\r\nEDIT TECHNICAL TICKET COMMENT SECTION");
                    print.ColoredText("\r\nSummary cannot be longer than 250 characters. Compile a summary of the Customer's issue: ", ConsoleColor.DarkRed);
                    ticketComment = Console.ReadLine();
                }
                if (ticketComment.Length < 20)
                {                    
                    Console.WriteLine("\r\nFILE NEW TECHNICAL TICKET");
                    print.ColoredText("\r\nComment section cannot be shorter than 20 characters. Compile a more extensive summary of the Customer's issue (limit 250 characters): " ,ConsoleColor.DarkRed);
                    ticketComment = Console.ReadLine();
                }
            }
            return ticketComment;
        }

        public int SelectTicketID()
        {
            var print = new OutputControl();
            Console.Write("Select the TicketID of the ticket you want to manage: ");
            while (true)
            {
                try
                {
                    return int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    print.ColoredText("Please choose the ID of one of the Trouble Tickets listed.", ConsoleColor.DarkRed);
                }
            }
        }

        public string SelectUserRole()
        {          
            var _db = new ConnectToServer();

            string administrator = "Administrator";
            string moderator = "Moderator";
            string user = "User";
            string selectionMsg = "\r\nChoose one of the following User Roles:\r\n";
            string currentUser = _db.RetrieveCurrentUserFromDatabase();

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
    }
}
