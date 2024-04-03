using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using System;
using Pocket_Auditor_Admin_Panel.Classes;
using System.Collections.Generic;
using System.Linq;
using PocketAuditor.Class;
using AndroidX.CardView.Widget;
using Android.Content;
using PocketAuditor.Activity;

namespace PocketAuditor.Adapter
{
    internal class adpt_UnsortedIndicators : RecyclerView.Adapter
    {
        DataSharingService DSS;

        List<jmdl_CategoriesIndicators> indicators = new List<jmdl_CategoriesIndicators>();
        List<jmdl_IndicatorsSubInd> jmISI;
        List<jmdl_IndicatorSubCat> jmISC;
        List<mdl_UnsortedIndicators> mUSI, list;
        Context context;

        public int SelectedCatID;

        public adpt_UnsortedIndicators(int selSCatID, Context pcon)
        {

            DSS = DataSharingService.GetInstance();

            SelectedCatID = selSCatID;
            context = pcon;
            jmISI = DSS.ISI_GetList();
            mUSI = DSS.USI_GetList();
            jmISC = DSS.ISC_GetList();

            list = new List<mdl_UnsortedIndicators>();

            foreach (mdl_UnsortedIndicators x in mUSI)
            {
                if (x.CategoryID == SelectedCatID)
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
            var id = Resource.Layout.mdl_indicator;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new adpt_IndicatorsViewHolder(itemView);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = list[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_IndicatorsViewHolder;
            //holder.TextView.Text = items[position];

            int amount = jmISI.Where(x => x.IndicatorID_fk.Equals(item.IndicatorID)).Count();

            holder.IndicatorTitle.Text = item.Indicator;

            if (amount > 0)
            {
                holder.SubIndicatorAmount.Text = ">> " + amount.ToString() + " Sub-Indicator/s Available";
                holder.IndicatorCBox.Enabled = false;
                holder.IndicatorCBox.Visibility = ViewStates.Invisible;
                holder.Card.Click += (sender, e) => { SelectIndicator(item.IndicatorID, item.Indicator); };
            }
            else
            {
                holder.SubIndicatorAmount.Text = ">> No Sub-Indicators Available";
                holder.IndicatorCBox.Enabled = true;
                holder.Card.Clickable = false;
            }
        }

        public override int ItemCount => list.Count;

        public void SelectIndicator(int id, string name)
        {
            DSS.SET_ISI_ID(id, name);
            Intent intent = new Intent(context, typeof(SubIndicatorActivity));
            context.StartActivity(intent);

            Toast.MakeText(context, id.ToString(), ToastLength.Short).Show();
        }
    }

    public class adpt_UnsortedIndicatorsViewHolder : RecyclerView.ViewHolder
    {
        public TextView IndicatorTitle;
        public TextView SubIndicatorAmount;
        public CheckBox IndicatorCBox;
        public CardView Card;


        public adpt_UnsortedIndicatorsViewHolder(View itemView) : base(itemView)
        {
            IndicatorTitle = itemView.FindViewById<TextView>(Resource.Id.txt_indicatorTitle);
            SubIndicatorAmount = itemView.FindViewById<TextView>(Resource.Id.txt_subIndicatorAmount);
            Card = itemView.FindViewById<CardView>(Resource.Id.cv_IndicatorCard);
            IndicatorCBox = itemView.FindViewById<CheckBox>(Resource.Id.cbox_Indicator);
        }
    }
}