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
    public class CategoryRemarksModel
    {
        public CategoryRemarksModel(int indicatorId, string selectedCategoryRemarks)
        {
            IndicatorId = indicatorId;
            SelectedCategoryRemarks = selectedCategoryRemarks;
        }

        public int IndicatorId { get; set; }
        public string SelectedCategoryRemarks { get; set; }

    }
}