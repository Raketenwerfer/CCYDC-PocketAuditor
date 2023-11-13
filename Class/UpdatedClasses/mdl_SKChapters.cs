using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_SKChapters
    {
        public int ChapterID { get; set; }
        public string Barangay { get; set; }
        public bool hasFinishedAudit { get; set; }

        public mdl_SKChapters(int chapterID, string barangay, bool hasFinishedAudit)
        {
            ChapterID = chapterID;
            Barangay = barangay;
            this.hasFinishedAudit = hasFinishedAudit;
        }
    }
}
