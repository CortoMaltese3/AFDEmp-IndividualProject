using System;

namespace IndividualProject
{
    class ActiveUserFunctions
    {   
        public static void UserFunctionMenuScreen(string currentUsernameRole)
        {
            string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
            switch (currentUsernameRole)
            {
                case "super_admin":
                    ConsoleKey function = InputOutputAnimationControl.AdminFunctionOptionsOutput();

                    switch (function)
                    {
                        case ConsoleKey.D1:
                            RoleFunctions.CheckAdminNotifications();
                            break;

                        case ConsoleKey.D2:
                            RoleFunctions.CreateNewUserFromRequestFunction();
                            break;

                        case ConsoleKey.D3:                           
                            RoleFunctions.ShowAvailableUsersFunction();                            
                            break;

                        case ConsoleKey.D4:                            
                            RoleFunctions.AlterUserRoleStatus();
                            break;

                        case ConsoleKey.D5:
                            RoleFunctions.DeleteUserFromDatabase();
                            break;
                           
                        case ConsoleKey.D6:
                            TransactedData.ManageCustomerTickets();
                            break;
                            
                        case ConsoleKey.D7:
                            TransactedData.ViewExistingOpenTicketsFunction();
                            break;

                        case ConsoleKey.D8:
                            TransactedData.EditExistingOpenTicketFunction();
                            break;

                        case ConsoleKey.D9:
                            TransactedData.DeleteExistingOpenOrClosedTicketFunction();
                            break;

                        case ConsoleKey.Escape:
                            ConnectToServer.LoggingOffQuasar();
                            break;
                    }
                    break;

                case "Administrator":
                    ConsoleKey function_admin = InputOutputAnimationControl.AdministratorFunctionOptionsOutput();

                    switch (function_admin)
                    {
                        case ConsoleKey.D1:
                            RoleFunctions.CheckUserNotifications();
                            break;

                        case ConsoleKey.D2:
                            TransactedData.ManageCustomerTickets();
                            break;

                        case ConsoleKey.D3:
                            TransactedData.ViewExistingOpenTicketsFunction();
                            break;

                        case ConsoleKey.D4:
                            TransactedData.EditExistingOpenTicketFunction();
                            break;

                        case ConsoleKey.D5:
                            TransactedData.DeleteExistingOpenOrClosedTicketFunction();
                            break;

                        case ConsoleKey.Escape:
                            ConnectToServer.LoggingOffQuasar();
                            break;
                    }
                    break;

                case "Moderator":
                    function = InputOutputAnimationControl.ModeratorFunctionOptionsOutput();
                    switch (function)
                    {
                        case ConsoleKey.D1:
                            RoleFunctions.CheckUserNotifications();
                            break;

                        case ConsoleKey.D2:
                            TransactedData.ManageCustomerTickets();
                            break;

                        case ConsoleKey.D3:
                            TransactedData.ViewExistingOpenTicketsFunction();
                            break;

                        case ConsoleKey.D4:
                            TransactedData.EditExistingOpenTicketFunction();
                            break;

                        case ConsoleKey.Escape:
                            ConnectToServer.LoggingOffQuasar();
                            break;
                    }
                    break;

                case "User":
                    function = InputOutputAnimationControl.UserFunctionOptionsOutput();
                    switch (function)
                    {
                        case ConsoleKey.D1:
                            RoleFunctions.CheckUserNotifications();
                            break;

                        case ConsoleKey.D2:
                            TransactedData.OpenCustomerTickets();
                            break;

                        case ConsoleKey.D3:
                            TransactedData.ViewExistingOpenTicketsFunction();
                            break;

                        case ConsoleKey.Escape:
                            ConnectToServer.LoggingOffQuasar();
                            break;
                    }
                    break;
            }
        }
    }
}
