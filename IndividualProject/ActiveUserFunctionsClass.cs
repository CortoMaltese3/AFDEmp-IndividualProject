using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class ActiveUserFunctionsClass
    {
        public static void ActiveUserProcedures()
        {
            if (ConnectToServerClass.RetrieveCurrentUserStatusFromDatabase() == "inactive")
            {
                ConnectToServerClass.UserLoginCredentials();
                InputOutputAnimationControlClass.QuasarScreen("Not registered");
                string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
                string currentUsernameRole = ConnectToServerClass.RetrieveCurrentUsernameRoleFromDatabase();
                switch (currentUsernameRole)
                {
                    case "super_admin":
                        ConsoleKey function = InputOutputAnimationControlClass.AdminFunctionOptionsOutput();

                        switch (function)
                        {
                            //Check Inbox
                            case ConsoleKey.D1:
                                RoleFunctionsClass.CheckAdminNotifications();
                                break;

                            //create new user from pending list
                            case ConsoleKey.D2:
                                RoleFunctionsClass.CreateNewUserFromRequestFunction();
                                break;

                            //show user list
                            case ConsoleKey.D3:
                                Console.WriteLine();
                                RoleFunctionsClass.ShowAvailableUsersFromDatabase();
                                //InputOutputAnimationControlClass.ClearScreen();
                                //ApplicationMenuClass.LoginScreen();
                                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                                ActiveUserProcedures();
                                break;

                            //upgrade-downgrade user                            
                            case ConsoleKey.D4:
                                Console.WriteLine();
                                RoleFunctionsClass.AlterUserRoleStatus();
                                break;

                            //delete username from db
                            case ConsoleKey.D5:
                                RoleFunctionsClass.DeleteUserFromDatabase();
                                break;

                            //create new customer ticket                               
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
            else
            {
                string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
                string currentUsernameRole = ConnectToServerClass.RetrieveCurrentUsernameRoleFromDatabase();
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                switch (currentUsernameRole)
                {
                    case "super_admin":
                        ConsoleKey function = InputOutputAnimationControlClass.AdminFunctionOptionsOutput();
                        switch (function)
                        {
                            //Check Inbox
                            case ConsoleKey.D1:
                                RoleFunctionsClass.CheckAdminNotifications();
                                break;

                            //create new user from pending list
                            case ConsoleKey.D2:
                                RoleFunctionsClass.CreateNewUserFromRequestFunction();
                                break;

                            //show user list
                            case ConsoleKey.D3:
                                Console.WriteLine();
                                RoleFunctionsClass.ShowAvailableUsersFromDatabase();
                                InputOutputAnimationControlClass.ClearScreen();
                                ApplicationMenuClass.LoginScreen();
                                break;

                            //upgrade-downgrade user                            
                            case ConsoleKey.D4:
                                Console.WriteLine();
                                RoleFunctionsClass.AlterUserRoleStatus();
                                break;

                            //delete username from db
                            case ConsoleKey.D5:
                                RoleFunctionsClass.DeleteUserFromDatabase();
                                break;

                            //create new customer ticket                               
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
}
