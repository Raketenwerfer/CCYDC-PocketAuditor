using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using System;
using Pocket_Auditor_Admin_Panel.Classes;
using System.Collections.Generic;
using System.Linq;

namespace PocketAuditor.Adapter
{
    internal class adpt_SubIndDetails : RecyclerView.Adapter
    {
        public event EventHandler<adpt_SubIndDetailsClickEventArgs> ItemClick;
        public event EventHandler<adpt_SubIndDetailsClickEventArgs> ItemLongClick;
        List<mdl_SubIndicators> items;

        public adpt_SubIndDetails(List<mdl_SubIndicators> data)
        {
            items = (List<mdl_SubIndicators>)data.Where(x => x.SubIndicatorType.Equals("DETAILS"));
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.mdl_subind_details;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new adpt_SubIndDetailsViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_SubIndDetailsViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Count;

        void OnClick(adpt_SubIndDetailsClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(adpt_SubIndDetailsClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class adpt_SubIndDetailsViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public adpt_SubIndDetailsViewHolder(View itemView, Action<adpt_SubIndDetailsClickEventArgs> clickListener,
                            Action<adpt_SubIndDetailsClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new adpt_SubIndDetailsClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new adpt_SubIndDetailsClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class adpt_SubIndDetailsClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}