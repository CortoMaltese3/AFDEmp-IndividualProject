using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IndividualProject
{
    class TransactedDataClass
    {
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        static readonly string newTechnicalIssuePath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\TechnicalIssues";

        public static void OpenNewCustomerTicket()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            Console.WriteLine("\r\nFILE NEW TECHNICAL TICKET");
            InputOutputAnimationControlClass.ClearScreen();
            string comment = InputOutputAnimationControlClass.TicketComment();
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand openNewTechnicalTicket = new SqlCommand($"INSERT INTO CustomerTickets VALUES ('{currentUsername}', 'open', '{comment}')", dbcon);
                openNewTechnicalTicket.ExecuteScalar();
                SqlCommand fetchNewTicketID = new SqlCommand($"SELECT ticketID FROM CustomerTickets WHERE comments = '{comment}'", dbcon);
                int ticketID = (int)fetchNewTicketID.ExecuteScalar();
                InputOutputAnimationControlClass.UniversalLoadingOuput("Filing new customer ticket in progress");
                Console.WriteLine($"New Customer Ticket with ID: {ticketID} has been successfully created. Status: Open");
            }
        }

        public static void CloseCustomerTicket()
        {
            Console.WriteLine("\r\nCLOSE AN EXISTING TECHNICAL TICKET");
            InputOutputAnimationControlClass.ClearScreen();
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
            InputOutputAnimationControlClass.ClearScreen();
            ApplicationMenuClass.LoginScreen();
        }
    }
}
