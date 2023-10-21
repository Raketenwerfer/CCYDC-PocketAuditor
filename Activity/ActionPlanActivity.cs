using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using PocketAuditor.Class;
using PocketAuditor.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Activity
{
    [Activity(Label = "ActionPlanActivity")]
    public class ActionPlanActivity : AppCompatActivity 
    {
        ImageView ReturnMA, AddNewPlan;
        RecyclerView DisplayPlans;

        public List<ActionPlanModel> planList = new List<ActionPlanModel>();

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

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
        }

        private void ReturnMA_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ManageMenu));
            StartActivity(intent);
        }

        private void AddNewPlan_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AddActionPlanActivity));
            StartActivity(intent);
        }
    }
}