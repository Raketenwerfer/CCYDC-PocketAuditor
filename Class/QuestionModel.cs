using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Class
{
    public class QuestionModel
    {
        // this model is used for calling the values from th db. and displaying it to the activity_drawer in the Questions
        public int EntryID { get; set; }
        public int CatID { get; set; }
        public int QuesNo { get; set; }
        public string Indicator { get; set; }
        public int ScoreValue { get; set; }
        public string Status { get; set; }

        public QuestionModel(int entryID, int catID, int quesno, string indicator, int scorevalue, string status)
        {
            EntryID = entryID;
            CatID = catID;
            QuesNo = quesno;
            Indicator = indicator;
            ScoreValue = scorevalue;
            Status = status;
        }
    }
}