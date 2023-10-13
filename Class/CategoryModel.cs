using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Class
{
    public class CategoryModel
    {
        public int CategoryID { get; set; }
        public string CategoryTitle { get; set; }

        public CategoryModel(int cat_id, string cat_title)
        {
            CategoryID = cat_id;
            CategoryTitle = cat_title;
        }
    }
}