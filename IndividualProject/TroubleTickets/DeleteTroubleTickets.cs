using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class DeleteTroubleTickets
    {
        public static void DeleteExistingOpenOrClosedTicketFunction()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("DELETE EXISTING TECHNICAL TICKETS");

            string viewList = "View List of Tickets";
            string back = "\r\nBack";
            string closeSpecific = "Delete Specific Ticket";
            string deleteTicketsMsg = "Choose one of the following functions\r\n";

            while (true)
            {
                string deleteTickets = SelectMenu.MenuColumn(new List<string> { viewList, closeSpecific, back }, currentUsername, deleteTicketsMsg).option;
                if (deleteTickets == viewList)
                {
                    ConnectToServer.ViewListOfAllCustomerTickets();
                    DeleteExistingOpenOrClosedTicketSubFunction();
                }
                else if (deleteTickets == closeSpecific)
                {
                    DeleteExistingOpenOrClosedTicketSubFunction();
                }
                else if (deleteTickets == back)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        private static void DeleteExistingOpenOrClosedTicketSubFunction()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            int ticketID = OutputControl.SelectTicketID();
            string previousTicketOwner = ConnectToServer.SelectUserAssignedToTicket(ticketID);
            if (ConnectToServer.CheckIfTicketIDWithStatusOpenOrClosedExistsInList(ticketID) == false)
            {
                Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }

            string yes = "Yes";
            string no = "No";
            string deleteTicketMsg = $"Are you sure you want to delete ticket {ticketID}? Action cannot be undone.\r\n";
            string optionYesOrNo2 = SelectMenu.MenuColumn(new List<string> { yes, no }, currentUsername, deleteTicketMsg).option;

            if (optionYesOrNo2 == yes)
            {
                ConnectToServer.DeleteCustomerTicket(currentUsername, ticketID);
                DataToTextFile.DeleteTicketToUserNotification(currentUsername, previousTicketOwner, ticketID);
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else if (optionYesOrNo2 == no)
            {
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
        }
    }
}
