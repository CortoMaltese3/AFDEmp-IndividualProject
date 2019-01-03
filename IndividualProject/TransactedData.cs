using System;
using System.Collections.Generic;
using System.IO;

namespace IndividualProject
{
    class TransactedData
    {
        static readonly string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
        static readonly string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();

        public static void ManageCustomerTickets()
        {

            string open = "Open new Customer Ticket", close = "Close Customer Ticket", back = "\r\nBack",
                manageTicketmsg = "\r\nChoose one of the following options to continue:\r\n";
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
            string open = "Open new Customer Ticket", back = "\r\nBack", manageTicketmsg = "\r\nChoose one of the following options to continue:\r\n";
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

            string assignTicket = "Would you like to assign the ticket to another user?\r\n";
            string yes = "Yes", no = "No";

            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no, }, currentUsername, assignTicket).option;

            if (yesOrNoSelection == yes)
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Loading");

                Dictionary<string, string> AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                Console.Write("\r\nPlease select a user and proceed to assign: ");
                string usernameAssignment = InputOutputAnimationControl.UsernameInput();

                while (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false || usernameAssignment == "admin")
                {
                    if (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false)
                    {
                        Console.WriteLine($"Database does not contain a User {usernameAssignment}.");
                        System.Threading.Thread.Sleep(1500);
                        InputOutputAnimationControl.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputOutputAnimationControl.UsernameInput();
                    }
                    else
                    {
                        Console.WriteLine("Cannot assign ticket to super_admin! Please choose a different user.");
                        System.Threading.Thread.Sleep(1500);
                        InputOutputAnimationControl.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputOutputAnimationControl.UsernameInput();
                    }
                }
                AssignTicketToUserNotification(currentUsername, usernameAssignment);
                return usernameAssignment;
            }

            else if (yesOrNoSelection == no)
            {
                return currentUsername;
            }
            return currentUsername;
        }

        public static void AssignTicketToUserNotification(string currentUserAssigning, string UserAssigningTicketTo)
        {
            try
            {
                DateTime dateTimeAdded = DateTime.Now;
                using (StreamWriter sw = File.AppendText(Globals.TTnotificationToUser + UserAssigningTicketTo + ".txt"))
                {
                    sw.WriteLine($"[{dateTimeAdded}] - User {currentUserAssigning} has assigned a new TT to you. Check your notifications for more details");
                }
            }
            catch (FileNotFoundException fileNotFound)
            {
                Console.WriteLine(fileNotFound.Message);
            }
        }

        public static void OpenNewCustomerTicket()
        {
            string comment = InputOutputAnimationControl.TicketComment();
            string userAssignedTo = AssignTicketToUser();

            ConnectToServer.OpenNewTechnicalTicket(currentUsername, userAssignedTo, comment);
            Console.WriteLine("\n\nPress any key to return");
            Console.ReadKey();
            ManageCustomerTickets();
        }

        public static void CloseCustomerTicket()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("CLOSE EXISTING TECHNICAL TICKETS");

            string yes = "Yes", no = "No", listOfTickets = "Would you like to open the list of Opened Tickets?\r\n";
            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, listOfTickets).option;
                if (optionYesOrNo == yes)
                {
                    ConnectToServer.ViewListOfOpenCustomerTickets();
                    CloseCustomerTicketFunction();
                }
                else if (optionYesOrNo == no)
                {
                    CloseCustomerTicketFunction();
                }
            }
        }

        public static void CloseCustomerTicketFunction()
        {
            int ticketID = InputOutputAnimationControl.SelectTicketID();
            if (ConnectToServer.CheckIfTicketIDWithStatusOpenExistsInList(ticketID) == false)
            {
                Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]\n\n(Press any key to continue)");
                Console.ReadKey();
                ManageCustomerTickets();
            }
            else
            {
                string yes = "Yes", no = "No", closeTicket = $"Are you sure you want to mark ticket {ticketID} as closed?\r\n";
                string optionYesOrNo2 = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, closeTicket).option;
                if (optionYesOrNo2 == yes)
                {
                    ConnectToServer.SetTicketStatusToClosed(currentUsername, ticketID);
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
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("DELETE EXISTING TECHNICAL TICKETS");

            string yes = "Yes", no = "No", listMsg = "Would you like to open the list of Existing Tickets?\r\n";
            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, listMsg).option;
                if (optionYesOrNo == yes)
                {
                    ConnectToServer.ViewListOfAllCustomerTickets();
                    DeleteExistingOpenOrClosedTicketSubFunction();
                }
                else if (optionYesOrNo == no)
                {
                    DeleteExistingOpenOrClosedTicketSubFunction();
                }
            }
        }

        public static void DeleteExistingOpenOrClosedTicketSubFunction()
        {
            int ticketID = InputOutputAnimationControl.SelectTicketID();
            if (ConnectToServer.CheckIfTicketIDWithStatusOpenOrClosedExistsInList(ticketID) == false)
            {
                Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }

            string yes = "Yes", no = "No", deleteTicketMsg = $"Are you sure you want to delete ticket {ticketID}? Action cannot be undone.\r\n";
            string optionYesOrNo2 = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, deleteTicketMsg).option;
            if (optionYesOrNo2 == yes)
            {
                ConnectToServer.DeleteCustomerTicket(currentUsername, ticketID);
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else if (optionYesOrNo2 == no)
            {
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
        }

        public static void ViewExistingOpenTicketsFunction()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("VIEW OPEN TECHNICAL TICKETS");
            string listTicketsMsg = "Would you like to open the list of Opened Tickets?\r\n";
            string yes = "Yes", no = "No";
            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, listTicketsMsg).option;
                if (optionYesOrNo == yes)
                {
                    ConnectToServer.ViewListOfOpenCustomerTickets();
                    ViewExistingOpenTicketsSubFunction();
                }
                else if (optionYesOrNo == no)
                {
                    ViewExistingOpenTicketsSubFunction();
                }
            }
        }

        public static void ViewExistingOpenTicketsSubFunction()
        {
            int TicketID = InputOutputAnimationControl.SelectTicketID();
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
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine($"VIEW TECHNICAL TICKET WITH [ID = {ticketID}]");
            ConnectToServer.SelectSingleCustomerTicket(ticketID);
            Console.Write("Press any key to return");
            Console.ReadKey();
        }

        public static void EditExistingOpenTicketFunction()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("EDIT OPEN TECHNICAL TICKET");

            string yes = "Yes", no = "No", listTicketsMsg = "Would you like to open the list of Opened Tickets?";
            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, listTicketsMsg).option;
                if (optionYesOrNo == yes)
                {
                    ConnectToServer.ViewListOfOpenCustomerTickets();
                    EditExistingOpenTicketSubFunction();
                }
                else if (optionYesOrNo == no)
                {
                    EditExistingOpenTicketSubFunction();
                }
            }
        }

        public static void EditExistingOpenTicketSubFunction()
        {
            int TicketID = InputOutputAnimationControl.SelectTicketID();
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
            string edit = "Edit Ticket Comment", assign = "Edit Ticket's User assignment", back = "Back",
                editMsg = "\r\nChoose one of the following options to continue:\r\n";
            while (true)
            {
                string EditCommentAndAssignment = SelectMenu.MenuColumn(new List<string> { edit, assign, back }, currentUsername, editMsg).option;

                if (EditCommentAndAssignment == edit)
                {
                    string ticketComment = InputOutputAnimationControl.TicketComment();
                    ConnectToServer.EditCommentOfOpenTicket(ID, ticketComment);
                }
                else if (EditCommentAndAssignment == assign)
                {
                    string newUserAssignment = AssignTicketToUser();
                    ChangeUserAssignmentToOpenTicket(ID, newUserAssignment);
                }
                else if (EditCommentAndAssignment == back)
                {
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    InputOutputAnimationControl.UniversalLoadingOuput("Loading");
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        private static void ChangeUserAssignmentToOpenTicket(int ID, string nextOwner)
        {
            ConnectToServer.ChangeUserAssignedTo(nextOwner, ID);

            if (nextOwner == currentUsername)
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"The ownership of the Customer Ticket with [ID = {ID}] remains to User: {nextOwner}\n\n(Press any key to continue)");
                Console.ReadKey();
            }
            else
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"The ownership of the Customer Ticket with [ID = {ID}] has been successfully transfered to User: {nextOwner}\n\n(Press any key to continue)");
                Console.ReadKey();
            }
        }

        public static void ViewUserNotificationsLog(string currentUser)
        {
            string[] lines = File.ReadAllLines(Globals.TTnotificationToUser + currentUser + ".txt");
            int index = 1;
            foreach (string line in lines)
            {
                Console.WriteLine(index + ". " + line + "\n");
                index++;                    
            }
        }
    }
}