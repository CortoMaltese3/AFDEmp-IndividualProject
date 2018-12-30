using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IndividualProject
{
    class TransactedData
    {
        static readonly string currentUsername = ConnectToServer.RetrieveCurrentUserFromDatabase();        
        static readonly string currentUsernameRole = ConnectToServer.RetrieveCurrentUsernameRoleFromDatabase();

        public static void ManageCustomerTickets()
        {

            string open = "Open new Customer Ticket", close = "Close Customer Ticket", back = "\r\nBack",
                manageTicketmsg = "\r\nChoose one of the following options to continue:\r\n";
            while (true)
            {
                string openCloseTicket = SelectMenu.MenuColumn(new List<string> {open, close, back}, currentUsername, manageTicketmsg).option;

                if (openCloseTicket == open)
                {
                    OpenNewCustomerTicket();
                }

                else if (openCloseTicket == close)
                {
                    CloseCustomerTicket();
                }

                else if (openCloseTicket == back)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }               
        }

        public static void OpenCustomerTickets()
        {

            string open = "Open new Customer Ticket", back = "\r\nBack", manageTicketmsg = "\r\nChoose one of the following options to continue:\r\n";
            while (true)
            {
                string openTicket = SelectMenu.MenuRow(new List<string> { open, back }, currentUsername, manageTicketmsg).option;

                if (openTicket == open)
                {
                    OpenNewCustomerTicket();
                }

                else if (openTicket == back)
                {
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        public static string AssignTicketToUser()
        {
            
            string assignTicket = "Would you like to assign the ticket to another user?";
            string yes = "Yes", no = "No";

            string yesOrNoSelection = SelectMenu.MenuRow(new List<string> { yes, no, }, currentUsername, assignTicket).option;

            if (yesOrNoSelection == yes)
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Loading");

                Dictionary<string, string> AvailableUsernamesDictionary = RoleFunctions.ShowAvailableUsersFromDatabase();
                Console.Write("\r\nPlease select a user and proceed to assign: ");
                string usernameAssignment = InputOutputAnimationControl.UsernameInput();

                while (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false || usernameAssignment == "admin")
                {
                    if (AvailableUsernamesDictionary.ContainsKey(usernameAssignment) == false)
                    {
                        Console.WriteLine($"Database does not contain a User {usernameAssignment}");
                        System.Threading.Thread.Sleep(1000);
                        InputOutputAnimationControl.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = RoleFunctions.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputOutputAnimationControl.UsernameInput();
                    }
                    else
                    {
                        Console.WriteLine("Cannot assign ticket to super_admin! Please choose a different user");
                        System.Threading.Thread.Sleep(1000);
                        InputOutputAnimationControl.QuasarScreen(currentUsername);
                        AvailableUsernamesDictionary = RoleFunctions.ShowAvailableUsersFromDatabase();
                        Console.Write("\r\nPlease select a user and proceed to assign: ");
                        usernameAssignment = InputOutputAnimationControl.UsernameInput();
                    }
                }
                return usernameAssignment;
            }

            else if (yesOrNoSelection == no)
            {
                return currentUsername;
            }
            return currentUsername;
        }

        public static void OpenNewCustomerTicket()
        {
            string comment = InputOutputAnimationControl.TicketComment();
            string userAssignedTo = AssignTicketToUser();

            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand openNewTechnicalTicket = new SqlCommand($"EXECUTE OpenNewTechnicalTicket '{currentUsername}', '{userAssignedTo}', '{comment}'", dbcon);
                openNewTechnicalTicket.ExecuteNonQuery();
                SqlCommand fetchNewTicketID = new SqlCommand("EXECUTE fetchNewTicketID", dbcon);
                int ticketID = (int)fetchNewTicketID.ExecuteScalar();
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Filing new customer ticket in progress");
                Console.WriteLine($"New Customer Ticket with ID: {ticketID} has been successfully created and assigned to {userAssignedTo}. Status: Open");
            }
            Console.WriteLine("\r\nPress any key to return");
            Console.ReadKey();
            ManageCustomerTickets();
        }

        public static void CloseCustomerTicket()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("CLOSE EXISTING TECHNICAL TICKETS");
            
            string listOfTickets = "Would you like to open the list of Opened Tickets?";
            string yes = "Yes", no = "No";
            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, listOfTickets).option;
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    if (optionYesOrNo == yes)
                    {
                        ViewListOfOpenCustomerTickets();

                        int ticketID = InputOutputAnimationControl.SelectTicketID();
                        if (CheckIfTicketIDWithStatusOpenExistsInList(ticketID) == false)
                        {
                            Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]");
                            System.Threading.Thread.Sleep(1000);
                            CloseCustomerTicket();
                        }                        
                        string closeTicket = $"Are you sure you want to mark ticket {ticketID} as closed?";
                        string optionYesOrNo2 = SelectMenu.MenuColumn(new List<string> { yes, no }, currentUsername, closeTicket).option;
                        if (optionYesOrNo2 == yes)
                        {
                            dbcon.Open();
                            SqlCommand closeCustomerTicket = new SqlCommand($"EXECUTE SetTicketStatusToClosed {ticketID}", dbcon);
                            closeCustomerTicket.ExecuteScalar();
                            InputOutputAnimationControl.QuasarScreen(currentUsername);
                            InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                            Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully marked as closed");
                            System.Threading.Thread.Sleep(1000);
                        }

                        else if (optionYesOrNo2 == no)
                        {
                            ManageCustomerTickets();
                        }
                    }

                    else if (optionYesOrNo == no)
                    {
                        int ticketID = InputOutputAnimationControl.SelectTicketID();
                        if (CheckIfTicketIDWithStatusOpenExistsInList(ticketID) == false)
                        {
                            Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]");
                            System.Threading.Thread.Sleep(1000);
                            ManageCustomerTickets();
                        }
                        string closeTicketMsg = $"Are you sure you want to mark ticket {ticketID} as closed?";

                        string optionYesOrNo2 = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, closeTicketMsg).option;

                        if (optionYesOrNo2 == yes)
                        {
                            dbcon.Open();
                            SqlCommand closeCustomerTicket = new SqlCommand($"EXECUTE SetTicketStatusToClosed {ticketID}", dbcon);
                            closeCustomerTicket.ExecuteScalar();
                            InputOutputAnimationControl.QuasarScreen(currentUsername);
                            InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                            Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully marked as closed");
                            System.Threading.Thread.Sleep(1000);
                        }
                        else if (optionYesOrNo2 == no)
                        {
                            ManageCustomerTickets();
                        }
                    }
                }
                ManageCustomerTickets();
            }
        }

        public static void DeleteExistingOpenOrClosedTicketFunction()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("DELETE EXISTING TECHNICAL TICKETS");

            string listMsg = "Would you like to open the list of Existing Tickets?";
            string yes = "Yes", no = "No";

            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, listMsg).option;
                using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
                {
                    if (optionYesOrNo == yes)
                    {
                        ViewListOfAllCustomerTickets();

                        int ticketID = InputOutputAnimationControl.SelectTicketID();
                        if (CheckIfTicketIDWithStatusOpenOrClosedExistsInList(ticketID) == false)
                        {
                            Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]");
                            System.Threading.Thread.Sleep(1000);
                            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                        }                        
                        string deleteTicketMsg = $"Are you sure you want to delete ticket {ticketID}? Action cannot be undone";

                        string optionYesOrNo2 = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, deleteTicketMsg).option;
                        if (optionYesOrNo2 == yes)
                        {
                            dbcon.Open();
                            SqlCommand deleteCustomerTicket = new SqlCommand($"EXECUTE DeleteCustomerTicket {ticketID}", dbcon);
                            deleteCustomerTicket.ExecuteScalar();
                            InputOutputAnimationControl.QuasarScreen(currentUsername);
                            InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                            Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully deleted");
                            System.Threading.Thread.Sleep(1000);
                            ManageCustomerTickets();
                        }
                        else if (optionYesOrNo2 == no)
                        {
                            ManageCustomerTickets();
                        }
                    }

                    else if (optionYesOrNo == no)
                    {
                        int ticketID = InputOutputAnimationControl.SelectTicketID();
                        if (CheckIfTicketIDWithStatusOpenOrClosedExistsInList(ticketID) == false)
                        {
                            Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]");
                            System.Threading.Thread.Sleep(1000);
                            ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                        }
                        Console.WriteLine($"Are you sure you want to delete ticket {ticketID}? Action cannot be undone");
                        string deleteTicketMsg = $"Are you sure you want to delete ticket {ticketID}? Action cannot be undone";

                        string optionYesOrNo2 = SelectMenu.MenuColumn(new List<string> { yes, no }, currentUsername, deleteTicketMsg).option;

                        if (optionYesOrNo2 == yes)
                        {
                            dbcon.Open();
                            SqlCommand deleteCustomerTicket = new SqlCommand($"EXECUTE DeleteCustomerTicket {ticketID}", dbcon);
                            deleteCustomerTicket.ExecuteScalar();
                            InputOutputAnimationControl.QuasarScreen(currentUsername);
                            InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                            Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully deleted");
                            System.Threading.Thread.Sleep(1000);
                        }
                        else if (optionYesOrNo2 == no)
                        {
                            ManageCustomerTickets();
                        }
                    }
                }                
            }
        }

        //public static void DeleteExistingOpenOrClosedTicketSubFunction()
        //{

        //    int ticketID = InputOutputAnimationControl.SelectTicketID();
        //    if (CheckIfTicketIDWithStatusOpenOrClosedExistsInList(ticketID) == false)
        //    {
        //        Console.WriteLine($"There is no Customer Ticket with [ID = {ticketID}]");
        //        System.Threading.Thread.Sleep(1000);
        //        ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
        //    }
        //    string deleteTicketMsg = $"Are you sure you want to delete ticket {ticketID}? Action cannot be undone";

        //    string optionYesOrNo2 = SelectMenu.MenuColumn(new List<string> { yes, no }, currentUsername, deleteTicketMsg).NameOfChoice;
        //    if (optionYesOrNo2 == yes)
        //    {
        //        dbcon.Open();
        //        SqlCommand deleteCustomerTicket = new SqlCommand($"EXECUTE DeleteCustomerTicket {ticketID}", dbcon);
        //        deleteCustomerTicket.ExecuteScalar();
        //        InputOutputAnimationControl.QuasarScreen(currentUsername);
        //        InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
        //        Console.WriteLine($"Customer ticket with CustomerID = {ticketID} has been successfully deleted");
        //        System.Threading.Thread.Sleep(1000);
        //    }
        //    else if (optionYesOrNo2 == no)
        //    {
        //        ManageCustomerTickets();
        //    }
        //}




        public static void ViewExistingOpenTicketsFunction()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("VIEW OPEN TECHNICAL TICKETS");

            //Console.WriteLine("Would you like to open the list of Opened Tickets?");
            string listTicketsMsg = "Would you like to open the list of Opened Tickets?";
            string yes = "Yes", no = "No";
            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, listTicketsMsg).option;
                if (optionYesOrNo == yes)
                {
                    ViewListOfOpenCustomerTickets();
                    int TicketID = InputOutputAnimationControl.SelectTicketID();
                    if (CheckIfTicketIDWithStatusOpenExistsInList(TicketID) == false)
                    {
                        Console.WriteLine($"There is no Customer Ticket with [ID = {TicketID}]");
                        System.Threading.Thread.Sleep(1000);
                        InputOutputAnimationControl.QuasarScreen(currentUsername);
                        InputOutputAnimationControl.UniversalLoadingOuput("Loading");
                        ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                    }

                    ViewSingleCustomerTicket(TicketID);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);

                }
                else if (optionYesOrNo == no)
                {
                    int TicketID = InputOutputAnimationControl.SelectTicketID();
                    if (CheckIfTicketIDWithStatusOpenExistsInList(TicketID) == false)
                    {
                        Console.WriteLine($"There is no Customer Ticket with [ID = {TicketID}]");
                        System.Threading.Thread.Sleep(1000);
                        InputOutputAnimationControl.QuasarScreen(currentUsername);
                        InputOutputAnimationControl.UniversalLoadingOuput("Loading");
                        ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                    }

                    ViewSingleCustomerTicket(TicketID);
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }
            }
        }

        private static void ViewSingleCustomerTicket(int ticketID)
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine($"VIEW TECHNICAL TICKET WITH [ID = {ticketID}]");

            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand ShowTicketsFromDatabase = new SqlCommand($"EXECUTE SelectSingleCustomerTicket {ticketID}", dbcon);
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
                Console.Write("Press any key to return");
                Console.ReadKey();
            }
        }

        private static void ViewListOfOpenCustomerTickets()
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

        private static void ViewListOfAllCustomerTickets()
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

        public static void EditExistingOpenTicketFunction()
        {
            InputOutputAnimationControl.QuasarScreen(currentUsername);
            InputOutputAnimationControl.UniversalLoadingOuput("Loading");
            Console.WriteLine("EDIT OPEN TECHNICAL TICKETS");

            Console.WriteLine("Would you like to open the list of Opened Tickets?");

            string listTicketsMsg = "Would you like to open the list of Opened Tickets?";

            string yes = "Yes", no = "No";
            while (true)
            {
                string optionYesOrNo = SelectMenu.MenuRow(new List<string> { yes, no }, currentUsername, listTicketsMsg).option;

                if (optionYesOrNo == yes)
                {
                    ViewListOfOpenCustomerTickets();
                    int TicketID = InputOutputAnimationControl.SelectTicketID();
                    if (CheckIfTicketIDWithStatusOpenExistsInList(TicketID) == false)
                    {
                        Console.WriteLine($"There is no Customer Ticket with [ID = {TicketID}]");
                        System.Threading.Thread.Sleep(1000);
                        ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                    }
                    else
                    {
                        EditTicketOptions(TicketID);
                        InputOutputAnimationControl.QuasarScreen(currentUsername);
                        Console.WriteLine();                        

                        string viewTicketMsg = $"Would you like to view the edited Ticket {TicketID}?";

                        while (true)
                        {
                            optionYesOrNo = SelectMenu.MenuColumn(new List<string> { yes, no }, currentUsername, viewTicketMsg).option;
                            if (optionYesOrNo == yes)
                            {
                                ViewSingleCustomerTicket(TicketID);
                                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                            }
                            else if (optionYesOrNo == no)
                            {
                                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                            }
                        }                       
                    }
                }
                else if (optionYesOrNo == no)
                {
                    int TicketID = InputOutputAnimationControl.SelectTicketID();
                    if (CheckIfTicketIDWithStatusOpenExistsInList(TicketID) == false)
                    {
                        Console.WriteLine($"There is no Customer Ticket with [ID = {TicketID}]");
                        System.Threading.Thread.Sleep(1000);
                        ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                    }
                    else
                    {
                        EditTicketOptions(TicketID);
                        InputOutputAnimationControl.QuasarScreen(currentUsername);
                        Console.WriteLine();                        

                        string viewTicketMsg = $"Would you like to view the edited Ticket {TicketID}?";
                        while (true)
                        {

                            optionYesOrNo = SelectMenu.MenuColumn(new List<string> { yes, no }, currentUsername, viewTicketMsg).option;
                            if (optionYesOrNo == yes)
                            {
                                ViewSingleCustomerTicket(TicketID);
                                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                            }
                            else if (optionYesOrNo == no)
                            {
                                ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                            }
                        }
                    }
                }
            }
        }

        private static void EditTicketOptions(int ID)
        {
            string edit = "Edit Ticket Comment", assign = "Edit Ticket's User assignment", back = "Back",
                editMsg = "\r\nChoose one of the following options to continue:\r\n";
            while (true)
            {
                string EditCommentAndAssignment = SelectMenu.MenuColumn(new List<string> { edit, assign, back }, currentUsername, editMsg).option;

                if (EditCommentAndAssignment == edit)
                {
                    string ticketComment = InputOutputAnimationControl.TicketComment();
                    EditCommentOfOpenTicket(ID, ticketComment);
                }

                else if (EditCommentAndAssignment == assign)
                {
                    string newUserAssignment = AssignTicketToUser();
                    ChangeUserAssignmentToOpenTicket(ID, newUserAssignment);
                }

                else if (EditCommentAndAssignment == back)
                {
                    InputOutputAnimationControl.QuasarScreen(currentUsername);
                    InputOutputAnimationControl.UniversalLoadingOuput("Loading");
                    ActiveUserFunctions.UserFunctionMenuScreen(currentUsernameRole);
                }             
            }
        }

        private static void EditCommentOfOpenTicket(int ID, string ticketComment)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand EditTicketCommendInDatabase = new SqlCommand($"EditCustomerTicketCommentSection '{ticketComment}', {ID}", dbcon);
                EditTicketCommendInDatabase.ExecuteScalar();
            }
            Console.WriteLine($"The comment section of the Customer Ticket with [ID = {ID}] has been successfully edited");
            System.Threading.Thread.Sleep(1000);
        }

        private static void ChangeUserAssignmentToOpenTicket(int ID, string nextOwner)
        {
            using (SqlConnection dbcon = new SqlConnection(Globals.connectionString))
            {
                dbcon.Open();
                SqlCommand EditTicketUserOwnerInDatabase = new SqlCommand($"EXECUTE ChangeUserAssignedTo '{nextOwner}', {ID}", dbcon);
                EditTicketUserOwnerInDatabase.ExecuteScalar();
            }
            if (nextOwner == currentUsername)
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"The ownership of the Customer Ticket with [ID = {ID}] remains to User: {nextOwner}");
                System.Threading.Thread.Sleep(1000);
            }
            else
            {
                InputOutputAnimationControl.QuasarScreen(currentUsername);
                InputOutputAnimationControl.UniversalLoadingOuput("Action in progress");
                Console.WriteLine($"The ownership of the Customer Ticket with [ID = {ID}] has been successfully transfered to User: {nextOwner}");
                System.Threading.Thread.Sleep(1000);
            }


        }

        private static bool CheckIfTicketIDWithStatusOpenExistsInList(int ID)
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
            return true;
        }

        private static bool CheckIfTicketIDWithStatusOpenOrClosedExistsInList(int ID)
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
            return true;
        }
    }
}