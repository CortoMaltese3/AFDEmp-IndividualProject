using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class SuperAdminFunctions
    {
        public static void CreateNewUserFromRequestFunction()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            string pendingUsername = DataToTextFile.GetPendingUsername();

            if (pendingUsername == " ")
            {
                ColorAndAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.Write("There are no pending requests.\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else
            {
                string pendingPassphrase = DataToTextFile.GetPendingPassphrase().Remove(0, 12);
                string yes = "Yes";
                string no = "No";
                string createUserMsg = $"\r\nYou are about to create a new username-password entry : {pendingUsername.Remove(0, 10)} - {pendingPassphrase}.\r\nWould you like to proceed?\r\n";
                string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, createUserMsg).option;

                if (yesOrNoSelection == yes)
                {
                    string pendingRole = OutputControl.SelectUserRole();

                    ConnectToServer.InsertNewUserIntoDatabase(pendingUsername, pendingPassphrase, pendingRole);
                    OutputControl.QuasarScreen(currentUsername);
                    ColorAndAnimationControl.UniversalLoadingOuput("Creating new user in progress");

                    DataToTextFile.ClearNewUserRegistrationList();
                    DataToTextFile.CreateNewUserLogFile(pendingUsername);

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
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
            Dictionary<string, string> AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();

            string username = InputControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                OutputControl.QuasarScreen(currentUsername);
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
                username = InputControl.UsernameInput();
            }
            ConnectToServer.RemoveUsernameFromDatabase(username);
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Deleting existing user in progress");
            DataToTextFile.DeleteUserNotificationsLog(username);
            Console.WriteLine($"Username {username} has been successfully deleted from database.\n\n(Press any key to continue)");
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static void ShowAvailableUsersFunction()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            ConnectToServer.ShowAvailableUsersFromDatabase();
            Console.Write("\r\nPress any key to return to Functions menu");
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        
        public static void AlterUserRoleStatus()
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            OutputControl.QuasarScreen(currentUsername);
            ColorAndAnimationControl.UniversalLoadingOuput("Loading");
            Dictionary<string, string> AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
            Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
            string username = InputControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                OutputControl.QuasarScreen(currentUsername);
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}\n\n(Press any key to continue)");
                }
                else
                {
                    Console.WriteLine("Cannot alter super_admin's Status! Please choose a different user\n\n(Press any key to continue)");
                }
                Console.ReadKey();
                OutputControl.QuasarScreen(currentUsername);
                AvailableUsernamesDictionary = ConnectToServer.ShowAvailableUsersFromDatabase();
                Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
                username = InputControl.UsernameInput();
            }
            string userRole = OutputControl.SelectUserRole();
            ConnectToServer.SelectSingleUserRole(username, currentUsername, userRole);
        }
    }
}
