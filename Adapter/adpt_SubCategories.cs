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

namespace PocketAuditor.Adapter
{
    internal class adpt_SubCategories : RecyclerView.Adapter
    {

        List<jmdl_CategoriesSubCategories> _jmCSC;
        public int SelectedCatID;

        public adpt_SubCategories(List<jmdl_CategoriesSubCategories> associate_CSC, int selectedCatID)
        {
            SelectedCatID = selectedCatID;
            _jmCSC = associate_CSC;
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
            var item = _jmCSC[position];

            var holder = viewHolder as adpt_SubCategoriesViewHolder;

            holder.SubCatTitle.Text = item.SubCategoryTitle;
        }


        public override int ItemCount => _jmCSC.Where(x => x.CategoryID_fk.Equals(SelectedCatID)).Count();

    }

    public class adpt_SubCategoriesViewHolder : RecyclerView.ViewHolder
    {
        public TextView SubCatTitle;

        public adpt_SubCategoriesViewHolder(View itemView) : base(itemView)
        {
            SubCatTitle = itemView.FindViewById<TextView>(Resource.Id.SubCategoryTitle);
        }
    }
}