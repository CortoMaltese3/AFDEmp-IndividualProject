using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class SuperAdminFunctions
    {
        private static ConnectToServer _db = new ConnectToServer();
        private static DataToTextFile _text = new DataToTextFile();
        private static OutputControl print = new OutputControl();

        //Handles creation/deleting/viewing/editing of users by super_admin
        public static void CreateNewUserFromRequestFunction()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            string pendingUsername = _text.GetPendingUsername();

            if (pendingUsername == " ")
            {
                print.UniversalLoadingOutput("Action in progress");
                Console.Write("There are no pending requests.\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else
            {
                pendingUsername = pendingUsername.Remove(0, 10);
                string pendingPassphrase = _text.GetPendingPassphrase().Remove(0, 12);
                string yes = "Yes";
                string no = "No";
                string createUserMsg = $"\r\nYou are about to create a new entry :\nUsername: {pendingUsername} - Password: {pendingPassphrase}\n\nWould you like to proceed?\n\n";
                string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, createUserMsg).option;

                if (yesOrNoSelection == yes)
                {
                    string pendingRole = print.SelectUserRole();

                    _db.InsertNewUserIntoDatabase(pendingUsername, pendingPassphrase, pendingRole);
                    print.QuasarScreen(currentUsername);
                    print.UniversalLoadingOutput("Creating new user in progress");

                    _text.ClearNewUserRegistrationList();
                    _text.CreateNewUserLogFile(pendingUsername);

                    print.ColoredText($"User {pendingUsername} has been created successfully. Status : {pendingRole}.\n\n(Press any key to continue)", ConsoleColor.DarkGreen);
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
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Loading");
            Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
            Dictionary<string, string> AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();

            string username = InputControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                print.QuasarScreen(currentUsername);
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    print.ColoredText($"\nDatabase does not contain a User {username}. Please select a different user.", ConsoleColor.DarkRed);
                }
                else
                {
                    print.ColoredText("\nCannot delete super_admin! Please choose a different user.", ConsoleColor.DarkRed);
                }
                Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
                AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
                username = InputControl.UsernameInput();
            }
            _db.RemoveUsernameFromDatabase(username);
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Deleting existing user in progress");
            _text.DeleteUserNotificationsLog(username);
            print.ColoredText($"\nUsername {username} has been successfully deleted from database.\n\n(Press any key to continue)", ConsoleColor.DarkGreen);
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static void ShowAvailableUsersFunction()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Loading");
            _db.ShowAvailableUsersFromDatabase();
            Console.Write("\r\nPress any key to return to Functions menu");
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        
        public static void AlterUserRoleStatus()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            print.QuasarScreen(currentUsername);
            print.UniversalLoadingOutput("Loading");
            Dictionary<string, string> AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
            Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
            string username = InputControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                print.QuasarScreen(currentUsername);
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    print.ColoredText($"\nDatabase does not contain a User {username}\n\n(Press any key to continue)", ConsoleColor.DarkRed);
                }
                else
                {
                    print.ColoredText("\nCannot alter super_admin's Status! Please choose a different user\n\n(Press any key to continue)", ConsoleColor.DarkRed);
                }
                Console.ReadKey();
                print.QuasarScreen(currentUsername);
                AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
                Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
                username = InputControl.UsernameInput();
            }
            string userRole = print.SelectUserRole();
            _db.SelectSingleUserRole(username, currentUsername, userRole);
        }
    }
}
