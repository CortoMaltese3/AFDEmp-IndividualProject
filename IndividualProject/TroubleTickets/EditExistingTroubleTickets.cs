using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class EditExistingTroubleTickets
    {
        public static void EditOpenTicket()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();

            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("EDIT OPEN TECHNICAL TICKET");

            string listTicketsMsg = "Choose one of the following options\r\n";
            string viewList = "View Trouble Ticket List";
            string viewSpecific = "Edit Specific Trouble Ticket";
            string back = "\r\nBack";

            while (true)
            {
                string editTicket = SelectMenu.MenuColumn(new List<string> { viewList, viewSpecific, back }, currentUsername, listTicketsMsg).option;
                if (editTicket == viewList)
                {
                    ConnectToServer.ViewListOfOpenCustomerTickets();
                    EditOpenTicketSubFunction();
                }
                else if (editTicket == viewSpecific)
                {
                    EditOpenTicketSubFunction();
                }
                else if (editTicket == back)
                {
                    OutputControl.QuasarScreen(currentUsername);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        private static void EditOpenTicketSubFunction()
        {
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            int TicketID = OutputControl.SelectTicketID();
            if (ConnectToServer.CheckIfTicketIDWithStatusOpenExistsInList(TicketID) == false)
            {
                Console.WriteLine($"There is no Customer Ticket with [ID = {TicketID}]\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else
            {
                EditTicketOptions(TicketID);
                ViewExistingTickets.ViewSingleCustomerTicket(TicketID);
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
        }

        private static void EditTicketOptions(int ID)
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();

            string edit = "Edit Ticket Comment";
            string assign = "Edit Ticket's User assignment";
            string back = "\r\nBack";
            string editMsg = "\r\nChoose one of the following options to continue:\r\n";

            while (true)
            {
                string EditCommentAndAssignment = SelectMenu.MenuColumn(new List<string> { edit, assign, back }, currentUsername, editMsg).option;

                if (EditCommentAndAssignment == edit)
                {
                    string ticketComment = OutputControl.TicketComment();
                    ConnectToServer.EditCommentOfOpenTicket(ID, ticketComment);
                }
                else if (EditCommentAndAssignment == assign)
                {
                    string newUserAssignment = AssignTroubleTickets.AssignTicketToUser();
                    AssignTroubleTickets.ChangeUserAssignmentToOpenTicket(ID, newUserAssignment);
                }
                else if (EditCommentAndAssignment == back)
                {
                    OutputControl.QuasarScreen(currentUsername);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }
    }
}
