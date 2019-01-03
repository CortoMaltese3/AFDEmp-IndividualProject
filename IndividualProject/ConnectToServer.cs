using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace IndividualProject
{
    public static class Globals
    {
        public static readonly string connectionString = "Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        public static readonly string newUserRequestPath = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\NewUserRequests\NewUserRequest.txt";
        public static readonly string TTnotificationToUser = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\CRMTickets\TechnicalIssues\TroubleTicketNotificationToUser_";
    }

    static class ConnectToServer
    {
        public static void UserLoginCredentials()
        {
            InputOutputAnimationControl.QuasarScreen("Not Registered");
            string username = InputOutputAnimationControl.UsernameInput();
            string passphrase = InputOutputAnimationControl.PassphraseInput();
            var dbcon = new SqlConnection(Globals.connectionString);

            while (TestConnectionToSqlServer(dbcon))
            {
                if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                {
                    SetCurrentUserStatusToActive(username);
                    InputOutputAnimationControl.QuasarScreen(username);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"Connection Established! Welcome back {username}!");
                    System.Threading.Thread.Sleep(1500);
                    ActiveUserFunctions.UserFunctionMenuScreen(RetrieveCurrentUsernameRoleFromDatabase());
                }
                else
                {
                    InputOutputAnimationControl.QuasarScreen("Not Registered");
                    Console.Write($"\r\nInvalid Username or Passphrase. Try again.\n\n(press any key to continue)");
                    Console.ReadKey();
                    UserLoginCredentials();
                }
            }
        }

        private static bool TestConnectionToSqlServer(this SqlConnection connectionString)
        {
            try
            {
                connectionString.Open();
                connectionString.Close();
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        private static bool CheckUsernameAndPasswordMatchInDatabase(string usernameCheck, string passphraseCheck)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand checkUsername = new SqlCommand("CheckUniqueCredentials", dbcon);
                checkUsername.CommandType = CommandType.StoredProcedure;
                checkUsername.Parameters.AddWithValue("@usernameCheck", usernameCheck);
                checkUsername.Parameters.AddWithValue("@passphraseCheck", passphraseCheck);
                int UserCount = (int)checkUsername.ExecuteScalar();
                if (UserCount != 0)
                {
                    return true;
                }
                return false;
            }
        }

        private static void SetCurrentUserStatusToActive(string currentUsername)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand SetStatusToActive = new SqlCommand($"SetCurrentUserStatusToActive", dbcon);
                SetStatusToActive.CommandType = CommandType.StoredProcedure;
                SetStatusToActive.Parameters.AddWithValue("@username", currentUsername);
                SetStatusToActive.ExecuteScalar();
            }
        }

        private static void SetCurrentUserStatusToInactive(string currentUsername)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand SetStatusToInactive = new SqlCommand("EXECUTE SetCurrentUserStatusToInactive", dbcon);
                SetStatusToInactive.ExecuteScalar();
            }
        }

        public static string RetrieveCurrentUserFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveLoginCredentials = new SqlCommand($"EXECUTE SelectCurrentUserFromDatabase", dbcon);
                string currentUsername = (string)RetrieveLoginCredentials.ExecuteScalar();
                return currentUsername;
            }
        }

        public static string RetrieveCurrentUsernameRoleFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveCurrentUsernameRole = new SqlCommand("EXECUTE SelectCurrentUserRoleFromDatabase", dbcon);
                string currentRole = (string)RetrieveCurrentUsernameRole.ExecuteScalar();
                return currentRole;
            }
        }

        private static string RetrieveCurrentUserStatusFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveCurrentUserStatus = new SqlCommand("EXECUTE SelectCurrentUserStatusFromDatabase", dbcon);
                string currentUserStatus = (string)RetrieveCurrentUserStatus.ExecuteScalar();
                return currentUserStatus;
            }
        }

        public static bool CheckUsernameAvailabilityInDatabase(string usernameCheck)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand checkUsername = new SqlCommand("CheckUniqueUsername", dbcon);
                checkUsername.CommandType = System.Data.CommandType.StoredProcedure;
                checkUsername.Parameters.AddWithValue("@usernameCheck", usernameCheck);
                int UserCount = (int)checkUsername.ExecuteScalar();
                if (UserCount != 0)
                {
                    return false;
                }
                return true;
            }
        }

        public static void InsertNewUserIntoDatabase(string pendingUsername, string pendingPassphrase, string pendingRole)
        {
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
        }

        public static void RemoveUsernameFromDatabase(string username)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand deleteUsername = new SqlCommand("RemoveUsernameFromDatabase", dbcon);
                deleteUsername.CommandType = CommandType.StoredProcedure;
                deleteUsername.Parameters.AddWithValue("@username", username);
                deleteUsername.ExecuteNonQuery();
            }
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

        public static int CountOpenTicketsAssignedToUser(string currentUsername)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand CountOpenTicketsAssignedToUser = new SqlCommand("CountOpenTicketsAssignedToUser", dbcon);
                CountOpenTicketsAssignedToUser.CommandType = CommandType.StoredProcedure;
                CountOpenTicketsAssignedToUser.Parameters.AddWithValue("@userAssignedTo", currentUsername);
                int countTicketsAssingedToUser = (int)CountOpenTicketsAssignedToUser.ExecuteScalar();
                return countTicketsAssingedToUser;
            }
        }

        public static void SelectOpenTicketsAssignedToUser(string currentUsername)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand OpenListOfTicketsAssignedToUser = new SqlCommand("SelectOpenTicketsAssignedToUser", dbcon);
                OpenListOfTicketsAssignedToUser.CommandType = CommandType.StoredProcedure;
                OpenListOfTicketsAssignedToUser.Parameters.AddWithValue("@userAssignedTo", currentUsername);

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
            }
        }

        public static void SelectSingleUserRole(string username, string currentUsername, string userRole)
        {
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
                Console.ReadKey();
            }
        }

        public static void OpenNewTechnicalTicket(string currentUsername, string userAssignedTo, string comment)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand openNewTechnicalTicket = new SqlCommand("OpenNewTechnicalTicket", dbcon);
                openNewTechnicalTicket.CommandType = CommandType.StoredProcedure;
                openNewTechnicalTicket.Parameters.AddWithValue("@username", currentUsername);
                openNewTechnicalTicket.Parameters.AddWithValue("@userAssignedTo", userAssignedTo);
                openNewTechnicalTicket.Parameters.AddWithValue("@comments", comment);
                openNewTechnicalTicket.ExecuteNonQuery();

                SqlCommand fetchNewTicketID = new SqlCommand("EXECUTE fetchNewTicketID", dbcon);
                int ticketID = (int)fetchNewTicketID.ExecuteScalar();
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Filing new customer ticket in progress");
                Console.WriteLine($"New Customer Ticket with ID: {ticketID} has been successfully created and assigned to {userAssignedTo}. Status: Open");
            }
        }

        public static void SetTicketStatusToClosed(string currentUsername, int ticketID)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand closeCustomerTicket = new SqlCommand($"EXECUTE SetTicketStatusToClosed {ticketID}", dbcon);
                closeCustomerTicket.ExecuteScalar();
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully marked as closed.\n\n(Press any key to continue)");
                Console.ReadKey();
            }
        }

        public static void EditCommentOfOpenTicket(int ID, string ticketComment)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand EditTicketCommendInDatabase = new SqlCommand($"EditCustomerTicketCommentSection '{ticketComment}', {ID}", dbcon);
                EditTicketCommendInDatabase.ExecuteScalar();
            }
            Console.WriteLine($"The comment section of the Customer Ticket with [ID = {ID}] has been successfully edited\n\n(Press any key yo continue)");
            Console.ReadKey();
        }

        public static void SelectSingleCustomerTicket(int ticketID)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand ShowTicketsFromDatabase = new SqlCommand("SelectSingleCustomerTicket", dbcon);
                ShowTicketsFromDatabase.CommandType = CommandType.StoredProcedure;
                ShowTicketsFromDatabase.Parameters.AddWithValue("@ticketID", ticketID);
                using (var reader = ShowTicketsFromDatabase.ExecuteReader())
                {
                    List<string> ShowtTicketToList = new List<string>();
                    while (reader.Read())
                    {
                        int ID = (int)reader[0];
                        DateTime dateCreated = (DateTime)reader[1];
                        string username = (string)reader[2];
                        string userAssignedTo = (string)reader[3];
                        string ticketStatus = (string)reader[4];
                        string comments = (string)reader[5];

                        ShowtTicketToList.Add(ticketID.ToString());
                        ShowtTicketToList.Add(dateCreated.ToString());
                        ShowtTicketToList.Add(username);
                        ShowtTicketToList.Add(userAssignedTo);
                        ShowtTicketToList.Add(ticketStatus);
                        ShowtTicketToList.Add(comments);
                        Console.WriteLine($"TicketID: {ticketID} \r\nDate created: {dateCreated} \r\nCreated By: {username} \r\nAssigned To: {userAssignedTo} \r\nTicket status: {ticketStatus} \r\bComment preview: {comments}");
                        Console.WriteLine(new string('#', Console.WindowWidth));
                    }
                }
            }
        }

        public static void ViewListOfOpenCustomerTickets()
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand ShowTicketsFromDatabase = new SqlCommand("EXECUTE SelectOpenCustomerTickets", dbcon);
                using (var reader = ShowTicketsFromDatabase.ExecuteReader())
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
            }
        }

        public static void ViewListOfAllCustomerTickets()
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand ShowTicketsFromDatabase = new SqlCommand("SELECT * FROM CustomerTickets", dbcon);
                using (var reader = ShowTicketsFromDatabase.ExecuteReader())
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
                        if (stringLength > 40)
                        {
                            comments = comments.Substring(0, 40) + "...";
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
            }
        }

        public static void DeleteCustomerTicket(string currentUsername, int ticketID)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand deleteCustomerTicket = new SqlCommand("DeleteCustomerTicket", dbcon);
                deleteCustomerTicket.CommandType = CommandType.StoredProcedure;
                deleteCustomerTicket.Parameters.AddWithValue("@ticketID", ticketID);
                deleteCustomerTicket.ExecuteScalar();
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully deleted\n\n(Press any key to continue)");
                Console.ReadKey();
            }
        }

        public static bool CheckIfTicketIDWithStatusOpenExistsInList(int ID)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand ShowTicketsFromDatabase = new SqlCommand("EXECUTE SelectTicketIDWithOpenStatus", dbcon);
                using (var reader = ShowTicketsFromDatabase.ExecuteReader())
                {
                    List<string> ShowtTicketsList = new List<string>();
                    while (reader.Read())
                    {
                        int ticketID = (int)reader[0];
                        ShowtTicketsList.Add(ticketID.ToString());
                    }
                    if (ShowtTicketsList.Contains(ID.ToString()) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void ChangeUserAssignedTo(string nextOwner, int ID)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand EditTicketUserOwnerInDatabase = new SqlCommand("ChangeUserAssignedTo", dbcon);
                EditTicketUserOwnerInDatabase.CommandType = CommandType.StoredProcedure;
                EditTicketUserOwnerInDatabase.Parameters.AddWithValue("@username", nextOwner);
                EditTicketUserOwnerInDatabase.Parameters.AddWithValue("@ID", ID);
                EditTicketUserOwnerInDatabase.ExecuteScalar();
            }
        }

        public static bool CheckIfTicketIDWithStatusOpenOrClosedExistsInList(int ID)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand ShowTicketsFromDatabase = new SqlCommand("SELECT ticketID FROM CustomerTickets", dbcon);
                using (var reader = ShowTicketsFromDatabase.ExecuteReader())
                {
                    List<string> ShowtTicketsList = new List<string>();
                    while (reader.Read())
                    {
                        int ticketID = (int)reader[0];
                        ShowtTicketsList.Add(ticketID.ToString());
                    }
                    if (ShowtTicketsList.Contains(ID.ToString()) == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void TerminateQuasar()
        {
            string yes = "Yes", no = "No", currentUsername = "Not Registered", exitMessage = "\r\nWould you like to exit Quasar?\r\n";
            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, exitMessage).option;

            if (yesOrNoSelection == yes)
            {
                SetCurrentUserStatusToInactive(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Wait for Quasar to shut down");
                InputOutputAnimationControl.SpecialThanksMessage();
                Environment.Exit(0);
            }
            else if (yesOrNoSelection == no)
            {
                ApplicationMenu.LoginScreen();
            }
        }

        public static void LoggingOffQuasar()
        {
            string yes = "Yes", no = "No", logOffMessage = "Would you like to log out?\r\n", currentUsername = RetrieveCurrentUserFromDatabase();
            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, logOffMessage).option;

            if (yesOrNoSelection == yes)
            {
                InputOutputAnimationControl.QuasarScreen("Not Registered");
                SetCurrentUserStatusToInactive(currentUsername);
                ApplicationMenu.LoginScreen();

            }
            else if (yesOrNoSelection == no)
            {
                ActiveUserFunctions.UserFunctionMenuScreen(RetrieveCurrentUsernameRoleFromDatabase());
            }
        }
    }
}
