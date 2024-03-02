using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_Categories
    {
        public int CategoryID { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryStatus { get; set; }

        public mdl_Categories(int categoryID, string categoryTitle, string categoryStatus)
        {
            CategoryID = categoryID;
            CategoryTitle = categoryTitle;
            CategoryStatus = categoryStatus;
        }
    }
}
