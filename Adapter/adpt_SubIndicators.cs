using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using System;
using Pocket_Auditor_Admin_Panel.Classes;
using System.Collections.Generic;
using System.Linq;

namespace PocketAuditor.Adapter
{
    internal class adpt_SubIndicators : RecyclerView.Adapter
    {
        List<mdl_SubIndicators> SI;
        List<jmdl_IndicatorsSubInd> jmISI, list;
        int SelectedIndID;

        public adpt_SubIndicators(int SelID,  List<jmdl_IndicatorsSubInd> pass_ISI,
            List<mdl_SubIndicators> pass_SI)
        {
            jmISI = pass_ISI;
            SelectedIndID = SelID;
            SI = pass_SI;

            list = new List<jmdl_IndicatorsSubInd>();
            foreach (jmdl_IndicatorsSubInd x in jmISI)
            {
                if (x.IndicatorID_fk == SelectedIndID)
                {
                    list.Add(x);
                }
            }
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.mdl_subind;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new adpt_SubIndDetailsViewHolder(itemView);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = list[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_SubIndDetailsViewHolder;
            //holder.TextView.Text = items[position];

            string name = null;

            foreach (mdl_SubIndicators x in SI)
            {
                if (x.SubIndicatorID == item.SubIndicatorID_fk)
                {
                    name = x.SubIndicator;
                }
            }

            holder.SubIndicator.Text = name;
        }

        public override int ItemCount => list.Count;


    }

    public class adpt_SubIndDetailsViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView SubIndicator;

        public adpt_SubIndDetailsViewHolder(View itemView) : base(itemView)
        {
            SubIndicator = itemView.FindViewById<TextView>(Resource.Id.txt_subIndicatorName);
        }
    }
}