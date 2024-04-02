using Android.App;
using Android.Database.Sqlite;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Adapter;
using PocketAuditor.Class;
using System.Collections.Generic;

namespace PocketAuditor.Activity
{
    [Activity(Label = "Select Indicators")]
    public class IndicatorActivity : AppCompatActivity
    {
        public RecyclerView Ind_Recycler;
        private adpt_Indicators isc_adapter;
        public DataSharingService DSS;
        TextView SubCategoryTitle;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_select_indicator);
            // Create your application here

            SubCategoryTitle = FindViewById<TextView>(Resource.Id.SubCategoryTitle);
            DSS = DataSharingService.GetInstance();

            SubCategoryTitle.Text = DSS.Get_ISC_Name();
            Ind_Recycler = FindViewById<RecyclerView>(Resource.Id.recView_Indicators);
            Ind_Recycler.SetLayoutManager(new LinearLayoutManager(this));

            isc_adapter = new adpt_Indicators(DSS.Get_ISC_ID(), this, DSS.ISI_GetList(),
                DSS.ISC_GetList());
            Ind_Recycler.SetAdapter(isc_adapter);
        }
    }
}