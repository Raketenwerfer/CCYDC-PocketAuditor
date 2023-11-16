using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using System;
using Pocket_Auditor_Admin_Panel.Classes;
using System.Collections.Generic;

namespace PocketAuditor.Adapter
{
    internal class adpt_Indicators : RecyclerView.Adapter
    {
        public event EventHandler<adpt_IndicatorsClickEventArgs> ItemClick;
        public event EventHandler<adpt_IndicatorsClickEventArgs> ItemLongClick;
        List<mdl_Indicators> items;

        public adpt_Indicators(List<mdl_Indicators> data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.mdl_indicator;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new adpt_IndicatorsViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_IndicatorsViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Count;

        void OnClick(adpt_IndicatorsClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(adpt_IndicatorsClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class adpt_IndicatorsViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public adpt_IndicatorsViewHolder(View itemView, Action<adpt_IndicatorsClickEventArgs> clickListener,
                            Action<adpt_IndicatorsClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new adpt_IndicatorsClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new adpt_IndicatorsClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class adpt_IndicatorsClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}