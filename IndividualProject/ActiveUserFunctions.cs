using System;
using System.Collections.Generic;

namespace IndividualProject
{
    class ActiveUserFunctions
    {
        public static void UserFunctionMenuScreen(string currentUsernameRole)
        {
            string currentUser = ConnectToServer.RetrieveCurrentUserFromDatabase();
            string notifications = "Check user notifications", requests = "Create new username/password from requests", viewUsers = "Show list of active users", modifyRole = "Upgrade/Downgrade user's role",
                   deleteUser = "Delete an active username from Database", manageTickets = "Manage Customer Trouble Tickets", viewTickets = "View Trouble Tickets",
                   editTicket = "Edit Trouble Tickets", deleteTicket = "Delete Trouble Tickets", logOut = "\nLog Out";
            string message = "Choose one of the following functions\n";

            switch (currentUsernameRole)
            {
                case "super_admin":
                    while (true)
                    {
                        string SuperAdminFunctionMenu = SelectMenu.MenuColumn(new List<string> { notifications, requests, viewUsers, modifyRole, deleteUser, manageTickets, viewTickets, editTicket, deleteTicket, logOut }, currentUser, message).NameOfChoice;

                        if (SuperAdminFunctionMenu == notifications)
                        {
                            RoleFunctions.CheckAdminNotifications();
                        }

                        else if (SuperAdminFunctionMenu == requests)
                        {
                            RoleFunctions.CreateNewUserFromRequestFunction();
                        }

                        else if (SuperAdminFunctionMenu == viewUsers)
                        {
                            RoleFunctions.ShowAvailableUsersFunction();
                        }

                        else if (SuperAdminFunctionMenu == modifyRole)
                        {
                            RoleFunctions.AlterUserRoleStatus();
                        }

                        else if (SuperAdminFunctionMenu == deleteUser)
                        {
                            RoleFunctions.DeleteUserFromDatabase();
                        }

                        else if (SuperAdminFunctionMenu == manageTickets)
                        {
                            TransactedData.ManageCustomerTickets();
                        }

                        else if (SuperAdminFunctionMenu == viewTickets)
                        {
                            TransactedData.ViewExistingOpenTicketsFunction();
                        }

                        else if (SuperAdminFunctionMenu == editTicket)
                        {
                            TransactedData.EditExistingOpenTicketFunction();
                        }

                        else if (SuperAdminFunctionMenu == deleteTicket)
                        {
                            TransactedData.DeleteExistingOpenOrClosedTicketFunction();
                        }

                        else if (SuperAdminFunctionMenu == logOut)
                        {
                            ConnectToServer.LoggingOffQuasar();
                        }
                    }

                case "Administrator":
                    while (true)
                    {
                        string AdminFunctionMenu = SelectMenu.MenuColumn(new List<string> { notifications, manageTickets, viewTickets, editTicket, deleteTicket, logOut }, currentUser, message).NameOfChoice;

                        if (AdminFunctionMenu == notifications)
                        {
                            RoleFunctions.CheckUserNotifications();
                        }

                        else if (AdminFunctionMenu == manageTickets)
                        {
                            TransactedData.ManageCustomerTickets();
                        }

                        else if (AdminFunctionMenu == viewTickets)
                        {
                            TransactedData.ViewExistingOpenTicketsFunction();
                        }

                        else if (AdminFunctionMenu == editTicket)
                        {
                            TransactedData.EditExistingOpenTicketFunction();
                        }

                        else if (AdminFunctionMenu == deleteTicket)
                        {
                            TransactedData.DeleteExistingOpenOrClosedTicketFunction();
                        }

                        else if (AdminFunctionMenu == logOut)
                        {
                            ConnectToServer.LoggingOffQuasar();
                        }
                    }

                case "Moderator":
                    while (true)
                    {
                        string ModeratorFunctionMenu = SelectMenu.MenuColumn(new List<string> { notifications, manageTickets, viewTickets, editTicket, logOut }, currentUser, message).NameOfChoice;

                        if (ModeratorFunctionMenu == notifications)
                        {
                            RoleFunctions.CheckUserNotifications();
                        }

                        else if (ModeratorFunctionMenu == manageTickets)
                        {
                            TransactedData.ManageCustomerTickets();
                        }

                        else if (ModeratorFunctionMenu == viewTickets)
                        {
                            TransactedData.ViewExistingOpenTicketsFunction();
                        }

                        else if (ModeratorFunctionMenu == editTicket)
                        {
                            TransactedData.EditExistingOpenTicketFunction();
                        }

                        else if (ModeratorFunctionMenu == logOut)
                        {
                            ConnectToServer.LoggingOffQuasar();
                        }
                    }

                case "User":
                    while (true)
                    {
                        string UserFunctionMenu = SelectMenu.MenuColumn(new List<string> { notifications, manageTickets, viewTickets, logOut }, currentUser, message).NameOfChoice;

                        if (UserFunctionMenu == notifications)
                        {
                            RoleFunctions.CheckUserNotifications();
                        }

                        else if (UserFunctionMenu == manageTickets)
                        {
                            TransactedData.ManageCustomerTickets();
                        }

                        else if (UserFunctionMenu == viewTickets)
                        {
                            TransactedData.ViewExistingOpenTicketsFunction();
                        }

                        else if (UserFunctionMenu == logOut)
                        {
                            ConnectToServer.LoggingOffQuasar();
                        }
                    }
            }
        }
    }
}
