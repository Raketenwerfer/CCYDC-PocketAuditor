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
        public ActionPlanModel(string actionPlan, string planName)
        {
            ActionPlan = actionPlan;
            PlanName = planName;
        }

        public string ActionPlan { get; set; }
        public string PlanName { get; set; }    
    }
}