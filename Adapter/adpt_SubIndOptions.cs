using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using System;
using Pocket_Auditor_Admin_Panel.Classes;
using System.Collections.Generic;
using System.Linq;

namespace PocketAuditor.Adapter
{
    internal class adpt_SubIndOptions : RecyclerView.Adapter
    {
        public event EventHandler<adpt_SubIndOptionsClickEventArgs> ItemClick;
        public event EventHandler<adpt_SubIndOptionsClickEventArgs> ItemLongClick;
        List<mdl_SubIndicators> items;

        public adpt_SubIndOptions(List<mdl_SubIndicators> data)
        {
            items = (List<mdl_SubIndicators>)data.Where(x => x.SubIndicatorType.Equals("OPTIONS"));
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.mdl_subind_options;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new adpt_SubIndOptionsViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_SubIndOptionsViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Count;

        void OnClick(adpt_SubIndOptionsClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(adpt_SubIndOptionsClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class adpt_SubIndOptionsViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public adpt_SubIndOptionsViewHolder(View itemView, Action<adpt_SubIndOptionsClickEventArgs> clickListener,
                            Action<adpt_SubIndOptionsClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new adpt_SubIndOptionsClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new adpt_SubIndOptionsClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class adpt_SubIndOptionsClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}