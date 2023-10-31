using Android.Database.Sqlite;
using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using PocketAuditor.Activity;
using PocketAuditor.Class;
using PocketAuditor.Database;
using SQLite;
using System;
using System.Collections.Generic;
using Android.App;
using AndroidX.CardView.Widget;
using Android.Database;

namespace PocketAuditor.Adapter
{
    public class ActionPlanAdapter : RecyclerView.Adapter
    {
        private readonly List<ActionPlanModel> actionPlans;
        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        private readonly ActionPlanActivity _APActivity;

        EditText planName, planCateDesc, planPasteLink;
        Spinner categorySpin;
        Button addPlan, cancelPlan, deletePlan;
        CheckBox typeToggle;

        int selectedCategoryID, selectedActionPlanID, sequence;
        public readonly List<CategoryModel> _Category = new List<CategoryModel>();
        public readonly List<string> ap_category = new List<string>();

        readonly string get_sequence = "SELECT COUNT(*) FROM ActionPlans";

        public ActionPlanAdapter(List<ActionPlanModel> actionPlan, ActionPlanActivity activity)
        {
            actionPlans = actionPlan;
            handler = new DB_Initiator(activity);
            SQLDB = handler.WritableDatabase;
            _APActivity = activity;
        }

        public void UpdateData(List<ActionPlanModel> newData)
        {
            actionPlans.Clear();
            actionPlans.AddRange(newData);
            NotifyDataSetChanged();
        }


        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View viewPlan = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.action_plans_model, parent, false);
            return new ActionPlanAdapterViewHolder(viewPlan);

        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            ActionPlanModel model = actionPlans[position];
            ActionPlanAdapterViewHolder viewActionPlan = viewHolder as ActionPlanAdapterViewHolder;

            viewActionPlan.APName.Text = model.APName;
            viewActionPlan.APDetail.Text = model.AP_Detail;
            viewActionPlan.APType.Text = model.AP_Type;
            viewActionPlan.APLink.Text = model.AP_ExtLink;

            if (model.AP_Type == "SPECIFIC")
            {
                viewActionPlan.APCatDesignation.Visibility = ViewStates.Visible;
                viewActionPlan.APCatDesignation.Text = model.AP_CategoryDesignation;
            }
            else
            {
                viewActionPlan.APCatDesignation.Visibility = ViewStates.Invisible;
            }

            viewActionPlan.CardView.Click += (sender, args) => _ConfigurePlan(_APActivity);


        }

        public override int ItemCount => actionPlans.Count;

        public void _ConfigurePlan(ActionPlanActivity activity)
        {

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(activity);

            LayoutInflater layoutInflater = LayoutInflater.From(activity);
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
                    Toast.MakeText(activity, "Fields must be Filled", ToastLength.Short).Show();
                }

                else
                {

                    if (typeToggle.Checked)
                    {
                        _AddPlan("GENERAL", planName, planCateDesc, planPasteLink);
                        //actionPlanAdapter.UpdateData();
                        _APActivity.PullActionPlans();
                        dialog.Dismiss();
                    }
                    else
                    {
                        _AddPlan("SPECIFIC", planName, planCateDesc, planPasteLink);
                        _APActivity.PullActionPlans();
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
            _APActivity.PullCategories(categorySpin);

            dialog.Show();
        }

        internal void UpdateData(object newDataList)
        {
            throw new NotImplementedException();
        }

        #region Inflated Layout Methods

        public void _AddPlan(string aptype, EditText planName, EditText planCateDesc, EditText planPasteLink)
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _APActivity._GetRowSequenceCount();

            _db.Execute("INSERT INTO ActionPlans(ActionPlanName, ActionPlanID, ActionPlanDetail, ExternalLink, ActionPlanStatus, ActionPlanType) " +
                "VALUES(?, ?, ?, ?, ?, ?)", planName.Text, sequence, planCateDesc.Text, planPasteLink.Text, "ACTIVE", aptype);

            Toast.MakeText(Application.Context, "New Action Plan created!", ToastLength.Short).Show();
            _db.Commit();
            _db.Close();

            _APActivity._PullActionPlanID();
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



        public void _AttachPlanToCategory()
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("INSERT INTO Associate_APtoC(ActionPlanID, CategoryID) " +
                "VALUES(?,?)", selectedActionPlanID, selectedCategoryID);

            _db.Commit();
            _db.Close();
        }

        #endregion
    }

    public class ActionPlanAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView APName;
        public TextView APDetail;
        public TextView APCatDesignation;
        public TextView APType;
        public TextView APLink;
        public CardView CardView;

        public ActionPlanAdapterViewHolder(View itemView) : base(itemView)
        {
            APName = itemView.FindViewById<TextView>(Resource.Id.txtAPName);
            APDetail = itemView.FindViewById<TextView>(Resource.Id.txtAPDetail);
            APCatDesignation = itemView.FindViewById<TextView>(Resource.Id.txtCategoryDesignation);
            APType = itemView.FindViewById<TextView>(Resource.Id.txtAPType);
            APLink = itemView.FindViewById<TextView>(Resource.Id.txtAPlink);
            CardView = itemView.FindViewById<CardView>(Resource.Id.CV_ActionPlan);
        }
    }
}