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

        public adpt_Categories(List<mdl_Categories> adpt_bucket)
        {
            categories = adpt_bucket;
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
        }

        public override int ItemCount => categories.Count;
    }

    public class adpt_CategoriesViewHolder : RecyclerView.ViewHolder
    {
        public TextView CategoryTitle;


        public adpt_CategoriesViewHolder(View itemView) : base(itemView)
        {
            CategoryTitle = itemView.FindViewById<TextView>(Resource.Id.CategoryTitle);

        }
    }
}