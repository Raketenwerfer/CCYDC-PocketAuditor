using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class rmdl_SubIndicatorAnswers
    {
        public int ChapterID_fk { get; set; }
        public bool SubAnswers { get; set; }
        public int SubIndicatorID_fk { get; set; }
        public int IndicatorID_fk { get; set; }

        public rmdl_SubIndicatorAnswers(int chapterID_fk, bool subAnswers, int subIndicatorID_fk, int indicatorID_fk)
        {
            ChapterID_fk = chapterID_fk;
            SubAnswers = subAnswers;
            SubIndicatorID_fk = subIndicatorID_fk;
            IndicatorID_fk = indicatorID_fk;
        }
    }
}
