using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IndividualProject
{
    class TransactedDataClass
    {
        static readonly string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        static readonly string newTechnicalIssuePath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\TechnicalIssues";

        public static void ManageCustomerTickets()
        {
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            //InputOutputAnimationControlClass.UniversalLoadingOuput("Loading");
            
            ConsoleKey option = InputOutputAnimationControlClass.ManageTicketOptionsSreen();
            switch (option)
            {
                case ConsoleKey.D1:
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    OpenNewCustomerTicket();
                    break;

                case ConsoleKey.D2:
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    CloseCustomerTicket();
                    break;

                case ConsoleKey.Escape:
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    ActiveUserFunctionsClass.ActiveUserProcedures();
                    break;
            }



        }

        public static string AssignTicketToUser()
        {
            Console.WriteLine("\r\nWould you like to assign the ticket to another?");
            string option = InputOutputAnimationControlClass.PromptYesOrNo();
            if (option == "Y" || option == "y")
            {
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);

                Dictionary<string, string> AvailableUsernamesDictionary = RoleFunctionsClass.ShowAvailableUsersFromDatabase();
                Console.Write("\r\nPlease select a user and proceed to assign: ");
                string usernameAssignment = InputOutputAnimationControlClass.UsernameInput();

                while (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false || usernameAssignment == "admin")
                {
                    if (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false)
                    {
                        Console.WriteLine($"Database does not contain a User {usernameAssignment}");
                        System.Threading.Thread.Sleep(2000);
                        InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = RoleFunctionsClass.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputOutputAnimationControlClass.UsernameInput();
                    }
                    else
                    {
                        Console.WriteLine("Cannot assign ticket to super_admin! Please choose a different user");
                        System.Threading.Thread.Sleep(2000);
                        InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = RoleFunctionsClass.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputOutputAnimationControlClass.UsernameInput();
                    }
                }
                return usernameAssignment;
            }
            return currentUsername;
        }

        public static void OpenNewCustomerTicket()
        {
            string comment = InputOutputAnimationControlClass.TicketComment();
            string userAssignedTo = AssignTicketToUser();

            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand openNewTechnicalTicket = new SqlCommand($"INSERT INTO CustomerTickets VALUES (GETDATE(), '{currentUsername}', '{userAssignedTo}', 'open', '{comment}')", dbcon);
                openNewTechnicalTicket.ExecuteScalar();
                SqlCommand fetchNewTicketID = new SqlCommand($"SELECT TOP 1 ticketID FROM CustomerTickets ORDER BY ticketID DESC", dbcon);
                int ticketID = (int)fetchNewTicketID.ExecuteScalar();
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                InputOutputAnimationControlClass.UniversalLoadingOuput("Filing new customer ticket in progress");
                Console.WriteLine($"New Customer Ticket with ID: {ticketID} has been successfully created and assigned to {userAssignedTo}. Status: Open");
            }
            Console.WriteLine("\r\nPress any key to return");
            Console.ReadKey();
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            InputOutputAnimationControlClass.UniversalLoadingOuput("Loading");
            ActiveUserFunctionsClass.ActiveUserProcedures();
        }

        public static void CloseCustomerTicket()
        {
            Console.WriteLine("\r\nCLOSE AN EXISTING TECHNICAL TICKET");
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            Console.WriteLine("Would you like to open the list of Opened Tickets?");
            string option = InputOutputAnimationControlClass.PromptYesOrNo();
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                if (option == "Y" || option == "y")
                {
                    dbcon.Open();
                    SqlCommand ShowTicketsFromDatabase = new SqlCommand("SELECT * FROM CustomerTickets WHERE ticketStatus = 'open'", dbcon);
                    using (var reader = ShowTicketsFromDatabase.ExecuteReader())
                    {
                        List<string> ShowtTicketsList = new List<string>();
                        while (reader.Read())
                        {
                            int ticketID = (int)reader[0];
                            string username = (string)reader[1];
                            string ticketStatus = (string)reader[2];
                            string comments = (string)reader[3];
                            var stringLength = comments.Length;
                            if (stringLength > 40)
                            {
                                comments = comments.Substring(0, 40) + "...";
                            }

                            ShowtTicketsList.Add(ticketID.ToString());
                            ShowtTicketsList.Add(username);
                            ShowtTicketsList.Add(ticketStatus);
                            ShowtTicketsList.Add(comments);
                            Console.WriteLine($"ticketID: {ticketID} - username: {username} - ticket status: {ticketStatus} - comment preview: {comments}");
                        }
                    }
                    CloseCustomerTicket();
                }
                else
                {
                    int ticketID = InputOutputAnimationControlClass.SelectTicketID();
                    Console.WriteLine($"Are you sure you want to mark ticket {ticketID} as closed?");
                    string option2 = InputOutputAnimationControlClass.PromptYesOrNo();
                    if (option2 == "Y" || option2 == "y")
                    {
                        dbcon.Open();
                        SqlCommand closeCustomerTicket = new SqlCommand($"UPDATE CustomerTickets SET ticketStatus = 'closed' WHERE ticketID = {ticketID} ", dbcon);
                        closeCustomerTicket.ExecuteScalar();
                        InputOutputAnimationControlClass.UniversalLoadingOuput("Action in progress");
                        Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully marked as closed");
                    }
                    else
                    {
                        CloseCustomerTicket();
                    }
                }
            }
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            ApplicationMenuClass.LoginScreen();
        }
    }
}
