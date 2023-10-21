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
    [Activity(Label = "ActionPlanActivity")]
    public class ActionPlanActivity : AppCompatActivity 
    {
        ImageView AddNewPlan;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.action_plans);

            AddNewPlan = FindViewById<ImageView>(Resource.Id.addnewPlan);

            AddNewPlan.Click += AddNewPlan_Click; 
        }

        private void AddNewPlan_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AddActionPlanActivity));
            StartActivity(intent);
        }
    }
}