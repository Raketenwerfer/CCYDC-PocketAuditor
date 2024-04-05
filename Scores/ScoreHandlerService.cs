using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Scores
{
    public class ScoreHandlerService
    {
        private static ScoreHandlerService instance;
        DataSharingService DSS;

        List<mdl_ScoreTable> Scores = new List<mdl_ScoreTable>();

        private string ID_Handle;

        private ScoreHandlerService()
        {
            DSS = DataSharingService.GetInstance();
        }

        public static ScoreHandlerService GetInstance()
        {
            if (instance  == null)
            {
                instance = new ScoreHandlerService();
            }

            return instance;
        }
    }
}