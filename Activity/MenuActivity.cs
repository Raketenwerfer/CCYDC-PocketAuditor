using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.CardView.Widget;
using System;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : AppCompatActivity
    {
        ImageButton beginAudit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.menu);

            beginAudit = FindViewById<ImageButton>(Resource.Id.BeginAuditButton);

            beginAudit.Click += BeginAudit_Click;
        }

        private void BeginAudit_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AuditActivity));
        }
    }
}