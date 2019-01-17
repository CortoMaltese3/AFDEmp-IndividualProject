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
                ColorAndAnimationControl.UniversalLoadingOuput("Action in progress");
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
                string createUserMsg = $"\r\nYou are about to create a new username-password entry : {pendingUsername} - {pendingPassphrase}.\r\nWould you like to proceed?\r\n";
                string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, createUserMsg).option;

                if (yesOrNoSelection == yes)
                {
                    string pendingRole = OutputControl.SelectUserRole();

                    _db.InsertNewUserIntoDatabase(pendingUsername, pendingPassphrase, pendingRole);
                    print.QuasarScreen(currentUsername);
                    ColorAndAnimationControl.UniversalLoadingOuput("Creating new user in progress");

                    _text.ClearNewUserRegistrationList();
                    _text.CreateNewUserLogFile(pendingUsername);

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
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            print.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
            Dictionary<string, string> AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();

            string username = InputControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                print.QuasarScreen(currentUsername);
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}. Please select a different user.");
                }
                else
                {
                    Console.WriteLine("Cannot delete super_admin! Please choose a different user.");
                }
                Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
                AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
                username = InputControl.UsernameInput();
            }
            _db.RemoveUsernameFromDatabase(username);
            print.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Deleting existing user in progress");
            _text.DeleteUserNotificationsLog(username);
            Console.WriteLine($"Username {username} has been successfully deleted from database.\n\n(Press any key to continue)");
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static void ShowAvailableUsersFunction()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = _db.RetrieveCurrentUsernameRoleFromDatabase();
            print.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            _db.ShowAvailableUsersFromDatabase();
            Console.Write("\r\nPress any key to return to Functions menu");
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        
        public static void AlterUserRoleStatus()
        {
            string currentUsername = _db.RetrieveCurrentUserFromDatabase();
            print.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Dictionary<string, string> AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
            Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
            string username = InputControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                print.QuasarScreen(currentUsername);
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}\n\n(Press any key to continue)");
                }
                else
                {
                    Console.WriteLine("Cannot alter super_admin's Status! Please choose a different user\n\n(Press any key to continue)");
                }
                Console.ReadKey();
                print.QuasarScreen(currentUsername);
                AvailableUsernamesDictionary = _db.ShowAvailableUsersFromDatabase();
                Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
                username = InputControl.UsernameInput();
            }
            string userRole = OutputControl.SelectUserRole();
            _db.SelectSingleUserRole(username, currentUsername, userRole);
        }
    }
}
