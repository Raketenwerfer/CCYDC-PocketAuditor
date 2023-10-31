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
using SQLite;
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

        EditText planName, planCateDesc, planPasteLink;
        Spinner categorySpin;
        Button addPlan, cancelPlan, deletePlan;
        CheckBox typeToggle;

        public List<ActionPlanModel> PlanList = new List<ActionPlanModel>();

        int selectedCategoryID, selectedActionPlanID, sequence;
        public readonly ActionPlanActivity _AAP;
        public readonly List<CategoryModel> _Category = new List<CategoryModel>();
        public readonly List<string> ap_category = new List<string>();

        readonly string get_sequence = "SELECT COUNT(*) FROM ActionPlans";

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
            //return to manage audit layout
            Finish();
        }

        private void AddNewPlan_Click(object sender, EventArgs e)
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);

            LayoutInflater layoutInflater = LayoutInflater.From(this);
            View apView = layoutInflater.Inflate(Resource.Layout.action_plans_prompt, null);
     
            builder.SetView(apView);

            builder.Create();

            Android.App.AlertDialog dialog = builder.Create();

            planName = apView.FindViewById<EditText>(Resource.Id.planName);
            planCateDesc = apView.FindViewById<EditText>(Resource.Id.plan_Details);
            planPasteLink = apView.FindViewById<EditText>(Resource.Id.plan_PasteLink);
            categorySpin = apView.FindViewById<Spinner>(Resource.Id.plan_cateDesignation);

            addPlan = apView.FindViewById<Button>(Resource.Id.addPlanBtn);
            cancelPlan = apView.FindViewById<Button>(Resource.Id.cancelPlanBtn);
            deletePlan = apView.FindViewById<Button>(Resource.Id.deletePlanBtn);

            typeToggle = apView.FindViewById<CheckBox>(Resource.Id.cxbox_APtypeToggle);

            builder.SetCancelable(false);

            addPlan.Click += delegate
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
                        _AddPlan("GENERAL", planName, planCateDesc, planPasteLink);
                        PullActionPlans();
                        dialog.Dismiss();
                    }
                    else
                    {
                        _AddPlan("SPECIFIC", planName, planCateDesc, planPasteLink);
                        PullActionPlans();
                        dialog.Dismiss();
                    }
                }

            };

            cancelPlan.Click += delegate
            {
                dialog.Dismiss();
            };

            categorySpin.ItemSelected += (sender, args) => GetCategoryID(categorySpin);

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

            deletePlan.Visibility = ViewStates.Gone;
            PullCategories(categorySpin);

            dialog.Show();

        }

        #region Action Plans Recycler View
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
                "WHERE (A.ActionPlanStatus == 'ACTIVE' AND C.CategoryStatus == 'ACTIVE') " +
                "OR (A.ActionPlanStatus == 'UNASSIGNED' AND C.CategoryStatus == 'ACTIVE')";

            ICursor cList = SQLDB.RawQuery(entryQuery, new string[] { });

            if (cList.Count > 0)
            {
                cList.MoveToFirst();

                do
                {
                    q_APName   = cList.GetString(cList.GetColumnIndex("ActionPlanName"));
                    q_AP_ID    = cList.GetInt(cList.GetColumnIndex("ActionPlanID"));
                    q_APdetail = cList.GetString(cList.GetColumnIndex("ActionPlanDetail"));
                    q_APlink   = cList.GetString(cList.GetColumnIndex("ExternalLink"));
                    q_APStatus = cList.GetString(cList.GetColumnIndex("ActionPlanStatus"));
                    q_APtype   = cList.GetString(cList.GetColumnIndex("ActionPlanType"));
                    q_AP_CD    = cList.GetString(cList.GetColumnIndex("CategoryTitle"));
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

        #endregion 

        #region Inflated Layout Methods

        public void _AddPlan(string aptype, EditText planName, EditText planCateDesc, EditText planPasteLink)
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


        public void GetCategoryID(Spinner categorySpin)
        {
            foreach (CategoryModel cm in _Category)
            {
                if (cm.CategoryTitle == categorySpin.SelectedItem.ToString())
                {
                    selectedCategoryID = cm.CategoryID;
                }
            }

            Toast.MakeText(Application.Context, "Selected " + categorySpin.SelectedItem.ToString(), ToastLength.Short).Show();
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

        public void PullCategories(Spinner spinner)
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

                PopulateCategoriesSpinner(spinner);
            }
        }

        public void _PullActionPlanID()
        {
            string ID_quesry = "SELECT ActionPlanID FROM ActionPlans WHERE ActionPlanStatus = 'ACTIVE'";
            ICursor qAPID = SQLDB.RawQuery(ID_quesry, new string[] { });

            if (qAPID.Count > 0)
            {
                qAPID.MoveToLast();

                selectedActionPlanID = qAPID.GetInt(qAPID.GetColumnIndex("ActionPlanID"));
            }
            qAPID.Close();

        }

        private void PopulateCategoriesSpinner(Spinner categorySpin)
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

        #endregion
    }
}
