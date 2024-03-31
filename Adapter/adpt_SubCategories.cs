using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pocket_Auditor_Admin_Panel.Classes;
using AndroidX.RecyclerView.Widget;
using AndroidX.CardView.Widget;
using PocketAuditor.Activity;
using PocketAuditor.Class;

namespace PocketAuditor.Adapter
{
    internal class adpt_SubCategories : RecyclerView.Adapter
    {

        public List<jmdl_CategoriesSubCategories> _jmCSC, list;
        public List<jmdl_IndicatorSubCat> _jmISC;
        public int SelectedCatID, SelectedSubCatID;
        DataSharingService DSS;
        Context context;

        public adpt_SubCategories(List<jmdl_CategoriesSubCategories> associate_CSC, int selectedCatID,
            Context pasS_context)
        {
            SelectedCatID = selectedCatID;
            _jmCSC = associate_CSC;
            this.context = pasS_context;
            DSS = DataSharingService.GetInstance();

            _jmISC = DSS.ISC_ListHolder;


            list = new List<jmdl_CategoriesSubCategories>();
            foreach (jmdl_CategoriesSubCategories x in _jmCSC)
            {
                if (x.CategoryID_fk == SelectedCatID)
                {
                    list.Add(x);
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = null;
            var id = Resource.Layout.mdl_subcategory;
            itemView = LayoutInflater.From(parent.Context).
                Inflate(id, parent, false);

            var vh = new adpt_SubCategoriesViewHolder(itemView);

            return vh;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = list[position];

            var holder = viewHolder as adpt_SubCategoriesViewHolder;

            SelectedSubCatID = item.SubCategoryID_fk;

            holder.SubCatTitle.Text = item.SubCategoryTitle;
            holder.SubCatCard.Click += (sender, e) => { SelectSubCategory(item.SubCategoryID_fk, item.SubCategoryTitle); };
        }

        public void SelectSubCategory(int id, string name)
        {
            DSS.SET_ISC_ID(id);
            DSS.ISC_SetList(_jmISC);
            Intent intent = new Intent(context, typeof(IndicatorActivity));
            context.StartActivity(intent);

            Toast.MakeText(context, id.ToString() + " " + name, ToastLength.Short).Show();
        }

        public override int ItemCount => list.Count;

    }

    public class adpt_SubCategoriesViewHolder : RecyclerView.ViewHolder
    {
        public TextView SubCatTitle;
        public CardView SubCatCard;

        public adpt_SubCategoriesViewHolder(View itemView) : base(itemView)
        {
            SubCatTitle = itemView.FindViewById<TextView>(Resource.Id.SubCategoryTitle);
            SubCatCard = itemView.FindViewById<CardView>(Resource.Id.cv_SubCategoryCard);
        }
    }
}