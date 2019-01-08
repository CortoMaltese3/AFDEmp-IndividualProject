using System;
using System.Collections.Generic;
using System.IO;

namespace IndividualProject
{
    class TransactedData
    {
        public static void ManageCustomerTickets()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            string open = "Open new Customer Ticket";
            string close = "Close Customer Ticket";
            string back = "\r\nBack";
            string manageTicketmsg = "\r\nChoose one of the following options to continue:\r\n";

            while (true)
            {
                string openCloseTicket = SelectMenu.MenuColumn(new List<string> { open, close, back }, currentUsername, manageTicketmsg).option;

                if (openCloseTicket == open)
                {
                    OpenNewCustomerTicket();
                }

                else if (openCloseTicket == close)
                {
                    CloseCustomerTicket();
                }

                else if (openCloseTicket == back)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        public static void OpenCustomerTickets()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            string open = "Open new Customer Ticket";
            string back = "\r\nBack";
            string manageTicketmsg = "\r\nChoose one of the following options to continue:\r\n";

            while (true)
            {
                string openTicket = SelectMenu.MenuRow(new List<string> { open, back }, currentUsername, manageTicketmsg).option;

                if (openTicket == open)
                {
                    OpenNewCustomerTicket();
                }

                else if (openTicket == back)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        public static string AssignTicketToUser()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string assignTicket = "Would you like to assign the ticket to another user?\r\n";
            string yes = "Yes";
            string no = "No";
            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no, }, currentUsername, assignTicket).option;

            if (yesOrNoSelection == yes)
            {
                OutputControl.QuasarScreen(currentUsername);
                ColorAndAnimationControl.UniversalLoadingOuput("Loading");

                Dictionary<string, string> AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                Console.Write("\r\nPlease select a user and proceed to assign: ");
                string usernameAssignment = InputControl.UsernameInput();

                while (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false || usernameAssignment == "admin")
                {
                    if (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false)
                    {
                        Console.WriteLine($"Database does not contain a User {usernameAssignment}.\n\n(Press any key to continue)");
                        Console.ReadKey();
                        OutputControl.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\n\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputControl.UsernameInput();
                    }
                    else
                    {
                        Console.WriteLine("Cannot assign ticket to super_admin! Please choose a different user.\n\n(Press any key to continue)");
                        Console.ReadKey();
                        OutputControl.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputControl.UsernameInput();
                    }
                }
                DataToTextFile.AssignTicketToUserNotification(currentUsername, usernameAssignment);
                return usernameAssignment;
            }

            else if (yesOrNoSelection == no)
            {
                return currentUsername;
            }
            return currentUsername;
        }

        public static void OpenNewCustomerTicket()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string comment = OutputControl.TicketComment();
            string userAssignedTo = AssignTicketToUser();

            ConnectToServer.OpenNewTechnicalTicket(currentUsername, userAssignedTo, comment);
            Console.WriteLine("\n\nPress any key to return");
            Console.ReadKey();
            ManageCustomerTickets();
        }

        public static void CloseCustomerTicket()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("CLOSE EXISTING TECHNICAL TICKETS");

            string viewList = "View List of Open Tickets";
            string back = "\r\nBack";
            string closeSpecific = "Close Specific Ticket";
            string listOfTickets = "Choose one of the following functions\r\n";

            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuColumn(new List<string> { viewList, closeSpecific, back }, currentUsername, listOfTickets).option;
                if (optionYesOrNo == viewList)
                {
                    ConnectToServer.ViewListOfOpenCustomerTickets();
                    CloseCustomerTicketFunction();
                }
                else if (optionYesOrNo == closeSpecific)
                {
                    CloseCustomerTicketFunction();
                }
                else if (optionYesOrNo == back)
                {
                    ManageCustomerTickets();
                }
            }
        }

        public static void CloseCustomerTicketFunction()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            int ticketID = OutputControl.SelectTicketID();
            string previousUserAssignedTo = ConnectToServer.SelectUserAssignedToTicket(ticketID);

            if (ConnectToServer.CheckIfTicketIDWithStatusOpenExistsInList(ticketID) == false)
            {
                Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]\n\n(Press any key to continue)");
                Console.ReadKey();
                ManageCustomerTickets();
            }
            else
            {
                string yes = "Yes";
                string no = "No";
                string closeTicket = $"Are you sure you want to mark ticket {ticketID} as closed?\r\n";
                string optionYesOrNo2 = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, closeTicket).option;

                if (optionYesOrNo2 == yes)
                {
                    ConnectToServer.SetTicketStatusToClosed(currentUsername, ticketID);
                    DataToTextFile.CloseTicketToUserNotification(currentUsername, previousUserAssignedTo, ticketID);
                    ManageCustomerTickets();
                }
                else if (optionYesOrNo2 == no)
                {
                    ManageCustomerTickets();
                }
            }
        }

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

        public static void DeleteExistingOpenOrClosedTicketSubFunction()
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
                DeleteTicketToUserNotification(currentUsername, previousTicketOwner, ticketID);
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else if (optionYesOrNo2 == no)
            {
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
        }

        public static void ViewExistingOpenTicketsFunction()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
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
                    ConnectToServer.ViewListOfOpenCustomerTickets();
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

        public static void ViewExistingOpenTicketsSubFunction()
        {
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            int TicketID = OutputControl.SelectTicketID();
            if (ConnectToServer.CheckIfTicketIDWithStatusOpenExistsInList(TicketID) == false)
            {
                Console.WriteLine($"There is no Customer Ticket with [ID = {TicketID}]\n\n(Press any key to go back to Main Menu)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            ViewSingleCustomerTicket(TicketID);
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        private static void ViewSingleCustomerTicket(int ticketID)
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine($"VIEW TECHNICAL TICKET WITH [ID = {ticketID}]");
            ConnectToServer.SelectSingleCustomerTicket(ticketID);
            Console.Write("Press any key to return");
            Console.ReadKey();
        }

        public static void EditExistingOpenTicketFunction()
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
                    EditExistingOpenTicketSubFunction();
                }
                else if (editTicket == viewSpecific)
                {
                    EditExistingOpenTicketSubFunction();
                }
                else if (editTicket == back)
                {
                    OutputControl.QuasarScreen(currentUsername);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        public static void EditExistingOpenTicketSubFunction()
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
                ViewSingleCustomerTicket(TicketID);
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
                    string newUserAssignment = AssignTicketToUser();
                    ChangeUserAssignmentToOpenTicket(ID, newUserAssignment);
                }
                else if (EditCommentAndAssignment == back)
                {
                    OutputControl.QuasarScreen(currentUsername);                    
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        private static void ChangeUserAssignmentToOpenTicket(int ID, string nextOwner)
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            ConnectToServer.ChangeUserAssignedTo(nextOwner, ID);

            if (nextOwner == currentUsername)
            {
                OutputControl.QuasarScreen(currentUsername);
                ColorAndAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"The ownership of the Customer Ticket with [ID = {ID}] remains to User: {nextOwner}\n\n(Press any key to continue)");
                Console.ReadKey();
            }
            else
            {
                OutputControl.QuasarScreen(currentUsername);
                ColorAndAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"The ownership of the Customer Ticket with [ID = {ID}] has been successfully transfered to User: {nextOwner}\n\n(Press any key to continue)");
                Console.ReadKey();
            }
        }

        public static void DeleteTicketToUserNotification(string userDeletingTheTicket, string previousTicketOwner, int ticketID)
        {
            try
            {
                DateTime dateTimeAdded = DateTime.Now;
                using (StreamWriter sw = File.AppendText(Globals.TTnotificationToUser + previousTicketOwner + ".txt"))
                {
                    sw.WriteLine($"[{dateTimeAdded}] - User {userDeletingTheTicket} has deleted the TT with [ID = {ticketID}] assigned to you. In case of emergency, contact the super_admin");
                }
            }
            catch (FileNotFoundException fileNotFound)
            {
                Console.WriteLine(fileNotFound.Message);
            }
        }
    }
}