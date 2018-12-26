using System;
using System.Data.SqlClient;

namespace IndividualProject
{
    static class ConnectToServer
    {
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";

        public static void UserLoginCredentials()
        {
            string username = InputOutputAnimationControl.UsernameInput();
            string passphrase = InputOutputAnimationControl.PassphraseInput();
            string currentUsername = RetrieveCurrentLoginCredentialsFromDatabase();
            var dbcon = new SqlConnection(connectionString);

            while (TestConnectionToSqlServer(dbcon))
            {
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
                else
                {
                    while (true)
                    {
                        string currentUsernameRole = RetrieveCurrentUsernameRoleFromDatabase();
                        InputOutputAnimationControl.QuasarScreen(username);
                        Console.WriteLine();
                        Console.Write($"Invalid Username or Passphrase. Try again.");
                        username = InputOutputAnimationControl.UsernameInput();
                        passphrase = InputOutputAnimationControl.PassphraseInput();
                        InputOutputAnimationControl.QuasarScreen(username);
                        InputOutputAnimationControl.UniversalLoadingOuput("Attempting connection to server");
                        if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                        {
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
                SqlCommand checkUsername = new SqlCommand($"SELECT COUNT(*) FROM UserCredentials " +
                    $"                                      WHERE (username = '{usernameCheck}' " +
                    $"                                      AND passphrase = '{passphraseCheck}')", dbcon);
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
                SqlCommand SetStatusToActive = new SqlCommand("EXECUTE SetCurrentUserStatusToActive", dbcon);
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

        public static string RetrieveCurrentLoginCredentialsFromDatabase()
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
            string currentUsername = RetrieveCurrentLoginCredentialsFromDatabase();
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            Console.WriteLine("\r\nWould you like to exit Quasar? ");
            string option = InputOutputAnimationControl.PromptYesOrNo();
            if (option == "y" || option == "Y")
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                SetCurrentUserStatusToInactive(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Wait for Quasar to shut down");

                Console.ForegroundColor = ConsoleColor.Cyan;
                for (int blink = 0; blink < 6; blink++)
                {
                    if (blink % 2 == 0)
                    {
                        InputOutputAnimationControl.WriteBottomLine("~~~~~Dedicated to Afro~~~~~");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        System.Threading.Thread.Sleep(300);
                    }
                    else
                    {
                        InputOutputAnimationControl.WriteBottomLine("~~~~~Dedicated to Afro~~~~~");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        System.Threading.Thread.Sleep(300);
                    }
                }
                Environment.Exit(0);
            }
            else
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                ApplicationMenuClass.LoginScreen();
            }
        }

        public static void LoggingOffQuasar()
        {
            string currentUsernameRole = RetrieveCurrentUsernameRoleFromDatabase();
            string currentUsername = RetrieveCurrentLoginCredentialsFromDatabase();
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            Console.WriteLine("\r\nWould you like to log out? ");
            string option = InputOutputAnimationControl.PromptYesOrNo();
            if (option == "y" || option == "Y")
            {
                SetCurrentUserStatusToInactive(currentUsername);
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Logging out");
                ApplicationMenuClass.LoginScreen();
            }
            else
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
            }
        }
    }
}
