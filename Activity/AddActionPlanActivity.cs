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
using SQLitePCL;
using PocketAuditor.Adapter;

namespace PocketAuditor.Activity
{
    [Activity(Label = "AddActionPlanActivity", NoHistory = true)]

    public class AddActionPlanActivity : AppCompatActivity
    {
        EditText planName, planCateDesc, planPasteLink;
        Spinner categorySpin;
        Button addPlan, cancelPlan;
        CheckBox typeToggle;

        int selectedCategoryID, selectedActionPlanID, sequence;
        public readonly ActionPlanActivity _AAP;
        public readonly List<CategoryModel> _Category = new List<CategoryModel>();
        public readonly List<string> ap_category = new List<string>();

        private readonly ActionPlanAdapter actionPlanAdapter;

        readonly string get_sequence = "SELECT COUNT(*) FROM ActionPlans";

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.action_plans_prompt);

            planName = FindViewById<EditText>(Resource.Id.planName);
            planCateDesc = FindViewById<EditText>(Resource.Id.plan_Details);
            planPasteLink = FindViewById<EditText>(Resource.Id.plan_PasteLink);
            categorySpin = FindViewById<Spinner>(Resource.Id.plan_cateDesignation);

            addPlan = FindViewById<Button>(Resource.Id.addPlanBtn);
            cancelPlan = FindViewById<Button>(Resource.Id.cancelPlanBtn);

            typeToggle = FindViewById<CheckBox>(Resource.Id.cxbox_APtypeToggle);
             
            addPlan.Click += AddPlan_Click;
            cancelPlan.Click += CancelPlan_Click;

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;

            categorySpin.ItemSelected += (sender, args) => GetCategoryID();

            typeToggle.CheckedChange += (sender, args) =>
            {
                if (typeToggle.Checked)
                {
                    categorySpin.Enabled = false;
                }
                else
                {
                    categorySpin.Enabled = true;
                }
            };

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

                if (typeToggle.Checked)
                {
                    _AddPlan("GENERAL");
                    //actionPlanAdapter.UpdateData();
                    Finish();
                }
                else
                {
                    _AddPlan("SPECIFIC");
                    Finish();
                }
                
            }
        }

        

        private void CancelPlan_Click(object sender, EventArgs e)
        {
            Finish();
        }


        public void _AddPlan(string aptype)
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _GetRowSequenceCount();
            sequence++;

            _db.Execute("INSERT INTO ActionPlans(ActionPlanName, ActionPlanID, ActionPlanDetail, ExternalLink, ActionPlanStatus, ActionPlanType) " + 
                "VALUES(?, ?, ?, ?, ?, ?)", planName.Text, sequence, planCateDesc.Text, planPasteLink.Text, "ACTIVE", aptype);

            Toast.MakeText(Application.Context, "New Action Plan created!", ToastLength.Short).Show();
            _db.Commit();
            _db.Close();

            _PullActionPlanID();
            _AttachPlanToCategory();
        }

        public void GetCategoryID()
        {
            foreach (CategoryModel cm in _Category)
            {
                if (cm.CategoryTitle == categorySpin.SelectedItem.ToString())
                {
                    selectedCategoryID = cm.CategoryID;
                }
            }

            Toast.MakeText(Application.Context, "Selected ID number " + selectedCategoryID, ToastLength.Short).Show();
        }

        public void _GetRowSequenceCount()
        {
            ICursor gseq = SQLDB.RawQuery(get_sequence, new string[] { });

            if (gseq.MoveToFirst())
            {
                sequence = gseq.GetInt(0);
            }
            else
            {
                sequence = 0;
            }

            gseq.Close();
        }


        public void _AttachPlanToCategory()
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("INSERT INTO Associate_APtoC(ActionPlanID, CategoryID) " +
                "VALUES(?,?)", selectedActionPlanID, selectedCategoryID);

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

        public void _PullActionPlanID()
        {
            string ID_quesry = "SELECT ActionPlanID FROM ActionPlans WHERE ActionPlanStatus = 'ACTIVE'";
            ICursor qAPID = SQLDB.RawQuery(ID_quesry, new string[] {});

            if (qAPID.Count > 0)
            {
                qAPID.MoveToLast();

                selectedActionPlanID = qAPID.GetInt(qAPID.GetColumnIndex("ActionPlanID"));
            }
            qAPID.Close();

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