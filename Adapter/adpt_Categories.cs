using AndroidX.RecyclerView.Widget;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using AndroidX.CardView.Widget;
using PocketAuditor.Class;
using Android.Content;

namespace PocketAuditor.Adapter
{
    internal class adpt_Categories : RecyclerView.Adapter
    {
        DataSharingService DSS;

        List<mdl_Indicators> indicators;
        List<mdl_Categories> categories;
        List<jmdl_CategoriesSubCategories> jm_CSC;
        List<jmdl_IndicatorSubCat> _jm_ISC;
        List<mdl_SubCategories> _subcategories;
        adpt_Indicators adapter;
        List<jmdl_CategoriesIndicators> joined_CI, sort_jCI;
        List<jmdl_IndicatorsSubInd> assoc_ISI;
        public int categoryid;
        Context context;
        SubCategoryActivity display_SCA;

        public adpt_Categories(List<mdl_Categories> adpt_categories, List<mdl_Indicators> adpt_indicators, 
            List<jmdl_IndicatorsSubInd> associate_isi, List<jmdl_CategoriesIndicators> associate_ci,
            List<jmdl_CategoriesSubCategories> associate_csc, Context pass)
        {
            indicators = adpt_indicators;
            categories = adpt_categories;
            joined_CI = associate_ci;
            assoc_ISI = associate_isi;
            jm_CSC = associate_csc;

            context = pass;

            sort_jCI = new List<jmdl_CategoriesIndicators>();

            //adapter = new adpt_Indicators(sort_jCI, associate_isi);

            DSS = DataSharingService.GetInstance();
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

            //holder.IndicatorRecycler.SetAdapter(adapter);

            holder.cv_categoryItem.Click += (sender, e) => { SelectCategory(item.CategoryID, jm_CSC,
                item.CategoryTitle); };
        }

        public override int ItemCount => categories.Count;

        public void SelectCategory(int selID, List<jmdl_CategoriesSubCategories> pass_jmCSC, string name)
        {
            DSS.CSC_SetList(pass_jmCSC, selID, name);
            Intent intent = new Intent(context, typeof(SubCategoryActivity));
            context.StartActivity(intent);

            Toast.MakeText(context, selID.ToString() + " " + name, ToastLength.Short).Show();
        }
    }


    public class adpt_CategoriesViewHolder : RecyclerView.ViewHolder
    {
        public TextView CategoryTitle;
        //public RecyclerView IndicatorRecycler;
        public CardView cv_categoryItem;


        public adpt_CategoriesViewHolder(View itemView) : base(itemView)
        {
            CategoryTitle = itemView.FindViewById<TextView>(Resource.Id.CategoryTitle);
            //IndicatorRecycler.SetLayoutManager(new LinearLayoutManager(itemView.Context));

            cv_categoryItem = itemView.FindViewById<CardView>(Resource.Id.cv_CategoryCard);
        }
    }
}