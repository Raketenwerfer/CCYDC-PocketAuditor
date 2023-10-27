using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using PocketAuditor.Adapter;
using PocketAuditor.Class;
using PocketAuditor.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Android.Security.Identity.CredentialDataResult;

namespace PocketAuditor.Activity
{
    [Activity(Label = "ActionPlanActivity")]
    public class ActionPlanActivity : AppCompatActivity 
    {
        ImageView ReturnMA, AddNewPlan;
        RecyclerView DisplayPlans;
        ActionPlanAdapter adapter;

        public List<ActionPlanModel> PlanList = new List<ActionPlanModel>();

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.action_plans);

            AddNewPlan = FindViewById<ImageView>(Resource.Id.addnewPlan);
            ReturnMA = FindViewById<ImageView>(Resource.Id.returnManageAudit);

            DisplayPlans = FindViewById<RecyclerView>(Resource.Id.R_displayPlan);
            DisplayPlans.SetLayoutManager(new LinearLayoutManager(this));

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase; 

            AddNewPlan.Click += AddNewPlan_Click; 
            ReturnMA.Click += ReturnMA_Click;


            PullActionPlans();
            
        }

        private void ReturnMA_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void AddNewPlan_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AddActionPlanActivity));
            StartActivity(intent);
        }

        public void PullActionPlans()
        {
            PlanList.Clear();

            int q_AP_ID, q_AP_CD_ID;
            string q_APName, q_APdetail, q_APlink, q_APStatus, q_APtype, q_AP_CD;
            string entryQuery = "SELECT A.ActionPlanName, A.ActionPlanDetail, AtC.ActionPlanID, A.ActionPlanType, A.ExternalLink, " +
                "A.ActionPlanStatus, C.CategoryTitle, AtC.CategoryID, C.CategoryStatus " +
                "FROM Associate_APtoC AtC " +
                "INNER JOIN ActionPlans A ON AtC.ActionPlanID = A.ActionPlanID " +
                "INNER JOIN Category_tbl C ON AtC.CategoryID = C.Category_ID " +
                "WHERE (A.ActionPlanStatus == 'ACTIVE' AND C.CategoryStatus == 'ACTIVE')";

            ICursor cList = SQLDB.RawQuery(entryQuery, new string[] { });

            if (cList.Count > 0)
            {
                cList.MoveToFirst();

                do
                {
                    q_APName = cList.GetString(cList.GetColumnIndex("ActionPlanName"));
                    q_AP_ID = cList.GetInt(cList.GetColumnIndex("ActionPlanID"));
                    q_APdetail = cList.GetString(cList.GetColumnIndex("ActionPlanDetail"));
                    q_APlink = cList.GetString(cList.GetColumnIndex("ExternalLink"));
                    q_APStatus = cList.GetString(cList.GetColumnIndex("ActionPlanStatus"));
                    q_APtype = cList.GetString(cList.GetColumnIndex("ActionPlanType"));
                    q_AP_CD = cList.GetString(cList.GetColumnIndex("CategoryTitle"));
                    q_AP_CD_ID = cList.GetInt(cList.GetColumnIndex("CategoryID"));


                    ActionPlanModel a = new ActionPlanModel(q_APName,q_AP_ID,q_APdetail,q_APlink,q_APStatus,q_APtype, q_AP_CD, q_AP_CD_ID);

                    PlanList.Add(a);
                }
                while (cList.MoveToNext());
                cList.Close();
            }

            adapter = new ActionPlanAdapter(PlanList, this);
            DisplayPlans.SetAdapter(adapter);
        }

    }
}