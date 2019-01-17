using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class ViewExistingTickets
    {
        private static ConnectToServer _db = new ConnectToServer();
        private static OutputControl print = new OutputControl();

        public static void ViewExistingOpenTicketsFunction()
        {            
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Loading");
            Console.WriteLine("VIEW OPEN TECHNICAL TICKETS");

            string listTicketsMsg = "Choose one of the following options\r\n";
            string viewList = "View Trouble Ticket List";
            string viewSpecific = "View Specific Trouble Ticket";
            string back = "\r\nBack";

            while (true)
            {
                string viewTickets = SelectMenu.MenuColumn(new List<string> { viewList, viewSpecific, back }, currentUsername, listTicketsMsg).option;
                if (viewTickets == viewList)
                {
                    _db.ViewListOfOpenCustomerTickets();
                    ViewExistingOpenTicketsSubFunction();
                }
                else if (viewTickets == viewSpecific)
                {
                    ViewExistingOpenTicketsSubFunction();
                }
                else if (viewTickets == back)
                {
                    print.QuasarScreen(currentUsername);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        private static void ViewExistingOpenTicketsSubFunction()
        {            
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            int TicketID = print.SelectTicketID();
            if (_db.CheckIfTicketIDWithStatusOpenExistsInList(TicketID) == false)
            {
                print.ColoredText($"There is no Customer Ticket with [ID = {TicketID}]\n\n(Press any key to go back to Main Menu)", ConsoleColor.DarkRed);
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            ViewSingleCustomerTicket(TicketID);
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static void ViewSingleCustomerTicket(int ticketID)
        {            
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Loading");
            Console.WriteLine($"VIEW TECHNICAL TICKET WITH [ID = {ticketID}]");
            _db.SelectSingleCustomerTicket(ticketID);
            Console.Write("Press any key to return");
            Console.ReadKey();
        }
    }
}
