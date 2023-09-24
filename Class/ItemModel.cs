﻿using Android.App;
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
    public class ItemModel
    {
       //*** IDs (EntryID, CategoryID, AnswerID)

        public string CatID { get; set; }
        public string EntryID { get; set; }
        public string EntryQuestion { get; set; }
        public string CategoryTitle { get; set; }
        public bool checkValue { get; set; }
        public string Remark { get; set; }
        public string btnIsInteracted { get; set; }
        public string isTrue { get; set; }

        public bool IsNoBtnSelected { get; set; }
        public bool IsYesBtnSelected {  get; set; }


        public ItemModel(string cat_id, string cat_title, string entry_id, string question, string remark, string isInteracted, string istrue)
        {
            CatID = cat_id;
            CategoryTitle = cat_title;
            EntryID = entry_id;
            EntryQuestion = question;
            Remark = remark;
            btnIsInteracted = isInteracted;
            isTrue = istrue;

            IsNoBtnSelected = false;
            IsYesBtnSelected = false;
        }
    }
}