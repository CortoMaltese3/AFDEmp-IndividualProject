using System.Collections.Generic;

namespace IndividualProject
{
    public class ManageTroubleTickets
    {
        public static void OpenOrCloseTroubleTicket()
        {
            var _db = new ConnectToServer();
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            string open = "Open new Customer Ticket";
            string close = "Close Customer Ticket";
            string back = "\r\nBack";
            string manageTicketmsg = "\r\nChoose one of the following options to continue:\r\n";

            while (true)
            {
                string openCloseTicketMsg = SelectMenu.MenuColumn(new List<string> { open, close, back }, currentUsername, manageTicketmsg).option;

                if (openCloseTicketMsg == open)
                {
                    OpenNewTroubleTicket.OpenTicket();
                }

                else if (openCloseTicketMsg == close)
                {
                    CloseExistingTroubleTickets.CloseTicket();
                }

                else if (openCloseTicketMsg == back)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }
    }
}
