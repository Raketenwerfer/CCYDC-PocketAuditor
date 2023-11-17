using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using System;
using System.Collections.Generic;

namespace PocketAuditor.Adapter
{
    internal class adpt_Categories : RecyclerView.Adapter
    {
        List<mdl_Categories> categories;
        List<jmdl_IndicatorsSubInd> isi;
        adpt_Indicators adapter;

        public adpt_Categories(List<mdl_Categories> adpt_bucket_1, List<mdl_Indicators> adpt_bucket_2, 
            List<jmdl_IndicatorsSubInd> adpt_bucket_3)
        {
            categories = adpt_bucket_1;
            isi = adpt_bucket_3;
            adapter = new adpt_Indicators(adpt_bucket_2, adpt_bucket_3);
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.mdl_category;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);


            var vh = new adpt_CategoriesViewHolder(itemView);

            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = categories[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_CategoriesViewHolder;
            //holder.TextView.Text = items[position];

            holder.CategoryTitle.Text = item.CategoryTitle;
            holder.IndicatorRecycler.SetAdapter(adapter);
        }

        public override int ItemCount => categories.Count;
    }

    public class adpt_CategoriesViewHolder : RecyclerView.ViewHolder
    {
        public TextView CategoryTitle;
        public RecyclerView IndicatorRecycler;


        public adpt_CategoriesViewHolder(View itemView) : base(itemView)
        {
            CategoryTitle = itemView.FindViewById<TextView>(Resource.Id.CategoryTitle);
            IndicatorRecycler = itemView.FindViewById<RecyclerView>(Resource.Id.IndicatorRecycler);
            IndicatorRecycler.SetLayoutManager(new LinearLayoutManager(itemView.Context)); // I don't know what context to put here
        }
    }
}