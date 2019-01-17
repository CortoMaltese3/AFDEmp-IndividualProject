using System;

namespace IndividualProject
{
    public class OpenNewTroubleTicket
    {
        public static void OpenTicket()
        {
            var _db = new ConnectToServer();
            var print = new OutputControl();

            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string comment = print.TicketComment();
            string userAssignedTo = AssignTroubleTickets.AssignTicketToUser();

            _db.OpenNewTechnicalTicket(currentUsername, userAssignedTo, comment);
            Console.WriteLine("\n\nPress any key to return");
            Console.ReadKey();
            ManageTroubleTickets.OpenOrCloseTroubleTicket();
        }
    }
}
