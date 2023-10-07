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
        public string CatID { get; set; }
        public string EntryID { get; set; }
        public string Indicator { get; set; }

        public QuestionModel(string catID, string entryID, string indicator)
        {
            CatID = catID;
            EntryID = entryID;
            Indicator = indicator;
        }
    }
}