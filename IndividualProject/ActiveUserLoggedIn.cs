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
                        Console.WriteLine("\r\nChoose one of the followin functions:");
                        Console.WriteLine("1: Create new username/password from requests");
                        Console.WriteLine("2: View the transacted data between users");
                        Console.WriteLine("3: Edit the transacted data between users");
                        Console.WriteLine("4: Delete the transacted data between users");
                        Console.WriteLine("5: Delete an active username from Database");
                        ConsoleKey function = Console.ReadKey().Key; 
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
