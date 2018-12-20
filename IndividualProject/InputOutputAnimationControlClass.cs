using System;
using System.Collections.Generic;
using System.Linq;

namespace IndividualProject
{
    class InputOutputAnimationControlClass
    {
        static int origRow;
        static int origCol;
        //static readonly string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();

        public static ConsoleKey LoginScreenOptions()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            QuasarScreen(currentUsername);
            UniversalLoadingOuput("Loading");
            Console.Write("Press '1' to login with your credentials or '2' to create a new account: ");
            ConsoleKey loginOrRegisterInput = Console.ReadKey().Key;
            if (loginOrRegisterInput == ConsoleKey.Escape)
            {
                return loginOrRegisterInput;
            }
            while (loginOrRegisterInput != ConsoleKey.D1 && loginOrRegisterInput != ConsoleKey.D2)
            {
                QuasarScreen(currentUsername);
                System.Threading.Thread.Sleep(500);
                Console.Write("\r\nPress '1' to login with your credentials or '2' to create a new account: ");
                loginOrRegisterInput = Console.ReadKey().Key;
                if (loginOrRegisterInput == ConsoleKey.Escape)
                {
                    return loginOrRegisterInput;
                }
            }
            return loginOrRegisterInput;
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

            Console.WriteLine("\r\nChoose one of the following functions or press Esc to return:");
            Console.WriteLine("1: Check user notifications");
            Console.WriteLine("2: Create new username/password from requests");
            Console.WriteLine("3: Show list of active users");
            Console.WriteLine("4: Upgrade/Downgrade user's role");
            Console.WriteLine("5: Delete an active username from Database");
            Console.WriteLine("6: Manage Customer Trouble Tickets");
            Console.WriteLine("7: View the transacted data between users");
            Console.WriteLine("8: Edit the transacted data between users");
            Console.WriteLine("9: Delete the transacted data between users");
            Console.Write("\r\nFunction: ");
            System.Threading.Thread.Sleep(500);
            ConsoleKey function = Console.ReadKey().Key;
            if (function == ConsoleKey.Escape)
            {
                return function;
            }
            //CheckWhetherInputIsEscapeToGoBack(function, currentUsername);
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
                Console.WriteLine("\r\nChoose one of the following functions or press Esc to return:");
                Console.WriteLine("1: Check user notifications");
                Console.WriteLine("2: Create new username/password from requests");
                Console.WriteLine("3: Show list of active users");
                Console.WriteLine("4: Upgrade/Downgrade user's role");
                Console.WriteLine("5: Delete an active username from Database");
                Console.WriteLine("6: Manage Customer Trouble Tickets");
                Console.WriteLine("7: View the transacted data between users");
                Console.WriteLine("8: Edit the transacted data between users");
                Console.WriteLine("9: Delete the transacted data between users");
                Console.Write("\r\nFunction: ");
                System.Threading.Thread.Sleep(500);
                function = Console.ReadKey().Key;
                if (function == ConsoleKey.Escape)
                {
                    return function;
                }
                //CheckWhetherInputIsEscapeToGoBack(function, currentUsername);
            }
            return function;
        }

        public static string TicketComment()
        {
            Console.Write("Compile a summary of the Customer's issue (limit 500 characters): ");
            string ticketComment = Console.ReadLine();
            while (ticketComment.Length > 500)
            {
                Console.Write("Summary cannot be longer than 500 characters. Compile a summary of the Customer's issue (limit 500 characters): ");
                ticketComment = Console.ReadLine();
            }
            return ticketComment;
        }

        public static int SelectTicketID()
        {
            Console.Write("Select the TicketID of the ticket you want to mark as Closed: ");
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

        //public static void ClearScreen()
        //{
        //    Console.WriteLine("press any key to continue");
        //    Console.ReadKey();
        //    Console.Clear();
        //}

        public static string SelectUserRole()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();

            Console.Write("\r\nPlease choose one of the following user roles : Administrator, Moderator, User  ->  ");
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
            Console.Write(message);
            DotsBlinking();
            Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
        }

        public static void WriteAt(int x, int y)
        {
            Console.SetCursorPosition(origCol + x, origRow + y);
        }

        public static void DotsBlinking()
        {
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
                System.Threading.Thread.Sleep(400);
                Console.SetCursorPosition(Console.CursorLeft + 0, Console.CursorTop + 0);
            }
        }



        public static void CenterText(string text)
        {
            Console.WriteLine(string.Format("{0," + (Console.WindowWidth + text.Length) / 2 + "}", text));
        }

        public static void QuasarScreen(string currentUser)
        {
            System.Threading.Thread.Sleep(500);
            Console.Clear();
            CenterText("Welcome to Quasar CRM Program");
            CenterText("-IT Crowd-");
            CenterText($"[{currentUser}]");
        }

        public static void CheckWhetherInputIsEscapeToGoBack(ConsoleKey consoloKey, string currentUsername)
        {
            if (consoloKey == ConsoleKey.Escape)
            {
                QuasarScreen(currentUsername);
                Console.WriteLine("\r\nWould you like to go back? ");
                string option = PromptYesOrNo();
                if (option == "y" || option == "Y")
                {
                    QuasarScreen(currentUsername);
                    ConnectToServerClass.ChangeCurrentUserStatusToInactive();
                    ApplicationMenuClass.LoginScreen();
                }
                else
                {
                    QuasarScreen(currentUsername);
                    ActiveUserFunctionsClass.ActiveUserProcedures();
                }
            }
            return;
        }
    }
}