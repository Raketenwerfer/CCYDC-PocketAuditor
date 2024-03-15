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
        public RecyclerView SC_Recycler;
        private adpt_SubCategories sc_adapter;
        public int SelectedCatID;
        public List<jmdl_CategoriesSubCategories> _jmCSC;
        public DataSharingService DSS;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_select_subcategory);

            DSS = DataSharingService.GetInstance();

            SelectedCatID = DSS.CSC_SelectedID;
            _jmCSC = DSS.CSC_ListHolder;

            // Create your application here

            SC_Recycler = FindViewById<RecyclerView>(Resource.Id.recView_SubCategories);
            SC_Recycler.SetLayoutManager(new LinearLayoutManager(this));

            sc_adapter = new adpt_SubCategories(_jmCSC, SelectedCatID);
            SC_Recycler.SetAdapter(sc_adapter);
        }
    }
}