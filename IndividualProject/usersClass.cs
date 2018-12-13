using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    class usersClass
    {
        //TODO server localhost?
        





        void CheckLoginCredentials()
        {

        }


        void AddUsernamesToDictionary()
        {
            //TODO find another way to add to dictionary
            Dictionary<string, string> userNames = new Dictionary<string, string>();
            userNames.Add("admin", "admin");            //super admin
            userNames.Add("kalogeorge", "00kalo");      //administrator
            userNames.Add("azlibi", "nom28");           //moderator
            userNames.Add("Corto", "c12342");           //user

        }

        void AddNewUsernameToDictionary()
        {
            
        }
    }
}
