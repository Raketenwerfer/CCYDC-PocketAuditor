using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using PocketAuditor.Activity;
using PocketAuditor.Fragment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor
{
    [Activity(Label = "ManageMenu")]
    public class ManageMenu : AppCompatActivity
    {
        //ImageView ToMainMenu;
        Button EditAP, EditCQ;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.manage_audit_menu);

            EditAP = FindViewById<Button>(Resource.Id.editAP);
            EditCQ = FindViewById<Button>(Resource.Id.editCQ);
            //ToMainMenu = FindViewById<ImageView>(Resource.Id.toMenu);

            EditAP.Click += StartEditAP;
            EditCQ.Click += StartEditCQ;
            //ToMainMenu.Click += ToMainMenu_Click; 
        }

        //private void ToMainMenu_Click(object sender, EventArgs e)
        //{
        //    Intent intent = new Intent(this, typeof(MenuActivity));
        //    StartActivity(intent);
        //}

        private void StartEditAP(object sender, EventArgs e)
        {
            StartActivity(typeof (ActionPlanActivity));

            Toast.MakeText(Application.Context, "Open Action Plans Here", ToastLength.Short).Show();
        }

        private void StartEditCQ(object sender, EventArgs e)
        {
            StartActivity(typeof(ManageCQ));
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(MenuActivity));
            Finish();
        }
    }
}