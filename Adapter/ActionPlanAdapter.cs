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

namespace PocketAuditor.Adapter
{
    public class ActionPlanAdapter : RecyclerView.Adapter
    {
        private readonly List<ActionPlanModel> actionPlans;
        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        private readonly ActionPlanActivity _APActivity;

        public ActionPlanAdapter(List<ActionPlanModel> actionPlan, ActionPlanActivity activity)
        {
            actionPlans = actionPlan;
            handler = new DB_Initiator(activity);
            SQLDB = handler.WritableDatabase;
            _APActivity = activity;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View viewPlans = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.action_plans_model, parent, false);
            return new ActionPlanAdapterViewHolder(viewPlans);
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
        }

        public override int ItemCount => actionPlans.Count;

        public void _EditPlan(ActionPlanActivity activity)
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("");

            Toast.MakeText(activity, "Plan Successfully Renamed!", ToastLength.Short).Show();
            _db.Commit();



            _db.Close();
        }

        public void _DeletePlan(ActionPlanActivity activity)
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("");

            Toast.MakeText(activity, "Plan Successfully Deleted!", ToastLength.Short).Show();
            _db.Commit();



            _db.Close();
        }

    }

    public class ActionPlanAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView APName;
        public TextView APDetail;
        public TextView APCatDesignation;
        public TextView APType;
        public TextView APLink;

        public ActionPlanAdapterViewHolder(View itemView) : base(itemView)
        {
            APName = itemView.FindViewById<TextView>(Resource.Id.txtAPName);
            APDetail = itemView.FindViewById<TextView>(Resource.Id.txtAPDetail);
            APCatDesignation = itemView.FindViewById<TextView>(Resource.Id.txtCategoryDesignation);
            APType = itemView.FindViewById<TextView>(Resource.Id.txtAPType);
            APLink = itemView.FindViewById<TextView>(Resource.Id.txtAPlink);
        }
    }
}