using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Class
{
    public class DataSharingService
    {
        private static DataSharingService instance;
        private int _interactions, _itemcount;
        public int CSC_SelectedID;
        private TextView _progress;
        public List<jmdl_CategoriesSubCategories> CSC_ListHolder;


        private DataSharingService()
        {
            _interactions = 0;
            _itemcount = 0;
        }

        public static DataSharingService GetInstance()
        {
            if (instance == null)
            {
                instance = new DataSharingService();
            }     

            return instance;
        }

        public int GetInteractions()
        {
            return _interactions;
        }

        public void SetInteractions(int count)
        {
            _interactions = count;
        }

        public int GetItemCount()
        {
            return _itemcount;
        }

        public void SetItemCount(int count)
        {
            _itemcount = count;
        }

        public void SetProgress(TextView progress)
        {
            _progress = progress;
        }

        public void UpdateProgress()
        {
            _progress.Enabled = true;
            _progress.Text = "Indicators Answered:  " + _interactions + "/" + _itemcount;
        }

        public void CSC_SetList(List<jmdl_CategoriesSubCategories> list, int id)
        {
            CSC_ListHolder = list;
            CSC_SelectedID = id;
        }
    }
}