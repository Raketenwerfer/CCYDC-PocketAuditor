﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pocket_Auditor_Admin_Panel.Classes
{
    public class mdl_ChapterScores
    {
        public int ChapterID_fk { get; set; }
        public int CategoryID_fk { get; set; }
        public double Score { get; set; }

        public mdl_ChapterScores(int chapterID_fk, int categoryID_fk, double score)
        {
            ChapterID_fk = chapterID_fk;
            CategoryID_fk = categoryID_fk;
            Score = score;
        }
    }
}