using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace IndividualProject
{
    class TransactedDataClass
    {
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        static readonly string newComplaintPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\Complaints";
        static readonly string newTechnicalIssuePath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\TechnicalIssues";

        public static void OpenNewCustomerTicket()
        {
            ConsoleKey ticketOption = InputOutputControlClass.ChooseTechnicalOrComplaintTicket();

            switch (ticketOption)
            {
                case ConsoleKey.D1:
                    OpenNewTechnicalTicket();
                    break;

                case ConsoleKey.D2:

                    break;
            }
        }

        static void OpenNewTechnicalTicket()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            Console.WriteLine("\r\nFILE NEW TECHNICAL TICKET");
            InputOutputControlClass.ClearScreen();
            string comment = InputOutputControlClass.TicketComment();
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand openNewTechnicalTicket = new SqlCommand($"INSERT INTO CustomerTickets VALUES ('{currentUsername}', 'open', '{comment}')", dbcon);
                openNewTechnicalTicket.ExecuteScalar();
                SqlCommand fetchNewTicketID = new SqlCommand($"SELECT ticketID FROM CustomerTickets WHERE comments = '{comment}'", dbcon);
                int ticketID = (int)fetchNewTicketID.ExecuteScalar();
                ConsoleOutputAndAnimations.FilingNewCustomerTicketOutput();
                Console.WriteLine($"New Customer Ticket with ID: {ticketID} has been successfully created. Status: Open");
            }
        }
    }
}
