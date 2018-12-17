using System;
using System.Collections.Generic;
using System.Linq;

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
                Console.WriteLine("\r\nPlease choose one of the following user roles : Administrator, Moderator, User");
                pendingRole = Console.ReadLine();
                notInRoleList = roleList.Any(x => x.Contains(pendingRole));
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

        public static ConsoleKey ChooseTechnicalOrComplaintTicket()
        {
            Console.WriteLine("\r\nPress '1' to create a new Technical Issue Ticket or '2' to create a new Complaint Ticket");
            ConsoleKey option = Console.ReadKey().Key;
            while (option != ConsoleKey.D1 && option != ConsoleKey.D2)
            {
                Console.WriteLine("\r\nPress '1' to create a new Technical Issue Ticket or '2' to create a new Complaint Ticket");
                option = Console.ReadKey().Key;
            }
            return option;
        }


        public static string PromptYesOrNo()
        {
            Console.Write("Type 'Y' for yes or 'N' for no :");
            string yesOrNo = Console.ReadLine();
            while 
                (
                    yesOrNo != "Y" && 
                    yesOrNo != "y" && 
                    yesOrNo != "N" && 
                    yesOrNo != "n"
                )
            {
                yesOrNo = Console.ReadLine();
            }
            return yesOrNo;
        }
    }

    static class ConsoleOutputAndAnimations
    {
        public static void AttemptingConnectionToServerOutput()
        {
            Console.Write("Attempting connection to server");
            DotsBlinking();
        }

        public static void CreatingNewUserOutput()
        {
            Console.Write("Creating new user in progress");
            DotsBlinking();
        }

        public static void DeletingExistingUserOutput()
        {
            Console.Write("Deleting existing user in progress");
            DotsBlinking();
        }

        public static void ModifyingExistingUserRoleOutput()
        {
            Console.Write("Modifying User's role status in progress");
            DotsBlinking();
        }

        public static void FilingNewCustomerTicketOutput()
        {
            Console.Write("Filing new customer ticket in progress");
            DotsBlinking();
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
                System.Threading.Thread.Sleep(500);
                Console.SetCursorPosition(Console.CursorLeft + 0, Console.CursorTop + 0);
            }
            Console.WriteLine();
        }

        public static ConsoleKey AdminFunctionOptionsOutput()
        {
            Console.WriteLine("\r\nChoose one of the following functions:");
            Console.WriteLine("1: Check user notifications");
            Console.WriteLine("2: Create new username/password from requests");
            Console.WriteLine("3: Show list of active users");
            Console.WriteLine("4: Upgrade/Downgrade user's role");
            Console.WriteLine("5: Delete an active username from Database");
            Console.WriteLine("6: Create new Customer Ticket");
            Console.WriteLine("7: View the transacted data between users");
            Console.WriteLine("8: Edit the transacted data between users");
            Console.WriteLine("9: Delete the transacted data between users\r\n");
            
            ConsoleKey function = Console.ReadKey().Key;
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
                Console.WriteLine("\r\nChoose one of the following functions:");
                Console.WriteLine("1: Check user notifications");
                Console.WriteLine("2: Create new username/password from requests");
                Console.WriteLine("3: Show list of active users");
                Console.WriteLine("4: Upgrade/Downgrade user's role");
                Console.WriteLine("5: Delete an active username from Database");
                Console.WriteLine("6: Create new Customer Ticket");
                Console.WriteLine("7: View the transacted data between users");
                Console.WriteLine("8: Edit the transacted data between users");
                Console.WriteLine("9: Delete the transacted data between users\r\n");

                function = Console.ReadKey().Key;
            }
            return function;
        }
    }
}