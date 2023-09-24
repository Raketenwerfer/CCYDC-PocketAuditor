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
    public class EntryAnswersModel
    {
        public int fk_EntryID { get; set; }
        // fk_EntryID is a foreign key referencing EntryID from Entry_tbl
        // IDs for the chosen answers in EntryAnswers_tbl are in the column AnswerID
        //
        // Database Source: CCYDC.db

        public string EntryAnswer { get; set; }
        // EntryAnswer accepts string "true" and "false" data. They are pulled from
        // ItemModel, written to the database, then pulled again from there to EntryAnswersModel

        public string EntryRemark { get; set; }
        // EntryRemark are from the written remarks taken from the cardviews. Same process
        // as EntryAnswer

        public int AnswerID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public EntryAnswersModel(int pk_AID, int fk_EID, int fk_CID, string eat_EA, string eat_ER, string categoryName)
        {
            this.AnswerID = pk_AID;
            this.fk_EntryID = fk_EID;
            this.EntryAnswer = eat_EA;
            this.EntryRemark = eat_ER;
            this.CategoryID = fk_CID;
            this.CategoryName = categoryName;
        }
        //public EntryAnswersModel(int pk_AID, int fk_EID, string eat_EA, string eat_ER)
        //{
        //    this.AnswerID = pk_AID;
        //    this.fk_EntryID = fk_EID;
        //    this.EntryAnswer = eat_EA;
        //    this.EntryRemark = eat_ER;
        //}
    }
}