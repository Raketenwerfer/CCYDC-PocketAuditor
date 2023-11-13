using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class rmdl_IndicatorAnswers
    {
        public int ChapterID_fk { get; set; }
        public bool Answer { get; set; }
        public int IndicatorID_fk { get; set; }
        public int CategoryID_fk { get; set; }

        public rmdl_IndicatorAnswers(int chapterID_fk, bool answer, int indicatorID_fk, int categoryID_fk)
        {
            ChapterID_fk = chapterID_fk;
            Answer = answer;
            IndicatorID_fk = indicatorID_fk;
            CategoryID_fk = categoryID_fk;
        }
    }
}
