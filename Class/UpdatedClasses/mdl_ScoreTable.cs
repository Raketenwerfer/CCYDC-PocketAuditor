using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_ScoreTable
    {
        public string EntryID { get; set; }
        public int ChapterID_fk { get; set; }
        public double Score { get; set; }
        public int AuditorID_fk { get; set; }

        public mdl_ScoreTable(string entryID, int chapterID_fk, double score, int auditorID_fk)
        {
            EntryID = entryID;
            ChapterID_fk = chapterID_fk;
            Score = score;
            AuditorID_fk = auditorID_fk;
        }
    }
}
