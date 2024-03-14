using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class jmdl_CategoriesSubCategories
    {
        public int SubCategoryID_fk { get; set; }
        public int CategoryID_fk { get; set; }
        public string CategoryTitle { get; set; }
        public string SubCategoryTitle { get; set; }
        public string SubCategoryStatus { get; set; }

        public jmdl_CategoriesSubCategories(int catid, int subcatid, string CatTitle, string subCategoryTitle, string subCategoryStatus)
        {
            SubCategoryID_fk = subcatid;
            CategoryID_fk = catid;
            CategoryTitle = CatTitle;
            SubCategoryTitle = subCategoryTitle;
            SubCategoryStatus = subCategoryStatus;
        }
    }
}
