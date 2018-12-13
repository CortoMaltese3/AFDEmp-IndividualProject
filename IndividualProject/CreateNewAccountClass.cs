using System;
using System.IO;

namespace IndividualProject
{
    class CreateNewAccountClass
    {
        public static void CreateNewAccount()
        {
            var path = @"C:\Users\giorg\Documents\Coding\AFDEmp\C#\Individual Project 1\NewUserRequest.txt";
            try
            {
                Console.WriteLine("\r\nChoose your username and password. Both must be limited to 20characters");
                string username = ConnectToServerClass.UsernameInput();
                string passphrase = ConnectToServerClass.PassphraseInput();
                //FileStream NewUserRequest = File.OpenWrite(path);
                //creates a file and writes collection of string (array) and closes the File
                //File.WriteAllLines(path, new string[] {$"username: {username}", $"passphrase: {passphrase}" });
                //NewUserRequest.Write(new string[] { $"username: {username}", $"passphrase: {passphrase}" });
                //NewUserRequest.Close();
                StreamWriter NewUserRequest = new StreamWriter(path, true);
                using (NewUserRequest)
                {
                    NewUserRequest.WriteLine($"username: {username}");
                    NewUserRequest.WriteLine($"password: {passphrase}");
                    NewUserRequest.WriteLine();
                }
            }
            catch (DirectoryNotFoundException)
            {
  
            }
        }

}
}
