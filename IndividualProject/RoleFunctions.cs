using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class RoleFunctions
    {
        static string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
        static string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();

        public static void CreateNewUserFromRequestFunction()
        {
            string pendingUsername = File.ReadLines(Globals.newUserRequestPath).First();
            if (pendingUsername == " ")
            {
                InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.Write("There are no pending requests.\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else
            {
                pendingUsername = pendingUsername.Remove(0, 10);

                string pendingPassphrase = File.ReadLines(Globals.newUserRequestPath).Skip(1).Take(1).First();
                pendingPassphrase = pendingPassphrase.Remove(0, 12);

                string yes = "Yes", no = "No", createUserMsg = $"\r\nYou are about to create a new username-password entry : {pendingUsername} - {pendingPassphrase}.\r\nWould you like to proceed?\r\n"; ;
                string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, createUserMsg).option;

                if (yesOrNoSelection == yes)
                {
                    string pendingRole = InputOutputAnimationControl.SelectUserRole();

                    ConnectToServer.InsertNewUserIntoDatabase(pendingUsername, pendingPassphrase, pendingRole);
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    InputOutputAnimationControl.UniversalLoadingOuput("Creating new user in progress");
                    //Clears the new user registrations List
                    File.WriteAllLines(Globals.newUserRequestPath, new string[] { " " });
                    //Creates a file for the new user to check notifications
                    File.WriteAllLines(Globals.TTnotificationToUser + pendingUsername + ".txt", new string[] { "NOTIFICATIONS LOG" });
                    Console.WriteLine($"User {pendingUsername} has been created successfully. Status : {pendingRole}.\n\n(Press any key to continue)");
                    Console.ReadKey();
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
                else if (yesOrNoSelection == no)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        public static void DeleteUserFromDatabase()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
            Dictionary<string, string> AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();

            string username = InputOutputAnimationControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}. Please select a different user.");
                }
                else
                {
                    Console.WriteLine("Cannot delete super_admin! Please choose a different user.");
                }
                Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
                AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                username = InputOutputAnimationControl.UsernameInput();
            }
            ConnectToServer.RemoveUsernameFromDatabase(username);
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Deleting existing user in progress");
            TransactedData.DeleteUserNotificationsLog(username);
            Console.WriteLine($"Username {username} has been successfully deleted from database.\n\n(Press any key to continue)");
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static void ShowAvailableUsersFunction()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            ConnectToServer.ShowAvailableUsersFromDatabase();
            Console.Write("\r\nPress any key to return to Functions menu");
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static void CheckUserNotifications()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            
            int countTickets = ConnectToServer.CountOpenTicketsAssignedToUser(currentUsername);
            if (countTickets == 0)
            {
                Console.WriteLine("\r\nThere are no notifications\n\n(Press any key to go back to Main Menu)");
                Console.ReadKey();
            }
            else
            {
                string showListOfTickets = "Show List of Tickets", back = "\r\nBack", showNotificationsLog = "Show notifications Log", openListMsg = $"There are [{countTickets}] open Trouble Tickets assigned to you.\r\nHow would you like to proceed?";
                string viewNotificationsList = SelectMenu.MenuColumn(new List<string> { showListOfTickets, showNotificationsLog, back }, currentUsername, openListMsg).option;
                if (viewNotificationsList == showListOfTickets)
                {
                    ConnectToServer.SelectOpenTicketsAssignedToUser(currentUsername);
                    Console.WriteLine("(Press any key to continue)");
                    Console.ReadKey();
                    CheckUserNotifications();
                }
                else if (viewNotificationsList == showNotificationsLog)
                {
                    TransactedData.ViewUserNotificationsLog(currentUsername);
                    Console.WriteLine("(Press any key to continue)");
                    Console.ReadKey();
                    CheckUserNotifications();
                }
                else if (viewNotificationsList == back)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }                
            }
        }

        public static void CheckAdminNotifications()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            string pendingUsernameCheck = File.ReadLines(Globals.newUserRequestPath).First();

            if (pendingUsernameCheck == " ")
            {
                Console.WriteLine("There are no pending User registrations\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else
            {
                string yes = "Yes", no = "No", requestMsg = "\r\nYou have 1 pending User registration request. Would you like to create new user?\n";
                string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, requestMsg).option;

                if (yesOrNoSelection == yes)
                {
                    CreateNewUserFromRequestFunction();
                }
                else if (yesOrNoSelection == no)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        public static void AlterUserRoleStatus()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Dictionary<string, string> AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
            Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
            string username = InputOutputAnimationControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {                    
                    Console.WriteLine($"Database does not contain a User {username}\n\n(Press any key to continue)");                    
                }
                else
                {
                    Console.WriteLine("Cannot alter super_admin's Status! Please choose a different user\n\n(Press any key to continue)");                
                }
                Console.ReadKey();
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
                username = InputOutputAnimationControl.UsernameInput();
            }
            string userRole = InputOutputAnimationControl.SelectUserRole();
            ConnectToServer.SelectSingleUserRole(username, currentUsername, userRole);
        }
    }
}
