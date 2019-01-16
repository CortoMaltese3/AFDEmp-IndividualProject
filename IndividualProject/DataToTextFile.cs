﻿using System;
using System.IO;
using System.Linq;

namespace IndividualProject
{
    //The DataToTextFile class deals with data stored/manipulated to/from text files. 
    //It manages the new user registrations and active users notifications log

    class DataToTextFile
    {
        //The NotificationsLog is Log History of the user's actions plus other user actions that involves the previous user.
        public static void ViewUserNotificationsLog(string currentUser)
        {
            try
            {
                string[] lines = File.ReadAllLines(Globals.TTnotificationToUser + currentUser + ".txt");
                int index = 1;
                Console.WriteLine("NOTIFICATIONS LOG");
                foreach (string line in lines.Skip(1))
                {
                    Console.WriteLine(index + ". " + line + "\n");
                    index++;
                }
            }
            catch (IOException exc)
            {
                Console.WriteLine($"There has been an unexpected error while trying to access the file. " +
                                  $"Make sure that you have access to the specific folder.\n{exc.Message}");
            }

        }

        public static void DeleteUserNotificationsLog(string userToBeDeleted)
        {
            try
            {
                File.Delete(Globals.TTnotificationToUser + userToBeDeleted + ".txt");
                Console.WriteLine($"{userToBeDeleted}'s notifications log has been successfully deleted");
            }
            catch (IOException exc)
            {
                Console.WriteLine($"There has been an unexpected error while trying to access the file. " +
                  $"Make sure that you have access to the specific folder and the File exists.\n{exc.Message}");
            }

        }

        public static void CloseTicketToUserNotification(string userClosingTheTicket, string previousTicketOwner, int ticketID)
        {
            try
            {
                DateTime dateTimeAdded = DateTime.Now;
                using (StreamWriter sw = File.AppendText(Globals.TTnotificationToUser + previousTicketOwner + ".txt"))
                {
                    sw.WriteLine($"[{dateTimeAdded}] - User {userClosingTheTicket} has marked the TT with [ID = {ticketID}] that was assigned to you as closed. Visit the View TT Section for more details");
                }
            }
            catch (IOException exc)
            {
                Console.WriteLine($"There has been an unexpected error while trying to access the file. " +
                                  $"Make sure that you have access to the specific folder and the File exists.\n{exc.Message}");
            }
        }

        public static void AssignTicketToUserNotification(string currentUserAssigning, string UserAssigningTicketTo)
        {
            try
            {
                DateTime dateTimeAdded = DateTime.Now;
                using (StreamWriter sw = File.AppendText(Globals.TTnotificationToUser + UserAssigningTicketTo + ".txt"))
                {
                    sw.WriteLine($"[{dateTimeAdded}] - User {currentUserAssigning} has assigned a new TT to you. Visit the View TT Section for more details");
                }
            }
            catch (IOException exc)
            {
                Console.WriteLine($"There has been an unexpected error while trying to access the file. " +
                                  $"Make sure that you have access to the specific folder and the File exists.\n{exc.Message}");
            }
        }

        //Gets the passphrase requested from the new user registrations list
        public static string GetPendingPassphrase()
        {
            try
            {
                string pendingPassphrase = File.ReadLines(Globals.newUserRequestPath).Skip(1).Take(1).First();
                return pendingPassphrase;
            }
            catch(IOException exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
         
        }

        //Gets the username requested from the new user registrations list
        public static string GetPendingUsername()
        {
            try
            {
                if (!Directory.Exists(Globals.newUserRequestFolderPath))
                {
                    CreateDirectoryAndFile(Globals.newUserRequestFolderPath, Globals.newUserRequestPath);
                }
                string pendingUsername = File.ReadLines(Globals.newUserRequestPath).First();
                return pendingUsername;
            }
            catch(IOException exc)
            {
                Console.WriteLine(exc.Message);
                return null;
            }
        }

        //Creates a folder path in which a text file will be created
        public static void CreateDirectoryAndFile(string folderPath, string filePath)
        {
            Directory.CreateDirectory(folderPath);
            if (!File.Exists(Globals.newUserRequestPath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(" ");
                }
            }
        }

        public static void NewUsernameRequestToList(string usernameAdd, string passphraseAdd)
        {
            try
            {
                File.WriteAllLines(Globals.newUserRequestPath, new string[] { $"username: {usernameAdd}", $"passphrase: {passphraseAdd}" });
            }
            catch(IOException exc)
            {
                Console.WriteLine($"There has been an unexpected error while trying to access the file. " +
                                  $"Make sure that you have access to the specific folder and the File exists.\n{exc.Message}");
            }
        }

        //Clears the new user registrations List to be used from the next user registering
        public static void ClearNewUserRegistrationList()
        {
            try
            {
                File.WriteAllLines(Globals.newUserRequestPath, new string[] { " " });
            }
            catch (IOException exc)
            {
                Console.WriteLine($"There has been an unexpected error while trying to access the file. " +
                                  $"Make sure that you have access to the specific folder and the File exists.\n{exc.Message}");
            }
        }

        //Creates a text file for the new user to be used as a notifications log
        public static void CreateNewUserLogFile(string pendingUsername)
        {
            try
            {
                if (!Directory.Exists(Globals.TTnotificationToUserFolder))
                {
                    CreateDirectoryAndFile(Globals.TTnotificationToUserFolder, Globals.TTnotificationToUser + pendingUsername + ".txt");
                }
                File.WriteAllLines(Globals.TTnotificationToUser + pendingUsername + ".txt", new string[] { "NOTIFICATIONS LOG" });
            }
            catch (IOException dir)
            {
                Console.WriteLine($"There has been an unexpected error while trying to create the file. " +
                                  $"Make sure that you have access to the specific folder.\n{dir.Message}");
            }
        }

        //Deletes a user's Notification Log if the admin removes him from the Database
        public static void DeleteTicketToUserNotification(string userDeletingTheTicket, string previousTicketOwner, int ticketID)
        {
            try
            {
                DateTime dateTimeAdded = DateTime.Now;
                using (StreamWriter sw = File.AppendText(Globals.TTnotificationToUser + previousTicketOwner + ".txt"))
                {
                    sw.WriteLine($"[{dateTimeAdded}] - User {userDeletingTheTicket} has deleted the TT with [ID = {ticketID}] assigned to you. In case of emergency, contact the super_admin");
                }
            }
            catch (IOException exc)
            {
                Console.WriteLine($"There has been an unexpected error while trying to access the file. " +
                  $"Make sure that you have access to the specific folder and the File exists.\n{exc.Message}");
            }
        }
    }
}