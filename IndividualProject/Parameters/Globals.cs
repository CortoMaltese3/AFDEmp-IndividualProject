using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    public static class Globals
    {
        public static readonly string connectionString = Properties.Settings.Default.connectionString;
            //"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";

        public static readonly string newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";
        public static readonly string TTnotificationToUser = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\TechnicalIssues\TroubleTicketNotificationToUser_";
    }
}
