using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Auxiliaries;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Scores
{
    public class ResponseReader
    {
        private static ResponseReader instance;
        private int AuditorID;
        private string ID_Handle;
        DataSharingService DSS;
        DatabaseInitiator dbInit;

        public List<mdl_ScoreTable> Scores = new List<mdl_ScoreTable>();

        private ResponseReader()
        {
            DSS = DataSharingService.GetInstance();
            dbInit = DSS.GetDatabase();
        }

        public static ResponseReader GetInstance()
        {
            if (instance  == null)
            {
                instance = new ResponseReader();
            }

            return instance;
        }

        public void AddResponse(int chapter, int category, string subcategory,
            int indicator, string subindicator, bool ischecked, string itemchecked, string remarks,
            string subindtype)
        {
            Scores.Add(new mdl_ScoreTable(chapter, category, subcategory, indicator, subindicator,
                ischecked, itemchecked, remarks, subindtype));
        }
    }
}