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
    public class ActionPlanModel
    {
        public string APName { get; set; }
        public int AP_ID { get; set; }
        public string AP_Detail { get; set; }
        public string AP_ExtLink { get; set; }
        public string AP_Status { get; set; }
        public string AP_Type { get; set; }

        public ActionPlanModel(string aPName, int aPid, string aP_Detail, string aP_ExtLink, string aP_Status, string aP_Type)
        {
            APName = aPName;
            AP_ID = aPid;
            AP_Detail = aP_Detail;
            AP_ExtLink = aP_ExtLink;
            AP_Status = aP_Status;
            AP_Type = aP_Type;
        }
    }
}