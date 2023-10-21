using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Activity
{
    [Activity(Label = "AddActionPlanActivity")]
    public class AddActionPlanActivity : AppCompatActivity
    {
        Button cancelPlan;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.add_action_plans);

            cancelPlan = FindViewById<Button>(Resource.Id.cancelPlanBtn);

            cancelPlan.Click += CancelPlan_Click; 
        }

        private void CancelPlan_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ActionPlanActivity));
            StartActivity(intent);
        }
    }
}