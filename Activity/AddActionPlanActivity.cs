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
        EditText planName, planCateName, planCateDesc, planPasteLink;
        Button addPlan, cancelPlan;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.add_action_plans);

            planName = FindViewById<EditText>(Resource.Id.planName);
            planCateName = FindViewById<EditText>(Resource.Id.plan_CatName);
            planCateDesc = FindViewById<EditText>(Resource.Id.plan_CatDescription);
            planPasteLink = FindViewById<EditText>(Resource.Id.plan_PasteLink);

            addPlan = FindViewById<Button>(Resource.Id.addPlanBtn);
            cancelPlan = FindViewById<Button>(Resource.Id.cancelPlanBtn);
             
            addPlan.Click += AddPlan_Click;
            cancelPlan.Click += CancelPlan_Click; 
        }

        private void AddPlan_Click(object sender, EventArgs e)
        {
            string Name = planName.Text; 
            string cateName = planCateName.Text;
            string cateDisc = planCateDesc.Text;
            string pasteLink = planPasteLink.Text;

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(cateName) ||
                string.IsNullOrWhiteSpace(cateDisc) || string.IsNullOrWhiteSpace(pasteLink))
            {
                Toast.MakeText(this, "Fields must be Filled", ToastLength.Short).Show();
            }
        }

        private void CancelPlan_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ActionPlanActivity));
            StartActivity(intent);
        }
    }
}