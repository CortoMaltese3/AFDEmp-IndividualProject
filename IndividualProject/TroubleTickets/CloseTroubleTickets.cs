using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class CloseExistingTroubleTickets
    {
        private static ConnectToServer _db = new ConnectToServer();
        private static DataToTextFile _text = new DataToTextFile();
        private static OutputControl print = new OutputControl();

        public static void CloseTicket()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            print.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("CLOSE EXISTING TECHNICAL TICKETS");

            string viewList = "View List of Open Tickets";
            string back = "\r\nBack";
            string closeSpecific = "Close Specific Ticket";
            string optionsMsg = "Choose one of the following functions\r\n";

            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuColumn(new List<string> { viewList, closeSpecific, back }, currentUsername, optionsMsg).option;
                if (optionYesOrNo == viewList)
                {
                    _db.ViewListOfOpenCustomerTickets();
                    CloseCustomerTicketFunction();
                }
                else if (optionYesOrNo == closeSpecific)
                {
                    CloseCustomerTicketFunction();
                }
                else if (optionYesOrNo == back)
                {
                    ManageTroubleTickets.OpenOrCloseTroubleTicket();
                }
            }
        }

        public static void CloseCustomerTicketFunction()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            int ticketID = OutputControl.SelectTicketID();
            string previousUserAssignedTo = _db.SelectUserAssignedToTicket(ticketID);

            if (_db.CheckIfTicketIDWithStatusOpenExistsInList(ticketID) == false)
            {
                Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]\n\n(Press any key to continue)");
                Console.ReadKey();
                ManageTroubleTickets.OpenOrCloseTroubleTicket();
            }
            else
            {
                string yes = "Yes";
                string no = "No";
                string closeTicket = $"Are you sure you want to mark ticket {ticketID} as closed?\r\n";
                string optionYesOrNo2 = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, closeTicket).option;

                if (optionYesOrNo2 == yes)
                {
                    _db.SetTicketStatusToClosed(currentUsername, ticketID);
                    _text.CloseTicketToUserNotification(currentUsername, previousUserAssignedTo, ticketID);
                    ManageTroubleTickets.OpenOrCloseTroubleTicket();
                }
                else if (optionYesOrNo2 == no)
                {
                    ManageTroubleTickets.OpenOrCloseTroubleTicket();
                }
            }
        }
    }
}
