using Android.Content;
using Android.Database.Sqlite;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PocketAuditor.Class;
using PocketAuditor.Database;
using PocketAuditor.Fragment;
using System;
using System.Collections.Generic;

namespace PocketAuditor.Adapter
{
    public class ResultsRemarksAdapter : RecyclerView.Adapter
    {
        private readonly List<CategoryRemarksModel> remarksList;

        public ResultsRemarksAdapter(List<CategoryRemarksModel> remarksList, AuditResults auditResults)
        {
            this.remarksList = remarksList; 
        }
        
        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.display_results_model, parent, false);
            return new ResultsRemarksAdapterViewHolder(view);
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CategoryRemarksModel remarksModel = remarksList[position];
            ResultsRemarksAdapterViewHolder viewHolder = holder as ResultsRemarksAdapterViewHolder;

            // Bind the data to the views in the layout
            viewHolder.IndicatorNumber.Text = remarksModel.IndicatorId.ToString();
            viewHolder.CategoryRemarks.Text = remarksModel.SelectedCategoryRemarks;
        }

        public override int ItemCount => remarksList.Count;
    }

    public class ResultsRemarksAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView IndicatorNumber;
        public TextView CategoryRemarks;
        
        public ResultsRemarksAdapterViewHolder(View itemView) : base(itemView)
        {
            IndicatorNumber = itemView.FindViewById<TextView>(Resource.Id.indiNumber);
            CategoryRemarks = itemView.FindViewById<TextView>(Resource.Id.cateRemarks);
        }
    }

}