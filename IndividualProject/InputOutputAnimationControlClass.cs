using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;

namespace IndividualProject
{
    class InputOutputAnimationControlClass
    {
        public static ConsoleKey LoginScreenOptions()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            QuasarScreen(currentUsername);
            UniversalLoadingOuput("Loading");
            Console.Write("Press '1' to login with your credentials, '2' to create a new account or 'Esc' to exit: ");
            ConsoleKey loginOrRegisterInput = Console.ReadKey().Key;
            if (loginOrRegisterInput == ConsoleKey.Escape)
            {
                return loginOrRegisterInput;
            }
            while (loginOrRegisterInput != ConsoleKey.D1 && loginOrRegisterInput != ConsoleKey.D2)
            {
                QuasarScreen(currentUsername);
                System.Threading.Thread.Sleep(500);
                Console.Write("\r\nPress '1' to login with your credentials, '2' to create a new account or 'Esc' to exit: ");
                loginOrRegisterInput = Console.ReadKey().Key;
                if (loginOrRegisterInput == ConsoleKey.Escape)
                {
                    return loginOrRegisterInput;
                }
            }
            return loginOrRegisterInput;
        }

        public static ConsoleKey ManageTicketOptionsSreen()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            QuasarScreen(currentUsername);
            UniversalLoadingOuput("Loading");
            Console.Write("Press '1' to Open a new ticket, '2' to Close an existing one or 'Esc' to go back to Main Menu: ");
            ConsoleKey openOrCloseTicketInput = Console.ReadKey().Key;
            if (openOrCloseTicketInput == ConsoleKey.Escape)
            {
                return openOrCloseTicketInput;
            }
            while (openOrCloseTicketInput != ConsoleKey.D1 && openOrCloseTicketInput != ConsoleKey.D2)
            {
                QuasarScreen(currentUsername);
                System.Threading.Thread.Sleep(500);
                Console.Write("Press '1' to Open a new ticket, '2' to Close an existing one or 'Esc' to go back to Main Menu: ");
                openOrCloseTicketInput = Console.ReadKey().Key;
                if (openOrCloseTicketInput == ConsoleKey.Escape)
                {
                    return openOrCloseTicketInput;
                }
            }
            return openOrCloseTicketInput;
        }

        public static ConsoleKey ManageTicketOptionsSreenAsUser()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            QuasarScreen(currentUsername);
            UniversalLoadingOuput("Loading");
            Console.Write("Press '1' to Open a new ticket or 'Esc' to go back to Main Menu: ");
            ConsoleKey openOrCloseTicketInput = Console.ReadKey().Key;
            if (openOrCloseTicketInput == ConsoleKey.Escape)
            {
                return openOrCloseTicketInput;
            }
            while (openOrCloseTicketInput != ConsoleKey.D1)
            {
                QuasarScreen(currentUsername);
                System.Threading.Thread.Sleep(500);
                Console.Write("Press '1' to Open a new ticket or 'Esc' to go back to Main Menu: ");
                openOrCloseTicketInput = Console.ReadKey().Key;
                if (openOrCloseTicketInput == ConsoleKey.Escape)
                {
                    return openOrCloseTicketInput;
                }
            }
            return openOrCloseTicketInput;
        }

        public static ConsoleKey EditTicketScreenOptions()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            QuasarScreen(currentUsername);
            UniversalLoadingOuput("Loading");
            Console.Write("Press '1' to edit the Ticket Comments, '2' assign it to a different User or 'Esc' to exit: ");
            ConsoleKey editOrChangeAssignment = Console.ReadKey().Key;
            if (editOrChangeAssignment == ConsoleKey.Escape)
            {
                return editOrChangeAssignment;
            }
            while (editOrChangeAssignment != ConsoleKey.D1 && editOrChangeAssignment != ConsoleKey.D2)
            {
                QuasarScreen(currentUsername);
                System.Threading.Thread.Sleep(500);
                Console.Write("Press '1' to edit the Ticket Comments, '2' assign it to a different User or 'Esc' to exit: ");
                editOrChangeAssignment = Console.ReadKey().Key;
                if (editOrChangeAssignment == ConsoleKey.Escape)
                {
                    return editOrChangeAssignment;
                }
            }
            return editOrChangeAssignment;
        }

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

        public static ConsoleKey AdminFunctionOptionsOutput()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            QuasarScreen(currentUsername);
            UniversalLoadingOuput("Loading");

            Console.WriteLine("Choose one of the following functions or press Esc to return:");
            Console.WriteLine("1: Check user notifications");
            Console.WriteLine("2: Create new username/password from requests");
            Console.WriteLine("3: Show list of active users");
            Console.WriteLine("4: Upgrade/Downgrade user's role");
            Console.WriteLine("5: Delete an active username from Database");
            Console.WriteLine("6: Manage Customer Trouble Tickets");
            Console.WriteLine("7: View Trouble Tickets");
            Console.WriteLine("8: Edit Trouble Tickets");
            Console.WriteLine("9: Delete Trouble Tickets");
            Console.Write("\r\nFunction: ");
            System.Threading.Thread.Sleep(500);
            ConsoleKey function = Console.ReadKey().Key;
            if (function == ConsoleKey.Escape)
            {
                return function;
            }
            while
                (
                    function != ConsoleKey.D1 &&
                    function != ConsoleKey.D2 &&
                    function != ConsoleKey.D3 &&
                    function != ConsoleKey.D4 &&
                    function != ConsoleKey.D5 &&
                    function != ConsoleKey.D6 &&
                    function != ConsoleKey.D7 &&
                    function != ConsoleKey.D8 &&
                    function != ConsoleKey.D9
                )
            {
                QuasarScreen(currentUsername);
                UniversalLoadingOuput("Loading");
                Console.WriteLine("Choose one of the following functions or press Esc to return:");
                Console.WriteLine("1: Check user notifications");
                Console.WriteLine("2: Create new username/password from requests");
                Console.WriteLine("3: Show list of active users");
                Console.WriteLine("4: Upgrade/Downgrade user's role");
                Console.WriteLine("5: Delete an active username from Database");
                Console.WriteLine("6: Manage Customer Trouble Tickets");
                Console.WriteLine("7: View Trouble Tickets");
                Console.WriteLine("8: Edit Trouble Tickets");
                Console.WriteLine("9: Delete Trouble Tickets");
                Console.Write("\r\nFunction: ");
                System.Threading.Thread.Sleep(500);
                function = Console.ReadKey().Key;
                if (function == ConsoleKey.Escape)
                {
                    return function;
                }
            }
            return function;
        }

        public static ConsoleKey AdministratorFunctionOptionsOutput()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();

            Console.WriteLine("\r\nChoose one of the following functions or press Esc to return:");
            Console.WriteLine("1: Check user notifications");
            Console.WriteLine("2: Manage Customer Trouble Tickets");
            Console.WriteLine("3: View Trouble Tickets");
            Console.WriteLine("4: Edit Trouble Tickets");
            Console.WriteLine("5: Delete Trouble Tickets");
            Console.Write("\r\nFunction: ");
            System.Threading.Thread.Sleep(500);
            ConsoleKey function = Console.ReadKey().Key;
            if (function == ConsoleKey.Escape)
            {
                return function;
            }
            while
                (
                    function != ConsoleKey.D1 &&
                    function != ConsoleKey.D2 &&
                    function != ConsoleKey.D3 &&
                    function != ConsoleKey.D4 &&
                    function != ConsoleKey.D5
                )
            {
                QuasarScreen(currentUsername);
                Console.WriteLine("\r\nChoose one of the following functions or press Esc to return:");
                Console.WriteLine("1: Check user notifications");
                Console.WriteLine("2: Manage Customer Trouble Tickets");
                Console.WriteLine("3: View Trouble Tickets");
                Console.WriteLine("4: Edit Trouble Tickets");
                Console.WriteLine("5: Delete Trouble Tickets");
                Console.Write("\r\nFunction: ");
                System.Threading.Thread.Sleep(500);
                function = Console.ReadKey().Key;
                if (function == ConsoleKey.Escape)
                {
                    return function;
                }
            }
            return function;
        }

        public static ConsoleKey ModeratorFunctionOptionsOutput()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();

            Console.WriteLine("\r\nChoose one of the following functions or press Esc to return:");
            Console.WriteLine("1: Check user notifications");
            Console.WriteLine("2: Manage Customer Trouble Tickets");
            Console.WriteLine("3: View Trouble Tickets");
            Console.WriteLine("4: Edit Trouble Tickets");
            Console.Write("\r\nFunction: ");
            System.Threading.Thread.Sleep(500);
            ConsoleKey function = Console.ReadKey().Key;
            if (function == ConsoleKey.Escape)
            {
                return function;
            }
            while
                (
                    function != ConsoleKey.D1 &&
                    function != ConsoleKey.D2 &&
                    function != ConsoleKey.D3 &&
                    function != ConsoleKey.D4 
                )
            {
                QuasarScreen(currentUsername);
                Console.WriteLine("\r\nChoose one of the following functions or press Esc to return:");
                Console.WriteLine("1: Check user notifications");
                Console.WriteLine("2: Manage Customer Trouble Tickets");
                Console.WriteLine("3: View Trouble Tickets");
                Console.WriteLine("4: Edit Trouble Tickets");
                Console.Write("\r\nFunction: ");
                System.Threading.Thread.Sleep(500);
                function = Console.ReadKey().Key;
                if (function == ConsoleKey.Escape)
                {
                    return function;
                }
            }
            return function;
        }

        public static ConsoleKey UserFunctionOptionsOutput()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();

            Console.WriteLine("\r\nChoose one of the following functions or press Esc to return:");
            Console.WriteLine("1: Check user notifications");
            Console.WriteLine("2: Manage Customer Trouble Tickets");
            Console.WriteLine("3: View Trouble Tickets");            
            Console.Write("\r\nFunction: ");
            System.Threading.Thread.Sleep(500);
            ConsoleKey function = Console.ReadKey().Key;
            if (function == ConsoleKey.Escape)
            {
                return function;
            }
            while
                (
                    function != ConsoleKey.D1 &&
                    function != ConsoleKey.D2 &&
                    function != ConsoleKey.D3                    
                )
            {
                QuasarScreen(currentUsername);
                Console.WriteLine("\r\nChoose one of the following functions or press Esc to return:");
                Console.WriteLine("1: Check user notifications");
                Console.WriteLine("2: Manage Customer Trouble Tickets");
                Console.WriteLine("3: View Trouble Tickets");                
                Console.Write("\r\nFunction: ");
                System.Threading.Thread.Sleep(500);
                function = Console.ReadKey().Key;
                if (function == ConsoleKey.Escape)
                {
                    return function;
                }
            }
            return function;
        }

        public static string TicketComment()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            QuasarScreen(currentUsername);
            UniversalLoadingOuput("Loading");
            Console.Write("EDIT TECHNICAL TICKET");
            Console.WriteLine("\r\nCompile a summary of the Customer's issue (limit 500 characters): ");
            string ticketComment = Console.ReadLine();
            while (ticketComment.Length > 500)
            {
                QuasarScreen(currentUsername);
                Console.WriteLine("FILE NEW TECHNICAL TICKET");
                Console.Write("\r\nSummary cannot be longer than 500 characters. Compile a summary of the Customer's issue (limit 500 characters): ");
                ticketComment = Console.ReadLine();
            }
            System.Threading.Thread.Sleep(500);
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
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();

            Console.Write("Please choose one of the following user roles : Administrator, Moderator, User  ->  ");
            string pendingRole = Console.ReadLine();
            List<string> roleList = new List<string>
            {
                "Administrator",
                "Moderator",
                "User"
            };

            bool notInRoleList = roleList.Any(x => x.Contains(pendingRole));

            while (notInRoleList == false || pendingRole == "")
            {
                QuasarScreen(currentUsername);
                Console.Write("\r\nPlease choose one of the following user roles : Administrator, Moderator, User  ->  ");
                pendingRole = Console.ReadLine();
                notInRoleList = roleList.Any(x => x.Contains(pendingRole));
            }
            return pendingRole;
        }

        public static string PromptYesOrNo()
        {
            Console.Write("Type 'Y' for yes or 'N' for no : ");
            string yesOrNo = Console.ReadLine();
            while
                (
                    yesOrNo != "Y" &&
                    yesOrNo != "y" &&
                    yesOrNo != "N" &&
                    yesOrNo != "n"
                )
            {
                Console.Write("Type 'Y' for yes or 'N' for no : ");
                yesOrNo = Console.ReadLine();
            }
            return yesOrNo;
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
                System.Threading.Thread.Sleep(300);
                Console.SetCursorPosition(Console.CursorLeft + 0, Console.CursorTop + 0);
                
            }
            Console.ResetColor();
        }

        public static void WriteBottomLine(string text)
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop + Console.WindowHeight -2;
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
            System.Threading.Thread.Sleep(300);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            CenterText("Quasar CRM Program");
            CenterText("-IT Crowd-");
            CenterText($"[{currentUser}]");
            WriteBottomLine("~CB6 Individual Project~");
            Console.ResetColor();
            WriteAt(0, 3);
        }

        public static void BackGroundMusic()
        {
            SoundPlayer player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "BG music.wav";
            player.Play();
        }
    }
}