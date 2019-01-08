using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndividualProject
{
    class WriteReadFromFile
    {
        public static string CheckEmptyFile()
        {
             string pendingUsername = File.ReadLines(Globals.newUserRequestPath).First();
            return pendingUsername;
        }
    }
}
