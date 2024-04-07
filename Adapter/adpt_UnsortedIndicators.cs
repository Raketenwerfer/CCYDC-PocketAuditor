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
using PocketAuditor.Scores;
using static Android.Content.ClipData;

namespace PocketAuditor.Adapter
{
    internal class adpt_UnsortedIndicators : RecyclerView.Adapter
    {
        DataSharingService DSS;
        ResponseReader RR;

        List<jmdl_CategoriesIndicators> indicators = new List<jmdl_CategoriesIndicators>();
        List<jmdl_IndicatorsSubInd> jmISI;
        List<jmdl_IndicatorSubCat> jmISC;
        List<mdl_UnsortedIndicators> mUSI, list;
        Context context;

        public int SelectedCatID;

        public adpt_UnsortedIndicators(int selSCatID, Context pcon)
        {

            DSS = DataSharingService.GetInstance();
            RR = ResponseReader.GetInstance();
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

            var vh = new adpt_UnsortedIndicatorsViewHolder(itemView);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = list[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_UnsortedIndicatorsViewHolder;
            //holder.TextView.Text = items[position];

            int amount = jmISI.Where(x => x.IndicatorID_fk.Equals(item.IndicatorID)).Count();


            RecordEntry(item.IndicatorID);


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
                CheckboxValueHandler(holder, SelectedCatID, null, item.IndicatorID, null, "GET");
            }

            holder.IndicatorCBox.CheckedChange += (sender, e) =>
            {
                CheckboxValueHandler(holder, SelectedCatID, null, item.IndicatorID, null, "SET");
            };
        }

        public void RecordEntry(int id)
        {
            var match = RR.Scores.FirstOrDefault(x =>
            x.CategoryID_fk == SelectedCatID &&
            x.SubCategoryID_fk == null &&
            x.IndicatorID_fk == id &&
            x.SubIndicatorID_fk == null);

            if (match == null)
            {
                RR.AddResponse(DSS.GetSelectedChapterID(), SelectedCatID, null, id,
                    null, false, "IND", null, null);
            }
        }

        public void CheckboxValueHandler(adpt_UnsortedIndicatorsViewHolder holder, int catid, string subcatid,
            int indid, string subindid, string operation)
        {
            var match = RR.Scores.FirstOrDefault(x =>
            x.CategoryID_fk == catid &&
            x.SubCategoryID_fk == subcatid &&
            x.IndicatorID_fk == indid &&
            x.SubIndicatorID_fk == subindid);

            if (match != null)
            {
                if (operation == "SET")
                {
                    match.IsChecked = holder.IndicatorCBox.Checked;
                }
                else if (operation == "GET")
                {
                    holder.IndicatorCBox.Checked = match.IsChecked;
                }
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