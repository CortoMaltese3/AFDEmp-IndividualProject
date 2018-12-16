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
                            case ConsoleKey.D1:
                                RoleFunctionsClass.CreateNewUserFromRequestFunction();
                                break;

                            case ConsoleKey.D2:

                                break;

                            case ConsoleKey.D3:

                                break;

                            case ConsoleKey.D4:

                                break;

                            case ConsoleKey.D5:
                                RoleFunctionsClass.DeleteUserFromDatabase();
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
