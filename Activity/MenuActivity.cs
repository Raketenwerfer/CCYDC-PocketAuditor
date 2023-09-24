using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : AppCompatActivity
    {
        Button beginAudit;
        Button manageAudit;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.menu);

            beginAudit = FindViewById<Button>(Resource.Id.beginAudit);
            manageAudit = FindViewById<Button>(Resource.Id.manageAudit);

            beginAudit.Click += BeginAudit_Click;
            manageAudit.Click += ManageAudit_Click;
        } 

        private void ManageAudit_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ManageActivity));
        }

        private void BeginAudit_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(MainActivity));
        }
    }
}