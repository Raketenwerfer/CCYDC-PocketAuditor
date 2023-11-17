using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using System;
using Pocket_Auditor_Admin_Panel.Classes;
using System.Collections.Generic;
using System.Linq;

namespace PocketAuditor.Adapter
{
    internal class adpt_Indicators : RecyclerView.Adapter
    {
        public event EventHandler<adpt_IndicatorsClickEventArgs> ItemClick;


        List<mdl_Indicators> indicators;
        List<jmdl_IndicatorsSubInd> jmISI;

        public adpt_Indicators(List<mdl_Indicators> adpt_bucket_1, List<jmdl_IndicatorsSubInd> adpt_bucket_2)
        {
            indicators = adpt_bucket_1;
            jmISI = adpt_bucket_2;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.mdl_indicator;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new adpt_IndicatorsViewHolder(itemView, OnClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = indicators[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_IndicatorsViewHolder;
            //holder.TextView.Text = items[position];


            holder.IndicatorTitle.Text = item.Indicator;
            
            if (item.IndicatorType == "COMPOSITE")
            {
                int count = 0;

                count = jmISI.Where(a => a.IndicatorID_fk.Equals(item.IndicatorID)).Count();

                holder.SubIndicatorAmount.Visibility = ViewStates.Visible;
                holder.SubIndicatorAmount.Text = count.ToString();
            }
            else
            {
                holder.SubIndicatorAmount.Visibility = ViewStates.Gone;
            }
        }

        public override int ItemCount => indicators.Count;

        void OnClick(adpt_IndicatorsClickEventArgs args) => ItemClick?.Invoke(this, args);

    }

    public class adpt_IndicatorsViewHolder : RecyclerView.ViewHolder
    {
        public TextView IndicatorTitle;
        public TextView SubIndicatorAmount;
        public ImageView IndicatorAnswerStatus;


        public adpt_IndicatorsViewHolder(View itemView, Action<adpt_IndicatorsClickEventArgs> clickListener) : base(itemView)
        {
            IndicatorTitle = itemView.FindViewById<TextView>(Resource.Id.txt_indicatorTitle);
            SubIndicatorAmount = itemView.FindViewById<TextView>(Resource.Id.txt_subIndicatorAmount);
            IndicatorAnswerStatus = itemView.FindViewById<ImageView>(Resource.Id.img_indicatorAnswerStatus);

            itemView.Click += (sender, e) => clickListener(new adpt_IndicatorsClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class adpt_IndicatorsClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}