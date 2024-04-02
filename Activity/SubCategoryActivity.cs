using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Adapter;
using PocketAuditor.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor
{
    [Activity(Label = "Select Sub-Category")]
    public class SubCategoryActivity : AppCompatActivity
    {
        public RecyclerView SC_Recycler, UnsortedRecycler;
        private adpt_SubCategories sc_adapter, uns_adapter;
        public int SelectedCatID;
        public List<jmdl_CategoriesSubCategories> _jmCSC;
        public DataSharingService DSS;
        TextView CategoryTitle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_select_subcategory);

            CategoryTitle = FindViewById<TextView>(Resource.Id.CategoryTitle);
            DSS = DataSharingService.GetInstance();

            SelectedCatID = DSS.CSC_SelectedID;
            CategoryTitle.Text = DSS.CSC_SelectedName;
            _jmCSC = DSS.CSC_ListHolder;

            // Create your application here

            SC_Recycler = FindViewById<RecyclerView>(Resource.Id.recView_SubCategories);
            SC_Recycler.SetLayoutManager(new LinearLayoutManager(this));

            UnsortedRecycler = FindViewById<RecyclerView>(Resource.Id.recView_UnsortedIndicators);
            UnsortedRecycler.SetLayoutManager(new LinearLayoutManager(this));

            sc_adapter = new adpt_SubCategories(_jmCSC, SelectedCatID, this);
            SC_Recycler.SetAdapter(sc_adapter);
        }
    }
}