using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using System;
using System.Collections.Generic;

namespace PocketAuditor.Adapter
{
    internal class adpt_Categories : RecyclerView.Adapter
    {
        public event EventHandler<adpt_CategoriesClickEventArgs> ItemClick;
        public event EventHandler<adpt_CategoriesClickEventArgs> ItemLongClick;
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


            var vh = new adpt_CategoriesViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = categories[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_CategoriesViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => categories.Count;

        void OnClick(adpt_CategoriesClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(adpt_CategoriesClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class adpt_CategoriesViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public adpt_CategoriesViewHolder(View itemView, Action<adpt_CategoriesClickEventArgs> clickListener,
                            Action<adpt_CategoriesClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new adpt_CategoriesClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new adpt_CategoriesClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class adpt_CategoriesClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}