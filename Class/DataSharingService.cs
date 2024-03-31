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
        public int CSC_SelectedID, ISC_SelectedID, ISI_SelectedID;
        private TextView _progress;
        public List<jmdl_CategoriesSubCategories> CSC_ListHolder;
        public List<jmdl_IndicatorSubCat> ISC_ListHolder;
        public List<jmdl_IndicatorsSubInd> ISI_ListHolder;
        public List<mdl_SubIndicators> SI_ListHolder;


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

        public void SI_SetList(List<mdl_SubIndicators> list)
        {
            SI_ListHolder = list;
        }

        public List<mdl_SubIndicators> SI_GetList()
        {
            return SI_ListHolder;
        }

        public void ISC_SetList(List<jmdl_IndicatorSubCat> list)
        {
            ISC_ListHolder = list;
        }
        public List<jmdl_IndicatorSubCat> ISC_GetList()
        {
            return ISC_ListHolder;
        }

        public void ISI_SetList(List<jmdl_IndicatorsSubInd> list)
        {
            ISI_ListHolder = list;
        }

        public List<jmdl_IndicatorsSubInd> ISI_GetList()
        {
            return ISI_ListHolder;
        }

        public int Get_ISC_ID()
        {
            return ISC_SelectedID;
        }
        public void SET_ISC_ID(int id)
        {
            ISC_SelectedID = id;
        }

        public int Get_ISI_ID()
        {
            return ISI_SelectedID;
        }
        public void SET_ISI_ID(int id)
        {
            ISI_SelectedID = id;
        }
    }
}