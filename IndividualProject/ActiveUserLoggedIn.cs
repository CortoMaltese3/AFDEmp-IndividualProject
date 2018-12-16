using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    class ActiveUserLoggedIn
    {
        public static void ActiveUserProcedures()
        {
            if (ConnectToServerClass.UserLoginCredentials())
            {
                string currentUsername = ConnectToServerClass.RetrieveCurrentLoginCredentialsFromDatabase();
                string currentUsernameRole = ConnectToServerClass.RetrieveCurrentUsernameRoleFromDatabase();
                switch (currentUsernameRole)
                {
                    case "super_admin":
                        ConsoleKey function = ConsoleOutputAndAnimations.AdminFunctionOptionsOutput();
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
                                InputOutputControlClass.ClearScreen();
                                ApplicationMenuClass.LoginScreen();
                                break;
                            
                            //upgrade-downgrade user                            
                            case ConsoleKey.D4:

                                break;
                            
                            //delete username from db
                            case ConsoleKey.D5:
                                RoleFunctionsClass.DeleteUserFromDatabase();
                                break;
                            
                            //view transacted data                                
                            case ConsoleKey.D6:
                                
                                break;
                            
                            //edit transacted data
                            case ConsoleKey.D7:
                                
                                break;
                            
                            //delete transcted data
                            case ConsoleKey.D8:
                                
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
