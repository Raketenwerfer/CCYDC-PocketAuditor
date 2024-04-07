using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using System;
using Pocket_Auditor_Admin_Panel.Classes;
using System.Collections.Generic;
using System.Linq;
using PocketAuditor.Class;
using PocketAuditor.Scores;

namespace PocketAuditor.Adapter
{
    internal class adpt_SubIndicators : RecyclerView.Adapter
    {
        List<mdl_SubIndicators> SI;
        List<jmdl_IndicatorsSubInd> jmISI, list;
        int SelectedIndID;

        DataSharingService DSS;
        ResponseReader RR;

        public adpt_SubIndicators(int SelID,  List<jmdl_IndicatorsSubInd> pass_ISI,
            List<mdl_SubIndicators> pass_SI)
        {
            jmISI = pass_ISI;
            SelectedIndID = SelID;
            SI = pass_SI;

            DSS = DataSharingService.GetInstance();
            RR = ResponseReader.GetInstance();

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
            string type = null;

            foreach (mdl_SubIndicators x in SI)
            {
                if (x.SubIndicatorID == item.SubIndicatorID_fk)
                {
                    name = x.SubIndicator;

                    if (x.SubIndicatorType == "DETAILS")
                    {
                        holder.ETxtDetails.Enabled = true;
                        holder.CBoxSubIndicators.Enabled = false;
                        holder.CBoxSubIndicators.Visibility = ViewStates.Gone;

                        RecordEntry(DSS.ISC_SelectedID.ToString(), SelectedIndID,
                            item.SubIndicatorID_fk.ToString(), "DETAILS");

                        InputValuesHandler(holder, DSS.ISC_SelectedID.ToString(), SelectedIndID,
                            item.SubIndicatorID_fk.ToString(), "GET_REMARKS_VALUE");

                        type = x.SubIndicatorType;
                    }
                    else if (x.SubIndicatorType == "OPTIONS")
                    {
                        holder.ETxtDetails.Enabled = false;
                        holder.CBoxSubIndicators.Enabled = true;
                        holder.ETxtDetails.Visibility = ViewStates.Gone;

                        RecordEntry(DSS.ISC_SelectedID.ToString(), SelectedIndID,
                            item.SubIndicatorID_fk.ToString(), "OPTIONS");

                        InputValuesHandler(holder, DSS.ISC_SelectedID.ToString(), SelectedIndID,
                            item.SubIndicatorID_fk.ToString(), "GET_ISCHECKED_VALUE");

                        type = x.SubIndicatorType;
                    }
                }
            }

            holder.SubIndicator.Text = name;


            holder.ETxtDetails.TextChanged += (sender, e) =>
            {
                InputValuesHandler(holder, DSS.ISC_SelectedID.ToString(), SelectedIndID,
                    item.SubIndicatorID_fk.ToString(), "SET_REMARKS_VALUE");
            };
            holder.CBoxSubIndicators.CheckedChange += (sender, e) =>
            {
                InputValuesHandler(holder, DSS.ISC_SelectedID.ToString(), SelectedIndID,
                    item.SubIndicatorID_fk.ToString(), "SET_ISCHECKED_VALUE");
            };
        }


        public void RecordEntry(string subcatid, int indid, string id, string type)
        {
            var match = RR.Scores.FirstOrDefault(x =>
            x.ChapterID_fk == DSS.GetSelectedChapterID() &&
            x.CategoryID_fk == DSS.CSC_SelectedID &&
            x.SubCategoryID_fk == subcatid &&
            x.IndicatorID_fk == indid &&
            x.SubIndicatorID_fk == id &&
            x.SubIndicatorType == type);

            if (match == null)
            {
                RR.AddResponse(DSS.GetSelectedChapterID(), DSS.CSC_SelectedID, subcatid, indid,
                    id, false, "SUBIND", null, type);
            }
        }

        public void InputValuesHandler(adpt_SubIndDetailsViewHolder holder, string subcatid,
            int indid, string subindid, string operation)
        {
            var match = RR.Scores.FirstOrDefault(x =>
            x.CategoryID_fk == DSS.CSC_SelectedID &&
            x.SubCategoryID_fk == subcatid &&
            x.IndicatorID_fk == indid &&
            x.SubIndicatorID_fk == subindid);

            if (match != null)
            {
                switch (operation)
                {
                    case "SET_ISCHECKED_VALUE":
                        match.IsChecked = holder.CBoxSubIndicators.Checked;
                        break;

                    case "GET_ISCHECKED_VALUE":
                        holder.CBoxSubIndicators.Checked = match.IsChecked;
                        break;

                    case "SET_REMARKS_VALUE":
                        match.Remarks = holder.ETxtDetails.Text;
                        break;

                    case "GET_REMARKS_VALUE":
                        holder.ETxtDetails.Text = match.Remarks;
                        break;
                }
            }
        }

        public override int ItemCount => list.Count;


    }

    public class adpt_SubIndDetailsViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView SubIndicator;
        public EditText ETxtDetails;
        public CheckBox CBoxSubIndicators;

        public adpt_SubIndDetailsViewHolder(View itemView) : base(itemView)
        {
            SubIndicator = itemView.FindViewById<TextView>(Resource.Id.txt_subIndicatorName);
            ETxtDetails = itemView.FindViewById<EditText>(Resource.Id.etxt_si_subIndicatorInput);
            CBoxSubIndicators = itemView.FindViewById<CheckBox>(Resource.Id.cbox_SubIndicator);
        }
    }
}