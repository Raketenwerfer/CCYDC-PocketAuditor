using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketAuditor.Adapter
{
    internal class adpt_Categories : RecyclerView.Adapter
    {
        List<mdl_Indicators> indicators;
        List<mdl_Categories> categories;
        adpt_Indicators adapter;
        List<jmdl_CategoriesIndicators> joined_CI, sort_jCI;
        List<jmdl_IndicatorsSubInd> assoc_ISI;
        public int categoryid;

        public adpt_Categories(List<mdl_Categories> adpt_categories, List<mdl_Indicators> adpt_indicators, 
            List<jmdl_IndicatorsSubInd> associate_isi, List<jmdl_CategoriesIndicators> associate_ci)
        {
            indicators = adpt_indicators;
            categories = adpt_categories;
            joined_CI = associate_ci;
            assoc_ISI = associate_isi;

            sort_jCI = new List<jmdl_CategoriesIndicators>();

            adapter = new adpt_Indicators(sort_jCI, associate_isi);
        }

        public void _SortQuery(int _catID)
        {
            // Clear the existing data
            sort_jCI.Clear();

            foreach (jmdl_CategoriesIndicators a in joined_CI)
            {
                if (a.CategoryID == _catID)
                {
                    jmdl_CategoriesIndicators bucket = new jmdl_CategoriesIndicators(a.CategoryID, a.CategoryTitle,
                        a.IndicatorID, a.Indicator, a.IndicatorType, a.ScoreValue);
                    {
                        bucket.CategoryID = a.CategoryID;
                        bucket.CategoryTitle = a.CategoryTitle;
                        bucket.Indicator = a.Indicator;
                        bucket.IndicatorID = a.IndicatorID;
                        bucket.IndicatorNumber = a.IndicatorNumber;
                        bucket.IndicatorType = a.IndicatorType;
                        bucket.ScoreValue = a.ScoreValue;
                    }
                    sort_jCI.Add(bucket);
                }
            }
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.mdl_category;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);


            var vh = new adpt_CategoriesViewHolder(itemView);

            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = categories[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as adpt_CategoriesViewHolder;
            //holder.TextView.Text = items[position];

            categoryid = item.CategoryID;

            holder.CategoryTitle.Text = item.CategoryTitle;

            _SortQuery(categoryid);

            holder.IndicatorRecycler.SetAdapter(adapter);
        }

        public override int ItemCount => categories.Count;
    }

    public class adpt_CategoriesViewHolder : RecyclerView.ViewHolder
    {
        public TextView CategoryTitle;
        public RecyclerView IndicatorRecycler;


        public adpt_CategoriesViewHolder(View itemView) : base(itemView)
        {
            CategoryTitle = itemView.FindViewById<TextView>(Resource.Id.CategoryTitle);
            IndicatorRecycler = itemView.FindViewById<RecyclerView>(Resource.Id.IndicatorRecycler);
            IndicatorRecycler.SetLayoutManager(new LinearLayoutManager(itemView.Context));
        }
    }
}