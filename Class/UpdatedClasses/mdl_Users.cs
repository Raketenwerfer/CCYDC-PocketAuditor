using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_Users
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string UserStatus { get; set; }

        public mdl_Users(int userID, string username, string password, string userType, string userStatus)
        {
            UserID = userID;
            Username = username;
            Password = password;
            UserType = userType;
            UserStatus = userStatus;
        }
    }
}
