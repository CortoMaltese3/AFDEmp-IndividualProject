namespace IndividualProject
{
    public static class Globals
    {
        public static readonly string connectionString = Properties.Settings.Default.connectionString;

        public static readonly string newUserRequestPath = Properties.Settings.Default.newUserRequestPath;

        public static readonly string newUserRequestFolderPath = Properties.Settings.Default.newUserRequestFolder;

        public static readonly string TTnotificationToUser = Properties.Settings.Default.TTnotificationToUser;   
        
        public static readonly string TTnotificationToUserFolder = Properties.Settings.Default.TTnotificationToUserFolder;        
    }
}
