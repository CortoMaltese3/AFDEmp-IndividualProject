using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    class ViewExistingTickets
    {
        public static void ViewExistingOpenTicketsFunction()
        {
            var _db = new ConnectToServer();
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
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
                    OutputControl.QuasarScreen(currentUsername);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        private static void ViewExistingOpenTicketsSubFunction()
        {
            var _db = new ConnectToServer();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            int TicketID = OutputControl.SelectTicketID();
            if (_db.CheckIfTicketIDWithStatusOpenExistsInList(TicketID) == false)
            {
                Console.WriteLine($"There is no Customer Ticket with [ID = {TicketID}]\n\n(Press any key to go back to Main Menu)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            ViewSingleCustomerTicket(TicketID);
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static void ViewSingleCustomerTicket(int ticketID)
        {
            var _db = new ConnectToServer();
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine($"VIEW TECHNICAL TICKET WITH [ID = {ticketID}]");
            _db.SelectSingleCustomerTicket(ticketID);
            Console.Write("Press any key to return");
            Console.ReadKey();
        }
    }
}
