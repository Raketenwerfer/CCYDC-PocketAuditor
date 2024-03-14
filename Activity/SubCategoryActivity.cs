using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Activity
{
    [Activity(Label = "SubCategoryActivity")]
    public class SubCategoryActivity : AppCompatActivity
    {
        RecyclerView SC_Recycler;
        public int SelectedCatID;
        public List<jmdl_CategoriesSubCategories> _jmCSC;

        public SubCategoryActivity(List<jmdl_CategoriesSubCategories> bucket_jmCSC, int _categoryID)
        {
            _jmCSC = bucket_jmCSC;
            SelectedCatID = _categoryID;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }
    }
}