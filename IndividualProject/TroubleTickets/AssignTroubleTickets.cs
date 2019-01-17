using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class AssignTroubleTickets
    {
        //User selects whether to assign the ticket to himself or transfer ownership to another

        private static ConnectToServer _db = new ConnectToServer();
        private static DataToTextFile _text = new DataToTextFile();
        private static OutputControl print = new OutputControl();

        public static string AssignTicketToUser()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string assignTicket = "Would you like to assign the ticket to another user?\r\n";
            string yes = "Yes";
            string no = "No";
            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no, }, currentUsername, assignTicket).option;

            if (yesOrNoSelection == yes)
            {
                print.QuasarScreen(currentUsername);
                ColorAndAnimationControl.UniversalLoadingOuput("Loading");

                Dictionary<string, string> AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
                Console.Write("\r\nPlease select a user and proceed to assign: ");
                string usernameAssignment = InputControl.UsernameInput();

                while (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false || usernameAssignment == "admin")
                {
                    if (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false)
                    {
                        Console.WriteLine($"Database does not contain a User {usernameAssignment}.\n\n(Press any key to continue)");
                        Console.ReadKey();
                        print.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\n\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputControl.UsernameInput();
                    }
                    else
                    {
                        Console.WriteLine("Cannot assign ticket to super_admin! Please choose a different user.\n\n(Press any key to continue)");
                        Console.ReadKey();
                        print.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputControl.UsernameInput();
                    }
                }
                _text.AssignTicketToUserNotification(currentUsername, usernameAssignment);
                return usernameAssignment;
            }

            else if (yesOrNoSelection == no)
            {
                return currentUsername;
            }
            return currentUsername;
        }

        public static void ChangeUserAssignmentToOpenTicket(int ID, string nextOwner)
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            _db.ChangeUserAssignedTo(nextOwner, ID);

            if (nextOwner == currentUsername)
            {
                print.QuasarScreen(currentUsername);
                ColorAndAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"The ownership of the Customer Ticket with [ID = {ID}] remains to User: {nextOwner}\n\n(Press any key to continue)");
                Console.ReadKey();
            }
            else
            {
                print.QuasarScreen(currentUsername);
                ColorAndAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"The ownership of the Customer Ticket with [ID = {ID}] has been successfully transfered to User: {nextOwner}\n\n(Press any key to continue)");
                Console.ReadKey();
            }
        }
    }
}