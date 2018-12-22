using System;
using System.Data.SqlClient;

namespace IndividualProject
{
    static class ConnectToServerClass
    {
        static readonly string connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";

        public static void UserLoginCredentials()
        {
            string username = InputOutputAnimationControlClass.UsernameInput();
            string passphrase = InputOutputAnimationControlClass.PassphraseInput();
            string currentUsername = RetrieveCurrentLoginCredentialsFromDatabase();
            var dbcon = new SqlConnection(connectionString);

            while (TestConnectionToSqlServer(dbcon))
            {
                if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                {
                    InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                    StoreCurrentLoginCredentialsToDatabase(username, passphrase);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"Connection Established! Welcome back {username}!");
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(1000);
                    return;
                }
                else
                {
                    while (true)
                    {
                        InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                        Console.WriteLine();
                        Console.Write($"Invalid Username or Passphrase. Try again.");
                        username = InputOutputAnimationControlClass.UsernameInput();
                        passphrase = InputOutputAnimationControlClass.PassphraseInput();
                        InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                        InputOutputAnimationControlClass.UniversalLoadingOuput("Attempting connection to server");
                        if (CheckUsernameAndPasswordMatchInDatabase(username, passphrase))
                        {
                            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                            StoreCurrentLoginCredentialsToDatabase(username, passphrase);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine($"Connection Established! Welcome back {username}!");
                            Console.ResetColor();
                            System.Threading.Thread.Sleep(1000);
                            return;
                        }
                    }
                }
            }
            return;
        }

        public static bool TestConnectionToSqlServer(this SqlConnection connectionString)
        {
            string currentUsername = RetrieveCurrentLoginCredentialsFromDatabase();
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            InputOutputAnimationControlClass.UniversalLoadingOuput("Attempting connection to server");
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
                SqlCommand checkUsername = new SqlCommand($"SELECT COUNT(*) FROM LoginCredentials " +
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

        public static void StoreCurrentLoginCredentialsToDatabase(string currentUsername, string currentPassphrase)
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand StoreLoginCredentials = new SqlCommand($"UPDATE CurrentLoginCredentials SET username = '{currentUsername}', passphrase = '{currentPassphrase}', currentStatus = 'active'", dbcon);
                StoreLoginCredentials.ExecuteScalar();
            }
        }

        public static string RetrieveCurrentLoginCredentialsFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveLoginCredentials = new SqlCommand($"SELECT username FROM CurrentLoginCredentials", dbcon);
                string currentUsername = (string)RetrieveLoginCredentials.ExecuteScalar();
                return currentUsername;
            }
        }

        public static string RetrieveCurrentUsernameRoleFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveCurrentUsernameRole = new SqlCommand($"SELECT userRole FROM UserLevelAccess u INNER JOIN CurrentLoginCredentials c ON c.username = u.username", dbcon);
                string currentRole = (string)RetrieveCurrentUsernameRole.ExecuteScalar();
                return currentRole;
            }
        }

        public static string RetrieveCurrentUserStatusFromDatabase()
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand RetrieveCurrentUserStatus = new SqlCommand($"SELECT currentStatus FROM CurrentLoginCredentials", dbcon);
                string currentUserStatus = (string)RetrieveCurrentUserStatus.ExecuteScalar();
                return currentUserStatus;
            }
        }

        public static void ChangeCurrentUserStatusToInactive()
        {
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();
                SqlCommand SetStatusToInnactive = new SqlCommand($"UPDATE CurrentLoginCredentials SET username = 'Not Registered', currentStatus = 'inactive'", dbcon);
                SetStatusToInnactive.ExecuteScalar();
            }
        }

        public static void TerminateQuasar()
        {
            string currentUsername = RetrieveCurrentLoginCredentialsFromDatabase();
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            Console.WriteLine("\r\nWould you like to exit Quasar? ");
            string option = InputOutputAnimationControlClass.PromptYesOrNo();
            if (option == "y" || option == "Y")
            {
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                ChangeCurrentUserStatusToInactive();
                InputOutputAnimationControlClass.UniversalLoadingOuput("Wait for Quasar to shut down");

                Console.ForegroundColor = ConsoleColor.Cyan;
                for (int blink = 0; blink < 8; blink++)
                {
                    if (blink % 2 == 0)
                    {
                        InputOutputAnimationControlClass.WriteBottomLine("~~~~~Dedicated to Afro~~~~~");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        System.Threading.Thread.Sleep(500);
                    }
                    else
                    {
                        InputOutputAnimationControlClass.WriteBottomLine("~~~~~Dedicated to Afro~~~~~");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        System.Threading.Thread.Sleep(500);
                    }
                }
                Environment.Exit(0);
            }
            else
            {
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                ApplicationMenuClass.LoginScreen();
            }
        }

        public static void LoggingOffQuasar()
        {
            string currentUsername = RetrieveCurrentLoginCredentialsFromDatabase();
            InputOutputAnimationControlClass.QuasarScreen(currentUsername);
            Console.WriteLine("\r\nWould you like to log out? ");
            string option = InputOutputAnimationControlClass.PromptYesOrNo();
            if (option == "y" || option == "Y")
            {
                ChangeCurrentUserStatusToInactive();
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                InputOutputAnimationControlClass.UniversalLoadingOuput("Logging out");
                ApplicationMenuClass.LoginScreen();
            }
            else
            {
                InputOutputAnimationControlClass.QuasarScreen(currentUsername);
                ActiveUserFunctionsClass.ActiveUserProcedures();
            }
        }
    }
}
