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
        List<jmdl_IndicatorsSubInd> jmISI;
        int SelectedIndID;

        public adpt_SubIndicators(int SelID,  List<jmdl_IndicatorsSubInd> pass_ISI)
        {
            jmISI = pass_ISI;
            SelectedIndID = SelID;
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
            var item = jmISI[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_SubIndDetailsViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => jmISI.Where(x => x.IndicatorID_fk.Equals(SelectedIndID)).Count();


    }

    public class adpt_SubIndDetailsViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }


        public adpt_SubIndDetailsViewHolder(View itemView) : base(itemView)
        {

        }
    }

    public class adpt_SubIndDetailsClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}