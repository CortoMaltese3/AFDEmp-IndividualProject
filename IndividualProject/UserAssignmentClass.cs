using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    public enum UserRoles
    {
        super_admin,
        administrator,
        moderator,
        user
    }

    public abstract class UserAssignmentClass
    {
        public static string connectionString = "Server=localhost; Database = Project1_Individual; User Id = admin; Password = admin";
        public string Username;
        public string Passphrase;
        public UserRoles LevelOfUserAccess;

    }

    public class SuperAmdin : UserAssignmentClass
    {

        
    }
}