using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PocketAuditor.Class;
using System;
using System.Collections.Generic;

namespace PocketAuditor.Adapter
{
    public class ActionPlanAdapter : RecyclerView.Adapter
    {
        private readonly List<ActionPlanModel> actionPlans;

        public ActionPlanAdapter(List<ActionPlanModel> actionPlan)
        {
            actionPlans = actionPlan;
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

            viewActionPlan.ActionPlan = (TextView)model.ActionPlan;
            viewActionPlan.Planname = (TextView)model.PlanName;
        }

        public override int ItemCount => actionPlans.Count;
    }

    public class ActionPlanAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView ActionPlan;
        public TextView Planname;

        public ActionPlanAdapterViewHolder(View itemView) : base(itemView)
        {
            ActionPlan = itemView.FindViewById<TextView>(Resource.Id.txtAPs);
            Planname = itemView.FindViewById<TextView>(Resource.Id.txtPlanName);
        }
    }
}