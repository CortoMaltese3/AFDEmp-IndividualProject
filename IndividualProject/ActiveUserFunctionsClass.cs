using System;

namespace IndividualProject
{
    class ActiveUserFunctionsClass
    {
        public static void ActiveUserProcedures()
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            string currentUsernameRole = ConnectToServerClass.RetrieveCurrentUsernameRoleFromDatabase();
            string currentUserStatus = ConnectToServerClass.RetrieveCurrentUserStatusFromDatabase();

            if (currentUserStatus == "inactive")
            {
                ConnectToServerClass.UserLoginCredentials();
                currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
                currentUsernameRole = ConnectToServerClass.RetrieveCurrentUsernameRoleFromDatabase();
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                UserFunctionSwitch(currentUsernameRole);
            }
            else
            {
                currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
                currentUsernameRole = ConnectToServerClass.RetrieveCurrentUsernameRoleFromDatabase();
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                UserFunctionSwitch(currentUsernameRole);
            }
        }

        //public static void MainMenuScreen(object sender, ConsoleCancelEventArgs args)
        //{
        //    string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
        //    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
        //    InputOutputAnimationControlClass.UniversalLoadingOuput("Force exit to Main Menu Screen");
        //    ActiveUserProcedures();
        //    args.Cancel = true;
        //}

        public static void UserFunctionSwitch(string currentUsernameRole)
        {
            string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
            switch (currentUsernameRole)
            {
                case "super_admin":
                    ConsoleKey function = InputOutputAnimationControlClass.AdminFunctionOptionsOutput();

                    switch (function)
                    {
                        case ConsoleKey.D1:
                            RoleFunctionsClass.CheckAdminNotifications();
                            break;

                        case ConsoleKey.D2:
                            RoleFunctionsClass.CreateNewUserFromRequestFunction();
                            break;

                        case ConsoleKey.D3:                           
                            RoleFunctionsClass.ShowAvailableUsersFunction();                            
                            break;

                        case ConsoleKey.D4:                            
                            RoleFunctionsClass.AlterUserRoleStatus();
                            break;

                        case ConsoleKey.D5:
                            RoleFunctionsClass.DeleteUserFromDatabase();
                            break;
                           
                        case ConsoleKey.D6:
                            TransactedDataClass.ManageCustomerTickets();
                            break;
                            
                        case ConsoleKey.D7:
                            TransactedDataClass.ViewExistingOpenTicketsFunction();
                            break;

                        case ConsoleKey.D8:
                            TransactedDataClass.EditExistingOpenTicketFunction();
                            break;

                        case ConsoleKey.D9:
                            TransactedDataClass.DeleteExistingOpenOrClosedTicketFunction();
                            break;

                        case ConsoleKey.Escape:
                            ConnectToServerClass.LoggingOffQuasar();
                            break;
                    }
                    break;

                case "Administrator":
                    ConsoleKey function_admin = InputOutputAnimationControlClass.AdministratorFunctionOptionsOutput();

                    switch (function_admin)
                    {
                        case ConsoleKey.D1:
                            RoleFunctionsClass.CheckUserNotifications();
                            break;

                        case ConsoleKey.D2:
                            TransactedDataClass.ManageCustomerTickets();
                            break;

                        case ConsoleKey.D3:
                            TransactedDataClass.ViewExistingOpenTicketsFunction();
                            break;

                        case ConsoleKey.D4:
                            TransactedDataClass.EditExistingOpenTicketFunction();
                            break;

                        case ConsoleKey.D5:
                            TransactedDataClass.DeleteExistingOpenOrClosedTicketFunction();
                            break;

                        case ConsoleKey.Escape:
                            ConnectToServerClass.LoggingOffQuasar();
                            break;
                    }
                    break;

                case "Moderator":
                    function = InputOutputAnimationControlClass.ModeratorFunctionOptionsOutput();
                    switch (function)
                    {
                        case ConsoleKey.D1:
                            RoleFunctionsClass.CheckUserNotifications();
                            break;

                        case ConsoleKey.D2:
                            TransactedDataClass.ManageCustomerTickets();
                            break;

                        case ConsoleKey.D3:
                            TransactedDataClass.ViewExistingOpenTicketsFunction();
                            break;

                        case ConsoleKey.D4:
                            TransactedDataClass.EditExistingOpenTicketFunction();
                            break;

                        case ConsoleKey.Escape:
                            ConnectToServerClass.LoggingOffQuasar();
                            break;
                    }
                    break;

                case "User":
                    function = InputOutputAnimationControlClass.UserFunctionOptionsOutput();
                    switch (function)
                    {
                        case ConsoleKey.D1:
                            RoleFunctionsClass.CheckUserNotifications();
                            break;

                        case ConsoleKey.D2:
                            TransactedDataClass.OpenCustomerTickets();
                            break;

                        case ConsoleKey.D3:
                            TransactedDataClass.ViewExistingOpenTicketsFunction();
                            break;

                        case ConsoleKey.Escape:
                            ConnectToServerClass.LoggingOffQuasar();
                            break;
                    }
                    break;
            }
        }
    }
}
