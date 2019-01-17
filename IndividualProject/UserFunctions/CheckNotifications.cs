using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    public class CheckNotifications
    {
        private static ConnectToServer _db = new ConnectToServer();
        private static DataToTextFile _text = new DataToTextFile();
        private static OutputControl print = new OutputControl();

        public static void CheckUserNotifications()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Loading");

            int countTickets = _db.CountOpenTicketsAssignedToUser(currentUsername);
            string showListOfTickets = "Show List of Tickets";
            string back = "\r\nBack";
            string showNotificationsLog = "Show notifications Log";
            string openListMsg = $"There are [{countTickets}] open Trouble Tickets assigned to you.\r\nHow would you like to proceed?";

            string viewNotificationsList = SelectMenu.MenuColumn(new List<string> { showListOfTickets, showNotificationsLog, back }, currentUsername, openListMsg).option;

            if (viewNotificationsList == showListOfTickets)
            {
                if (countTickets == 0)
                {
                    Console.WriteLine("\r\nYou do not have any Tickets assigned to you.\n\n(Press any key to continue)");
                    Console.ReadKey();
                }
                else
                {
                    _db.SelectOpenTicketsAssignedToUser(currentUsername);
                    Console.WriteLine("(Press any key to continue)");
                    Console.ReadKey();
                    CheckUserNotifications();
                }
            }
            else if (viewNotificationsList == showNotificationsLog)
            {
                _text.ViewUserNotificationsLog(currentUsername);
                Console.WriteLine("(Press any key to continue)");
                Console.ReadKey();
                CheckUserNotifications();
            }
            else if (viewNotificationsList == back)
            {
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
        }

        public static void CheckAdminNotifications()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Loading");
            string pendingUsernameCheck = _text.GetPendingUsername();            

            if (pendingUsernameCheck == " ")
            {
                Console.WriteLine("There are no pending User registrations\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else
            {
                string yes = "Yes";
                string no = "No";
                string requestMsg = "\r\nYou have 1 pending User registration request. Would you like to create new user?\n";
                string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, requestMsg).option;

                if (yesOrNoSelection == yes)
                {
                    SuperAdminFunctions.CreateNewUserFromRequestFunction();
                }
                else if (yesOrNoSelection == no)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

    }
}
