using System;

namespace IndividualProject
{
    public class OpenNewTroubleTicket
    {
        public static void OpenTicket()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string comment = OutputControl.TicketComment();
            string userAssignedTo = TransactedData.AssignTicketToUser();

            ConnectToServer.OpenNewTechnicalTicket(currentUsername, userAssignedTo, comment);
            Console.WriteLine("\n\nPress any key to return");
            Console.ReadKey();
            ManageTroubleTickets.OpenOrCloseTroubleTicket();
        }
    }
}
