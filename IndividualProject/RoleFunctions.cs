using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    class RoleFunctions
    {
        static string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();
        static string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();

        public static void CreateNewUserFromRequestFunction()
        {
            string pendingUsername = File.ReadLines(Globals.newUserRequestPath).First();
            if (pendingUsername == " ")
            {
                InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.Write("There are no pending requests.\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else
            {
                pendingUsername = pendingUsername.Remove(0, 10);

                string pendingPassphrase = File.ReadLines(Globals.newUserRequestPath).Skip(1).Take(1).First();
                pendingPassphrase = pendingPassphrase.Remove(0, 12);

                string createUserMsg = $"\r\nYou are about to create a new username-password entry : {pendingUsername} - {pendingPassphrase}.\r\nWould you like to proceed?\r\n";

                string yes = "Yes", no = "No";
                InputOutputAnimationControl.QuasarScreen(currentUsername);

                string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, createUserMsg).option;

                if (yesOrNoSelection == yes)
                {
                    string pendingRole = InputOutputAnimationControl.SelectUserRole();

                    using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                    {
                        dbcon.Open();
                        SqlCommand appendUserToDatabase = new SqlCommand("InsertNewUserIntoDatabase", dbcon);
                        appendUserToDatabase.CommandType = CommandType.StoredProcedure;
                        appendUserToDatabase.Parameters.AddWithValue("@username", pendingUsername);
                        appendUserToDatabase.Parameters.AddWithValue("@passphrase", pendingPassphrase);
                        appendUserToDatabase.Parameters.AddWithValue("@userRole", pendingRole);
                        appendUserToDatabase.ExecuteScalar();
                    }
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    InputOutputAnimationControl.UniversalLoadingOuput("Creating new user in progress");
                    //Clears the new user registrations List
                    File.WriteAllLines(Globals.newUserRequestPath, new string[] { " " });
                    //Creates a file for the new user to check notifications
                    File.WriteAllLines(Globals.TTnotificationToUser + pendingUsername + ".txt", new string[] { " " });
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
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
            Dictionary<string, string> AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();

            string username = InputOutputAnimationControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    Console.WriteLine($"Database does not contain a User {username}");
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
                    AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
                    username = InputOutputAnimationControl.UsernameInput();
                }
                else
                {
                    Console.WriteLine("Cannot delete super_admin! Please choose a different user.");
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    Console.WriteLine("\r\nChoose a User from the list and proceed to delete.");
                    AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
                    username = InputOutputAnimationControl.UsernameInput();
                }
            }

            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand deleteUsername = new SqlCommand("RemoveUsernameFromDatabase", dbcon);
                deleteUsername.CommandType = CommandType.StoredProcedure;
                deleteUsername.Parameters.AddWithValue("@username", username);
                deleteUsername.ExecuteNonQuery();
            }
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Deleting existing user in progress");
            Console.WriteLine($"Username {username} has been successfully deleted from database.\n\n(Press any key to continue)");
            Console.ReadKey();
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static Dictionary<string, string> ShowAvailableUsersFromDatabase()
        {
            Console.WriteLine("LIST OF USERS REGISTERED IN QUASAR\r\n");
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand ShowUsersFromDatabase = new SqlCommand("EXECUTE SelectUsersAndRolesInDatabase", dbcon);

                using (var reader = ShowUsersFromDatabase.ExecuteReader())
                {
                    Dictionary<string, string> AvailableUsernamesDictionary = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        var username = reader[0];
                        var status = reader[1];
                        AvailableUsernamesDictionary.Add((string)username, (string)status);
                        Console.WriteLine($"username: {username} - status: {status}");
                    }
                    return AvailableUsernamesDictionary;
                }
            }
        }

        public static void ShowAvailableUsersFunction()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            ShowAvailableUsersFromDatabase();
            Console.Write("\r\nPress any key to return to Functions menu");
            Console.ReadKey();
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }

        public static void CheckUserNotifications()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand CountOpenTicketsAssignedToUser = new SqlCommand("CountOpenTicketsAssignedToUser", dbcon);
                CountOpenTicketsAssignedToUser.CommandType = CommandType.StoredProcedure;
                CountOpenTicketsAssignedToUser.Parameters.AddWithValue("@userAssignedTo", currentUsername);

                SqlCommand OpenListOfTicketsAssignedToUser = new SqlCommand($"EXECUTE SelectOpenTicketsAssignedToUser '{currentUsername}'", dbcon);
                OpenListOfTicketsAssignedToUser.CommandType = CommandType.StoredProcedure;
                OpenListOfTicketsAssignedToUser.Parameters.AddWithValue("@userAssignedTo", currentUsername);
                int countTickets = (int)CountOpenTicketsAssignedToUser.ExecuteScalar();

                if (countTickets == 0)
                {
                    Console.WriteLine("\r\nThere are no notifications\n\n(Press any key to go back to Main Menu)");
                    Console.ReadKey();
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
                else
                {
                    string yes = "Yes", no = "No", openListMsg = $"There are [{countTickets}] open Trouble Tickets assigned to you.\r\nWould you like to open a list of the Tickets?";
                    string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, openListMsg).option;

                    if (yesOrNoSelection == yes)
                    {
                        using (var reader = OpenListOfTicketsAssignedToUser.ExecuteReader())
                        {
                            List<string> ShowtTicketsList = new List<string>();
                            while (reader.Read())
                            {
                                int ticketID = (int)reader[0];
                                DateTime dateCreated = (DateTime)reader[1];
                                string username = (string)reader[2];
                                string userAssignedTo = (string)reader[3];
                                string ticketStatus = (string)reader[4];
                                string comments = (string)reader[5];
                                var stringLength = comments.Length;
                                if (stringLength > 60)
                                {
                                    comments = comments.Substring(0, 60) + "...";
                                }

                                ShowtTicketsList.Add(ticketID.ToString());
                                ShowtTicketsList.Add(dateCreated.ToString());
                                ShowtTicketsList.Add(username);
                                ShowtTicketsList.Add(userAssignedTo);
                                ShowtTicketsList.Add(ticketStatus);
                                ShowtTicketsList.Add(comments);
                                Console.WriteLine($"TicketID: {ticketID} \r\nDate created: {dateCreated} \r\nCreated By: {username} \r\nAssigned To: {userAssignedTo} \r\nTicket status: {ticketStatus} \r\bComment preview: {comments}");
                                Console.WriteLine(new string('#', Console.WindowWidth));
                                Console.WriteLine();
                            }
                        }
                        Console.WriteLine("(Press any key to go back to Main Menu)");
                        Console.ReadKey();
                        ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                    }
                    else if (yesOrNoSelection == no)
                    {
                        ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                    }
                }
            }
        }

        public static void CheckAdminNotifications()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            string pendingUsernameCheck = File.ReadLines(Globals.newUserRequestPath).First();

            if (pendingUsernameCheck == " ")
            {
                Console.WriteLine("\r\nThere are no pending User registrations\n\n(Press any key to continue)");
                Console.ReadKey();
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
            else
            {
                string yes = "Yes", no = "No";

                string requestMsg = "\r\nYou have 1 pending User registration request. Would you like to create new user?\n";
                string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, requestMsg).option;

                if (yesOrNoSelection == yes)
                {
                    CreateNewUserFromRequestFunction();

                }

                else if (yesOrNoSelection == no)
                {
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        public static void AlterUserRoleStatus()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Dictionary<string, string> AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
            Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");

            string username = InputOutputAnimationControl.UsernameInput();

            while (AvailableUsernamesDictionary.ContainsKey(username) == false || username == "admin")
            {
                if (AvailableUsernamesDictionary.ContainsKey(username) == false)
                {
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    Console.WriteLine($"Database does not contain a User {username}\n\n(Press any key to continue)");
                    Console.ReadKey();
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
                    Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
                    username = InputOutputAnimationControl.UsernameInput();
                }
                else
                {
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    Console.WriteLine("Cannot alter super_admin's Status! Please choose a different user\n\n");
                    Console.ReadKey();
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    AvailableUsernamesDictionary = ShowAvailableUsersFromDatabase();
                    Console.WriteLine("\r\nChoose a User from the list and proceed to upgrade/downgrade Role Status");
                    username = InputOutputAnimationControl.UsernameInput();
                }
            }
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            string userRole = InputOutputAnimationControl.SelectUserRole();

            InputOutputAnimationControl.QuasarScreen(currentUsername);

            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand selectPreviousUserRole = new SqlCommand("SelectSingleUserRole", dbcon);
                selectPreviousUserRole.CommandType = CommandType.StoredProcedure;
                selectPreviousUserRole.Parameters.AddWithValue("@username", username);
                string previousUserRole = (string)selectPreviousUserRole.ExecuteScalar();
                while (previousUserRole == userRole)
                {
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    Console.WriteLine();
                    Console.WriteLine($"User '{username}' already is {userRole}. Please proceed to choose a different Role Status\n\n(Press any key to continue)");
                    Console.ReadKey();
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    Console.WriteLine();
                    userRole = InputOutputAnimationControl.SelectUserRole();
                    selectPreviousUserRole = new SqlCommand("SelectSingleUserRole", dbcon);
                    selectPreviousUserRole.CommandType = CommandType.StoredProcedure;
                    selectPreviousUserRole.Parameters.AddWithValue("@username", username);
                    previousUserRole = (string)selectPreviousUserRole.ExecuteScalar();
                }

                SqlCommand alterUserRole = new SqlCommand("UpdateUserRole", dbcon);
                alterUserRole.CommandType = CommandType.StoredProcedure;
                alterUserRole.Parameters.AddWithValue("@username", username);
                alterUserRole.Parameters.AddWithValue("@userRole", userRole);
                SqlCommand selectUserRole = new SqlCommand("SelectSingleUserRole", dbcon);
                selectUserRole.CommandType = CommandType.StoredProcedure;
                selectUserRole.Parameters.AddWithValue("@username", username);
                alterUserRole.ExecuteScalar();
                string newUserRole = (string)selectUserRole.ExecuteScalar();
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Modifying User's role status in progress");
                Console.WriteLine($"User {username} has been successfully modified as {newUserRole}\n\n(Press any key to continue)");
            }
            Console.ReadKey();
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        }
    }
}
