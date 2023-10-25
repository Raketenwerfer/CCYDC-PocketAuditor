using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using AndroidX.AppCompat.App;
using PocketAuditor.Class;
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
        EditText planName, planCateDesc, planPasteLink;
        Spinner categorySpin;
        Button addPlan, cancelPlan;
        RecyclerView recylerAP;

        public List<CategoryModel> _Category = new List<CategoryModel>();
        public readonly List<string> ap_category = new List<string>();

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.action_plans_prompt);

            planName = FindViewById<EditText>(Resource.Id.planName);
            planCateDesc = FindViewById<EditText>(Resource.Id.plan_CatDescription);
            planPasteLink = FindViewById<EditText>(Resource.Id.plan_PasteLink);
            categorySpin = FindViewById<Spinner>(Resource.Id.cateSpin);

            addPlan = FindViewById<Button>(Resource.Id.addPlanBtn);
            cancelPlan = FindViewById<Button>(Resource.Id.cancelPlanBtn);
             
            addPlan.Click += AddPlan_Click;
            cancelPlan.Click += CancelPlan_Click;

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;


            PullCategories();


        }

        private void AddPlan_Click(object sender, EventArgs e)
        {
            string Name = planName.Text; 
            string cateDisc = planCateDesc.Text;
            string pasteLink = planPasteLink.Text;

            if (string.IsNullOrWhiteSpace(Name) || 
                string.IsNullOrWhiteSpace(cateDisc) || string.IsNullOrWhiteSpace(pasteLink))
            {
                Toast.MakeText(this, "Fields must be Filled", ToastLength.Short).Show();
            }

            else
            {
                _AddPlan();
                Intent intent = new Intent(this, typeof(ActionPlanActivity));
                StartActivity(intent);
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

            _db.Execute("INSERT INTO ActionPlans(ActionPlanName, ActionPlanDetail, ExternalLink, ActionPlanStatus) " + 
                "VALUES(?, ?, ?)", planName.Text, planCateDesc.Text, planPasteLink.Text, "ACTIVE");

            Toast.MakeText(Application.Context, "New Action Plan created!", ToastLength.Short).Show();
            _db.Commit();



            _db.Close();
        }


        public void _AttachPlanToCategory()
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("");


            _db.Commit();



            _db.Close();
        }

        public void PullCategories()
        {
            _Category.Clear();

            // Re-added, new categories are viewable on my end with this code. Gibutang nako ni para tabang refresh sa category
            // list
            int q_CatID;
            string catQuery = "SELECT * FROM Category_tbl WHERE CategoryStatus = 'ACTIVE'";

            ICursor cList = SQLDB.RawQuery(catQuery, new string[] { });

            if (cList.Count > 0)
            {
                cList.MoveToFirst();

                do
                {
                    q_CatID = cList.GetInt(cList.GetColumnIndex("Category_ID"));
                    string q_CatTitle = cList.GetString(cList.GetColumnIndex("CategoryTitle"));

                    CategoryModel a = new CategoryModel(q_CatID, q_CatTitle);

                    _Category.Add(a);
                }
                while (cList.MoveToNext());

                cList.Close();

                PopulateCategoriesSpinner();
            }
        }

        private void PopulateCategoriesSpinner()
        {
            foreach (CategoryModel CM in _Category)
            {
                if (!ap_category.Exists(i => i == CM.CategoryTitle))
                {
                    ap_category.Add(CM.CategoryTitle);
                }
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ap_category.Select(a => a).ToList());
            categorySpin.Adapter = adapter;
        }
    }
}