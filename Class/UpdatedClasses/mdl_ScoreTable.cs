using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_ScoreTable
    {
        public int ChapterID_fk { get; set; }
        public int CategoryID_fk { get; set; }
        public string SubCategoryID_fk { get; set; }
        // Set to string as this is nullable in the database
        public int IndicatorID_fk { get; set; }
        public string SubIndicatorID_fk { get; set; }
        public bool IsChecked { get; set; }
        public string ItemChecked { get; set; }
        public string Remarks { get; set; }
        public string SubIndicatorType { get; set; }

        public mdl_ScoreTable(int chapterID_fk, int catID,
            string subcatID, int indID, string subindID, bool ischekced, string itemchecked, string remarks,
            string subIndicatorType)
        {
            ChapterID_fk = chapterID_fk;
            CategoryID_fk = catID;
            SubCategoryID_fk = subcatID;
            IndicatorID_fk = indID;
            SubIndicatorID_fk = subindID;
            IsChecked = ischekced;
            ItemChecked = itemchecked;
            Remarks = remarks;
            SubIndicatorType = subIndicatorType;
        }
    }
}
