﻿using AndroidX.RecyclerView.Widget;
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
    internal class adpt_Indicators : RecyclerView.Adapter
    {
        DataSharingService DSS;

        List<jmdl_CategoriesIndicators> indicators = new List<jmdl_CategoriesIndicators>();
        List<jmdl_IndicatorsSubInd> jmISI;
        List<jmdl_IndicatorSubCat> jmISC;
        Context context;

        public int SelectedSubCatID;

        public adpt_Indicators(int selSCatID, Context pcon,
            List<jmdl_IndicatorsSubInd> pass_ISI, List<jmdl_IndicatorSubCat> pass_ISC)
        {

            DSS = DataSharingService.GetInstance();
            SelectedSubCatID = selSCatID;
            context = pcon;
            jmISC = pass_ISC;
            jmISI = pass_ISI;
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
            var item = jmISC[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_IndicatorsViewHolder;
            //holder.TextView.Text = items[position];


            holder.IndicatorTitle.Text = item.Indicator;

            //if (item.IndicatorType == "COMPOSITE")
            //{
            //    int count = 0;

            //    count = jmISI.Where(a => a.IndicatorID_fk.Equals(item.IndicatorID)).Count();

            //    holder.SubIndicatorAmount.Visibility = ViewStates.Visible;
            //    holder.SubIndicatorAmount.Text = count.ToString();
            //}
            //else
            //{
            //    holder.SubIndicatorAmount.Visibility = ViewStates.Gone;
            //}

            holder.Card.Click += (sender, e) => { SelectIndicator(item.IndicatorID_fk); };
        }

        public override int ItemCount => jmISC.Where(x => x.SubCategoryID_fk.Equals(SelectedSubCatID)).Count();

        public void SelectIndicator(int id)
        {
            DSS.SET_ISI_ID(id);
            Intent intent = new Intent(context, typeof(SubIndicatorActivity));
            context.StartActivity(intent);

            Toast.MakeText(context, id.ToString(), ToastLength.Short).Show();
        }
    }

    public class adpt_IndicatorsViewHolder : RecyclerView.ViewHolder
    {
        public TextView IndicatorTitle;
        public TextView SubIndicatorAmount;
        public ImageView IndicatorAnswerStatus;
        public CardView Card;


        public adpt_IndicatorsViewHolder(View itemView) : base(itemView)
        {
            IndicatorTitle = itemView.FindViewById<TextView>(Resource.Id.txt_indicatorTitle);
            SubIndicatorAmount = itemView.FindViewById<TextView>(Resource.Id.txt_subIndicatorAmount);
            Card = itemView.FindViewById<CardView>(Resource.Id.cv_IndicatorCard);
        }
    }
}