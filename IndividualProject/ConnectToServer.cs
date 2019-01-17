using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace IndividualProject
{
    
    //The ConnectToServer class handles the interactions with the Database
    public class ConnectToServer
    {
        static OutputControl print = new OutputControl();

        public void UserLoginCredentials()
        {
            print.QuasarScreen("Not Registered");
            string username = InputControl.UsernameInput();
            string passphrase = InputControl.PassphraseInput();
            var dbcon = new SqlConnection(Globals.connectionString);

            while (TestConnectionToSqlServer(dbcon))
            {
                if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                {
                    SetCurrentUserStatusToActive(username);
                    print.QuasarScreen(username);
                    print.ColoredText($"Connection Established! Welcome back {username}!", ConsoleColor.DarkGreen);
                    System.Threading.Thread.Sleep(1500);
                    ActiveUserFunctions.UserFunctionMenuScreen(RetrieveCurrentUsernameRoleFromDatabase());
                }
                else
                {
                    print.QuasarScreen("Not Registered");
                    print.ColoredText($"\r\nInvalid Username or Passphrase. Try again.\n\n(press any key to continue)", ConsoleColor.DarkRed);
                    Console.ReadKey();
                    UserLoginCredentials();
                }
            }
        }

        private bool TestConnectionToSqlServer(SqlConnection connectionString)
        {
            try
            {
                connectionString.Open();
                connectionString.Close();
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
            return true;
        }

        private bool CheckUsernameAndPasswordMatchInDatabase(string usernameCheck, string passphraseCheck)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                try
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
                }
                catch (SqlException exc)
                {
                    Console.WriteLine(exc.Message);
                }
                return false;
            }
        }

        private void SetCurrentUserStatusToActive(string currentUsername)
        {
            try
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
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }

        }

        private void SetCurrentUserStatusToInactive(string currentUsername)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    dbcon.Open();
                    SqlCommand SetStatusToInactive = new SqlCommand("EXECUTE SetCurrentUserStatusToInactive", dbcon);
                    SetStatusToInactive.ExecuteScalar();
                }
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public string RetrieveCurrentUserFromDatabase()
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    dbcon.Open();
                    SqlCommand RetrieveLoginCredentials = new SqlCommand($"EXECUTE SelectCurrentUserFromDatabase", dbcon);
                    string currentUsername = (string)RetrieveLoginCredentials.ExecuteScalar();
                    return currentUsername;
                }
            }
            catch (SqlException)
            {
                return "There has been an unexpected error while trying to connect to the DataBase";
            }

        }

        public string RetrieveCurrentUsernameRoleFromDatabase()
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    dbcon.Open();
                    SqlCommand RetrieveCurrentUsernameRole = new SqlCommand("EXECUTE SelectCurrentUserRoleFromDatabase", dbcon);
                    string currentRole = (string)RetrieveCurrentUsernameRole.ExecuteScalar();
                    return currentRole;
                }
            }

            catch (SqlException)
            {
                return "There has been an unexpected error while trying to connect to the DataBase";
            }
        }

        private string RetrieveCurrentUserStatusFromDatabase()
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    dbcon.Open();
                    SqlCommand RetrieveCurrentUserStatus = new SqlCommand("EXECUTE SelectCurrentUserStatusFromDatabase", dbcon);
                    string currentUserStatus = (string)RetrieveCurrentUserStatus.ExecuteScalar();
                    return currentUserStatus;
                }
            }
            catch (SqlException)
            {
                return "There has been an unexpected error while trying to connect to the DataBase";
            }
        }

        public bool CheckUsernameAvailabilityInDatabase(string usernameCheck)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    dbcon.Open();
                    SqlCommand checkUsername = new SqlCommand("CheckUniqueUsername", dbcon);
                    checkUsername.CommandType = CommandType.StoredProcedure;
                    checkUsername.Parameters.AddWithValue("@usernameCheck", usernameCheck);
                    int UserCount = (int)checkUsername.ExecuteScalar();
                    if (UserCount != 0)
                    {
                        return false;
                    }
                }
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
            return true;
        }

        public void InsertNewUserIntoDatabase(string pendingUsername, string pendingPassphrase, string pendingRole)
        {
            try
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
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void RemoveUsernameFromDatabase(string username)
        {
            try
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
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public Dictionary<string, string> ShowAvailableUsersFromDatabase()
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

        public int CountOpenTicketsAssignedToUser(string currentUsername)
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

        public void SelectOpenTicketsAssignedToUser(string currentUsername)
        {
            try
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
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }            
        }

        public void SelectSingleUserRole(string username, string currentUsername, string userRole)
        {
            try
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
                        print.QuasarScreen(currentUsername);
                        Console.WriteLine();
                        Console.WriteLine($"User '{username}' already is {userRole}. Please proceed to choose a different Role Status\n\n(Press any key to continue)");
                        Console.ReadKey();
                        print.QuasarScreen(currentUsername);
                        Console.WriteLine();
                        userRole = print.SelectUserRole();
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
                    print.QuasarScreen(currentUsername);
                    print.UniversalLoadingOutput("Modifying User's role status in progress");
                    Console.WriteLine($"User {username} has been successfully modified as {newUserRole}\n\n(Press any key to continue)");
                    Console.ReadKey();
                }
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }            
        }

        public void OpenNewTechnicalTicket(string currentUsername, string userAssignedTo, string comment)
        {
            try
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
                    print.QuasarScreen(currentUsername);
                    print.UniversalLoadingOutput("Filing new customer ticket in progress");
                    Console.WriteLine($"New Customer Ticket with ID: {ticketID} has been successfully created and assigned to {userAssignedTo}. Status: Open");
                }
            }
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void SetTicketStatusToClosed(string currentUsername, int ticketID)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    dbcon.Open();
                    SqlCommand closeCustomerTicket = new SqlCommand($"EXECUTE SetTicketStatusToClosed {ticketID}", dbcon);
                    closeCustomerTicket.ExecuteScalar();
                    print.QuasarScreen(currentUsername);
                    print.UniversalLoadingOutput("Action in progress");
                    Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully marked as closed.\n\n(Press any key to continue)");
                    Console.ReadKey();
                }
            }
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void EditCommentOfOpenTicket(int ID, string ticketComment)
        {
            try
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
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void SelectSingleCustomerTicket(int ticketID)
        {
            try
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
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void ViewListOfOpenCustomerTickets()
        {
            try
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
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void ViewListOfAllCustomerTickets()
        {
            try
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
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public void DeleteCustomerTicket(string currentUsername, int ticketID)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    dbcon.Open();
                    SqlCommand deleteCustomerTicket = new SqlCommand("DeleteCustomerTicket", dbcon);
                    deleteCustomerTicket.CommandType = CommandType.StoredProcedure;
                    deleteCustomerTicket.Parameters.AddWithValue("@ticketID", ticketID);
                    deleteCustomerTicket.ExecuteScalar();
                    print.QuasarScreen(currentUsername);
                    print.UniversalLoadingOutput("Action in progress");
                    Console.WriteLine($"Customer ticket with ID = {ticketID} has been successfully deleted\n\n(Press any key to continue)");
                    Console.ReadKey();
                }
            }
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public bool CheckIfTicketIDWithStatusOpenExistsInList(int ID)
        {
            try
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
            }
            catch(SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }            
            return true;
        }

        public void ChangeUserAssignedTo(string nextOwner, int ID)
        {
            try
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
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public string SelectUserAssignedToTicket(int ticketID)
        {
            try
            {
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    dbcon.Open();
                    SqlCommand SelectUserAssignedToTicketInDatabase = new SqlCommand("SelectUserAssignedToTicket", dbcon);
                    SelectUserAssignedToTicketInDatabase.CommandType = CommandType.StoredProcedure;
                    SelectUserAssignedToTicketInDatabase.Parameters.AddWithValue("@ID", ticketID);
                    string user = (string)SelectUserAssignedToTicketInDatabase.ExecuteScalar();
                    return user;
                }
            }
            catch (SqlException)
            {
                return "There has been an unexpected error while trying to connect to the DataBase";
            }            
        }



        public bool CheckIfTicketIDWithStatusOpenOrClosedExistsInList(int ID)
        {
            try
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
            }
            catch (SqlException exc)
            {
                Console.WriteLine(exc.Message);
            }
            return true;
        }

        public void TerminateQuasar()
        {
            var print = new OutputControl();
            string yes = "Yes";
            string no = "No";
            string currentUsername = "Not Registered";
            string exitMessage = "\r\nWould you like to exit Quasar?\r\n";
            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, exitMessage).option;

            if (yesOrNoSelection == yes)
            {
                SetCurrentUserStatusToInactive(currentUsername);
                print.UniversalLoadingOutput("Wait for Quasar to shut down");
                print.SpecialThanksMessage();
                Environment.Exit(0);
            }
            else if (yesOrNoSelection == no)
            {
                ApplicationMenu.LoginScreen();
            }
        }

        public void LoggingOffQuasar()
        {
            string yes = "Yes";
            string no = "No";
            string logOffMessage = "Would you like to log out?\r\n";
            string currentUsername = RetrieveCurrentUserFromDatabase();
            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, logOffMessage).option;

            if (yesOrNoSelection == yes)
            {
                print.QuasarScreen("Not Registered");
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
