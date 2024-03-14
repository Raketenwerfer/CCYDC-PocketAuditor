using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_SubCategories
    {
        public int SubCategoryID { get; set; }
        public string SubCategoryTitle { get; set; }
        public string SubCategoryStatus { get; set; }


        public mdl_SubCategories(int id, string title, string status)
        {
            SubCategoryID = id;
            SubCategoryTitle = title;
            SubCategoryStatus = status;
        }
    }
}
