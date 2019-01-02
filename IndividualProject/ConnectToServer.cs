using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
                    string currentUsernameRole = RetrieveCurrentUsernameRoleFromDatabase();
                    InputOutputAnimationControl.QuasarScreen(username);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"Connection Established! Welcome back {username}!");
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(1500);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
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
                SqlCommand checkUsername = new SqlCommand($"EXECUTE CheckUniqueCredentials '{usernameCheck}', '{passphraseCheck}'", dbcon);
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
                SqlCommand SetStatusToActive = new SqlCommand($"EXECUTE SetCurrentUserStatusToActive '{currentUsername}'", dbcon);
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
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsername);
            }
        }
    }
}
