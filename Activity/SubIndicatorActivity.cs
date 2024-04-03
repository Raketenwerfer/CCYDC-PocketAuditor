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
    [Activity(Label = "SubIndicatorActivity")]
    public class SubIndicatorActivity : AppCompatActivity
    {
        public RecyclerView SI_Recycler;
        private adpt_SubIndicators si_adapter;
        TextView IndicatorTitle;
        ImageButton back;

        DataSharingService DSS;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_select_subindicator);

            // Create your application here

            IndicatorTitle = FindViewById<TextView>(Resource.Id.displayIndicatorName);
            DSS = DataSharingService.GetInstance();
            back = FindViewById<ImageButton>(Resource.Id.imgtbn_SI_Back);

            IndicatorTitle.Text = DSS.Get_ISI_Name();
            
            SI_Recycler = FindViewById<RecyclerView>(Resource.Id.recView_SubIndicators);
            SI_Recycler.SetLayoutManager(new LinearLayoutManager(this));

            si_adapter = new adpt_SubIndicators(DSS.Get_ISI_ID(), DSS.ISI_GetList(), DSS.SI_GetList());
            SI_Recycler.SetAdapter(si_adapter);

            back.Click += (sender, e) => { Finish(); };
        }
    }
}