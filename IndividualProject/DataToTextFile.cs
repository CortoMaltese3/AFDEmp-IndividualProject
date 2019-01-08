using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    class DataToTextFile
    {
        public static void ViewUserNotificationsLog(string currentUser)
        {
            string[] lines = File.ReadAllLines(Globals.TTnotificationToUser + currentUser + ".txt");
            int index = 1;
            Console.WriteLine("NOTIFICATIONS LOG");
            foreach (string line in lines.Skip(1))
            {
                Console.WriteLine(index + ". " + line + "\n");
                index++;
            }
        }

        public static void DeleteUserNotificationsLog(string userToBeDeleted)
        {
            try
            {
                File.Delete(Globals.TTnotificationToUser + userToBeDeleted + ".txt");
            }
            catch (FileNotFoundException dir)
            {
                Console.WriteLine(dir.Message);
            }
            Console.WriteLine($"{userToBeDeleted}'s notifications log has been successfully deleted");
        }

        public static void CloseTicketToUserNotification(string userClosingTheTicket, string previousTicketOwner, int ticketID)
        {
            try
            {
                DateTime dateTimeAdded = DateTime.Now;
                using (StreamWriter sw = File.AppendText(Globals.TTnotificationToUser + previousTicketOwner + ".txt"))
                {
                    sw.WriteLine($"[{dateTimeAdded}] - User {userClosingTheTicket} has marked the TT with [ID = {ticketID}] that was assigned to you as closed. Visit the View TT Section for more details");
                }
            }
            catch (FileNotFoundException fileNotFound)
            {
                Console.WriteLine(fileNotFound.Message);
            }
        }

        public static void AssignTicketToUserNotification(string currentUserAssigning, string UserAssigningTicketTo)
        {
            try
            {
                DateTime dateTimeAdded = DateTime.Now;
                using (StreamWriter sw = File.AppendText(Globals.TTnotificationToUser + UserAssigningTicketTo + ".txt"))
                {
                    sw.WriteLine($"[{dateTimeAdded}] - User {currentUserAssigning} has assigned a new TT to you. Visit the View TT Section for more details");
                }
            }
            catch (FileNotFoundException fileNotFound)
            {
                Console.WriteLine(fileNotFound.Message);
            }
        }
        
        public static string GetPendingPassphrase()
        {
            string pendingPassphrase = File.ReadLines(Globals.newUserRequestPath).Skip(1).Take(1).First();
            return pendingPassphrase;
        }

        public static string GetPendingUsername()
        {
            string pendingUsername = File.ReadLines(Globals.newUserRequestPath).First();
            return pendingUsername;
        }

        public static void ClearNewUserRegistrationList()
        {
            //Clears the new user registrations List
            File.WriteAllLines(Globals.newUserRequestPath, new string[] { " " });            
        }

        public static void CreateNewUserLogFile(string pendingUsername)
        {
            //Creates a file for the new user to check notifications
            File.WriteAllLines(Globals.TTnotificationToUser + pendingUsername + ".txt", new string[] { "NOTIFICATIONS LOG" });
        }



    }
}
