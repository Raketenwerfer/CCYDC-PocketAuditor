using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.AppCompat.App;
using PocketAuditor.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Activity
{
    [Activity(Label = "AddActionPlanActivity")]
    public class AddActionPlanActivity : AppCompatActivity
    {
        EditText planName, planCateName, planCateDesc, planPasteLink;
        Button addPlan, cancelPlan;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.action_plans_prompt);

            planName = FindViewById<EditText>(Resource.Id.planName);
            planCateName = FindViewById<EditText>(Resource.Id.plan_CatName);
            planCateDesc = FindViewById<EditText>(Resource.Id.plan_CatDescription);
            planPasteLink = FindViewById<EditText>(Resource.Id.plan_PasteLink);

            addPlan = FindViewById<Button>(Resource.Id.addPlanBtn);
            cancelPlan = FindViewById<Button>(Resource.Id.cancelPlanBtn);
             
            addPlan.Click += AddPlan_Click;
            cancelPlan.Click += CancelPlan_Click;

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;


        }

        private void AddPlan_Click(object sender, EventArgs e)
        {
            string Name = planName.Text; 
            string cateName = planCateName.Text;
            string cateDisc = planCateDesc.Text;
            string pasteLink = planPasteLink.Text;

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(cateName) ||
                string.IsNullOrWhiteSpace(cateDisc) || string.IsNullOrWhiteSpace(pasteLink))
            {
                Toast.MakeText(this, "Fields must be Filled", ToastLength.Short).Show();
            }

        }

        private void CancelPlan_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ActionPlanActivity));
            StartActivity(intent);
        }


        public void _AddPlan()
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("");

            Toast.MakeText(Application.Context, "New Action Plan created!", ToastLength.Short).Show();
            _db.Commit();



            _db.Close();
        }


        public void _AttachPlanToCategory()
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("");

            Toast.MakeText(Application.Context, "New Action Plan created!", ToastLength.Short).Show();
            _db.Commit();



            _db.Close();
        }

        public void _EditPlan()
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("");

            Toast.MakeText(Application.Context, "New Action Plan created!", ToastLength.Short).Show();
            _db.Commit();



            _db.Close();
        }

        public void _DeletePlan()
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("");

            Toast.MakeText(Application.Context, "New Action Plan created!", ToastLength.Short).Show();
            _db.Commit();



            _db.Close();
        }
    }
}