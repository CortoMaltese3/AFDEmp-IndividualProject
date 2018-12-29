using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IndividualProject
{
    static class ConnectToServer
    {
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";

        public static void UserLoginCredentials()
        {
            InputOutputAnimationControl.QuasarScreen("Not Registered");
            string username = InputOutputAnimationControl.UsernameInput();
            string passphrase = InputOutputAnimationControl.PassphraseInput();
            var dbcon = new SqlConnection(connectionString);

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
                    System.Threading.Thread.Sleep(1000);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
                else
                {
                    while (true)
                    {
                        InputOutputAnimationControl.QuasarScreen("Not Registered");
                        Console.Write($"Invalid Username or Passphrase. Try again.");
                        username = InputOutputAnimationControl.UsernameInput();
                        passphrase = InputOutputAnimationControl.PassphraseInput();
                        InputOutputAnimationControl.QuasarScreen("Not Registered");
                        InputOutputAnimationControl.UniversalLoadingOuput("Attempting connection to server");
                        if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                        {
                            string currentUsernameRole = RetrieveCurrentUsernameRoleFromDatabase();
                            InputOutputAnimationControl.QuasarScreen(username);
                            SetCurrentUserStatusToActive(username);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine($"Connection Established! Welcome back {username}!");
                            Console.ResetColor();
                            System.Threading.Thread.Sleep(1000);
                            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                        }
                    }
                }
            }
            return;
        }

        public static bool TestConnectionToSqlServer(this SqlConnection connectionString)
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

        public static bool CheckUsernameAndPasswordMatchInDatabase(string usernameCheck, string passphraseCheck)
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
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

        public static void SetCurrentUserStatusToActive(string currentUsername)
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand SetStatusToActive = new SqlCommand($"EXECUTE SetCurrentUserStatusToActive '{currentUsername}'", dbcon);
                SetStatusToActive.ExecuteScalar();
            }
        }

        public static void SetCurrentUserStatusToInactive(string currentUsername)
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand SetStatusToInactive = new SqlCommand("EXECUTE SetCurrentUserStatusToInactive", dbcon);
                SetStatusToInactive.ExecuteScalar();
            }
        }

        public static string RetrieveCurrentUserFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveLoginCredentials = new SqlCommand($"EXECUTE SelectCurrentUserFromDatabase", dbcon);
                string currentUsername = (string)RetrieveLoginCredentials.ExecuteScalar();
                return currentUsername;
            }
        }

        public static string RetrieveCurrentUsernameRoleFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveCurrentUsernameRole = new SqlCommand("EXECUTE SelectCurrentUserRoleFromDatabase", dbcon);
                string currentRole = (string)RetrieveCurrentUsernameRole.ExecuteScalar();
                return currentRole;
            }
        }

        public static string RetrieveCurrentUserStatusFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveCurrentUserStatus = new SqlCommand("EXECUTE SelectCurrentUserStatusFromDatabase", dbcon);
                string currentUserStatus = (string)RetrieveCurrentUserStatus.ExecuteScalar();
                return currentUserStatus;
            }
        }

        public static void TerminateQuasar()
        {
            string yes = "Yes", no = "No", currentUsername = RetrieveCurrentUserFromDatabase();
            InputOutputAnimationControl.QuasarScreen(currentUsername);            
            string exitMessage = "Would you like to exit Quasar ?";
            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, exitMessage).NameOfChoice;

            if (yesOrNoSelection == yes)
            {
                InputOutputAnimationControl.QuasarScreen("Not Registered");
                ConnectToServer.SetCurrentUserStatusToInactive(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Wait for Quasar to shut down");

                Console.ForegroundColor = ConsoleColor.Cyan;
                for (int blink = 0; blink < 6; blink++)
                {
                    if (blink % 2 == 0)
                    {
                        InputOutputAnimationControl.WriteBottomLine("~~~~~Special thanks to Afro~~~~~");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        System.Threading.Thread.Sleep(300);
                    }
                    else
                    {
                        InputOutputAnimationControl.WriteBottomLine("~~~~~Special thanks to Afro~~~~~");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        System.Threading.Thread.Sleep(300);
                    }
                }
                Environment.Exit(0);
            }

            else if (yesOrNoSelection == no)
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                ApplicationMenuClass.LoginScreen();
            }
        }

        public static void LoggingOffQuasar()
        {
            string yes = "Yes", no = "No", currentUsername = RetrieveCurrentUserFromDatabase();
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            string logOffMessage = "Would you like to log out? ";
            string yesOrNoSelection = SelectMenu.MenuColumn(new List<string> { yes, no }, currentUsername, logOffMessage).NameOfChoice;

            if (yesOrNoSelection == yes)
            {
                InputOutputAnimationControl.QuasarScreen("Not Registered");
                SetCurrentUserStatusToInactive(currentUsername);
                ApplicationMenuClass.LoginScreen();

            }

            else if (yesOrNoSelection == no)
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsername);
            }
        }
    }
}
