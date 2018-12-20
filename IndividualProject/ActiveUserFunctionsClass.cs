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
                            Console.WriteLine();
                            RoleFunctionsClass.ShowAvailableUsersFromDatabase();
                            Console.WriteLine("Press any key to return to Functions menu");
                            Console.ReadKey();
                            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                            ActiveUserProcedures();
                            break;

                        case ConsoleKey.D4:
                            Console.WriteLine();
                            RoleFunctionsClass.AlterUserRoleStatus();
                            break;

                        case ConsoleKey.D5:
                            RoleFunctionsClass.DeleteUserFromDatabase();
                            break;
                           
                        case ConsoleKey.D6:
                            TransactedDataClass.OpenNewCustomerTicket();
                            break;

                        //view transacted data                                
                        case ConsoleKey.D7:

                            break;

                        //edit transacted data
                        case ConsoleKey.D8:
                            TransactedDataClass.CloseCustomerTicket();
                            break;

                        //delete transcted data
                        case ConsoleKey.D9:

                            break;

                        case ConsoleKey.Escape:
                            ConnectToServerClass.LoggingOffQuasar();
                            break;
                    }
                    break;

                case "administrator":

                    break;

                case "moderator":

                    break;

                case "user":

                    break;
            }
        }
    }
}
