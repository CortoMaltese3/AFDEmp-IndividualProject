using System.Collections.Generic;

namespace IndividualProject
{
    class ActiveUserFunctions
    {
        public static void UserFunctionMenuScreen(string currentUsernameRole)
        {
            string currentUser = ConnectToServer.RetrieveCurrentUserFromDatabase();
            int countTickets = ConnectToServer.CountOpenTicketsAssignedToUser(currentUser);
            
            string notificationsAdmin = $"Check user notifications";
            string notificationsUser = $"Check user notifications [{countTickets}]";
            string requests = "Create new username/password from requests";
            string viewUsers = "Show list of active users";
            string modifyRole = "Upgrade/Downgrade user's role";
            string deleteUser = "Delete an active username from Database";
            string manageTickets = "Manage Customer Trouble Tickets";
            string viewTickets = "View Trouble Tickets";
            string editTicket = "Edit Trouble Tickets";
            string deleteTicket = "Delete Trouble Tickets";
            string logOut = "\nLog Out";
            string message = "Choose one of the following functions\n";


            switch (currentUsernameRole)
            {
                #region Super Admin Functions
                case "super_admin":
                    while (true)
                    {
                        string SuperAdminFunctionMenu = SelectMenu.MenuColumn(new List<string> { notificationsAdmin, requests, viewUsers, modifyRole, deleteUser, manageTickets, viewTickets, editTicket, deleteTicket, logOut }, currentUser, message).option;

                        if (SuperAdminFunctionMenu == notificationsAdmin)
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
                            ManageTroubleTickets.OpenOrCloseTroubleTicket();
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
                            DeleteTroubleTickets.DeleteExistingOpenOrClosedTicketFunction();
                        }

                        else if (SuperAdminFunctionMenu == logOut)
                        {
                            ConnectToServer.LoggingOffQuasar();
                        }
                    }
                #endregion

                #region Administrator Functions
                case "Administrator":
                    while (true)
                    {
                        string AdminFunctionMenu = SelectMenu.MenuColumn(new List<string> { notificationsUser, manageTickets, viewTickets, editTicket, deleteTicket, logOut }, currentUser, message).option;

                        if (AdminFunctionMenu == notificationsUser)
                        {
                            RoleFunctions.CheckUserNotifications();
                        }

                        else if (AdminFunctionMenu == manageTickets)
                        {
                            ManageTroubleTickets.OpenOrCloseTroubleTicket();
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
                            DeleteTroubleTickets.DeleteExistingOpenOrClosedTicketFunction();
                        }

                        else if (AdminFunctionMenu == logOut)
                        {
                            ConnectToServer.LoggingOffQuasar();
                        }
                    }
                #endregion

                #region Moderator Functions
                case "Moderator":
                    while (true)
                    {
                        string ModeratorFunctionMenu = SelectMenu.MenuColumn(new List<string> { notificationsUser, manageTickets, viewTickets, editTicket, logOut }, currentUser, message).option;

                        if (ModeratorFunctionMenu == notificationsUser)
                        {
                            RoleFunctions.CheckUserNotifications();
                        }

                        else if (ModeratorFunctionMenu == manageTickets)
                        {
                            ManageTroubleTickets.OpenOrCloseTroubleTicket();
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
                #endregion

                #region User Functions
                case "User":
                    while (true)
                    {
                        string UserFunctionMenu = SelectMenu.MenuColumn(new List<string> { notificationsUser, manageTickets, viewTickets, logOut }, currentUser, message).option;

                        if (UserFunctionMenu == notificationsUser)
                        {
                            RoleFunctions.CheckUserNotifications();
                        }

                        else if (UserFunctionMenu == manageTickets)
                        {
                            ManageTroubleTickets.OpenOrCloseTroubleTicket();
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
                    #endregion
            }
        }
    }
}
