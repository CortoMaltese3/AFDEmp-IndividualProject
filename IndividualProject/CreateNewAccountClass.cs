using System;
using System.Data.SqlClient;
using System.IO;

namespace IndividualProject
{
    class CreateNewAccountClass
    {
        

        public static void CreateNewAccountRequest()
        {
            var path = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\NewUserRequest.txt";
            //try
            //{
            //    File.Exists(path);
            //}
            //catch (DirectoryNotFoundException e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            try
            {
                Console.WriteLine("\r\nChoose your username and password. Both must be limited to 20 characters");
                string username = ConnectToServerClass.UsernameInput();
                string passphrase = ConnectToServerClass.PassphraseInput();
                while (CheckUsernameAvailability(username) != 0)
                {
                    Console.WriteLine("This username is already in use. Choose a different one");
                    username = ConnectToServerClass.UsernameInput();
                }

                //FileStream NewUserRequest = File.OpenWrite(path);
                //creates a file and writes collection of string (array) and closes the File
                //File.WriteAllLines(path, new string[] {$"username: {username}", $"passphrase: {passphrase}" });
                //NewUserRequest.Write(new string[] { $"username: {username}", $"passphrase: {passphrase}" });
                //NewUserRequest.Close();
                //StreamWriter NewUserRequest = new StreamWriter(path, true);

                //check why catch fails, it creates a new file if it doesnt exist

                using (StreamWriter NewUserRequest = File.AppendText(path))
                {
                    NewUserRequest.WriteLine($"username: {username}");
                    NewUserRequest.WriteLine($"password: {passphrase}");
                    NewUserRequest.WriteLine();
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Your Account Request is Pending. Please wait for the administrator to grant you access.");
        }

        public static int CheckUsernameAvailability(string usernameCheck)
        {
            //TODO : Check if this is a vulnerability
            var connectionString = $"Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
            using (SqlConnection dbcon = new SqlConnection(connectionString))
            {
                dbcon.Open();

                SqlCommand checkUsername = new SqlCommand($"SELECT COUNT(*) FROM LoginCredentials WHERE (username = '{usernameCheck}')", dbcon);
                int UserCount = (int)checkUsername.ExecuteScalar();
                return UserCount;
            }
        }
    }
}
