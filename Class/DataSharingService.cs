using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Auxiliaries;
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
        public int CSC_SelectedID, ISC_SelectedID, ISI_SelectedID, SK_ChapterID;
        public string CSC_SelectedName, ISC_SelectedName, ISI_SelectedName, SK_ChapterName;
        private bool SK_IsFinished;
        private TextView _progress;
        public List<jmdl_CategoriesSubCategories> CSC_ListHolder;
        public List<jmdl_IndicatorSubCat> ISC_ListHolder;
        public List<jmdl_IndicatorsSubInd> ISI_ListHolder;
        public List<mdl_SubIndicators> SI_ListHolder;
        public List<mdl_UnsortedIndicators> USI_ListHolder;
        public List<mdl_Users> U_HOLDER;

        private DatabaseInitiator DBHOlder;


        public DataSharingService()
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

        public void SetSelectedChapter(int id, string name, bool isDone)
        {
            SK_ChapterID = id;
            SK_ChapterName = name;
            SK_IsFinished = isDone;
        }

        public int GetSelectedChapterID()
        {
            return SK_ChapterID;
        }
        public string GetSelectedChapterName()
        {
            return SK_ChapterName;
        }
        public bool GetSelectedChapterStatus()
        {
            return SK_IsFinished;
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


        #region List Setters

        /// Category
        public void CSC_SetList(List<jmdl_CategoriesSubCategories> list, int id, string name)
        {
            CSC_ListHolder = list;
            CSC_SelectedID = id;
            CSC_SelectedName = name;
        }


        public void SI_SetList(List<mdl_SubIndicators> list)
        {
            SI_ListHolder = list;
        }

        public List<mdl_SubIndicators> SI_GetList()
        {
            return SI_ListHolder;
        }

        /// Unsorted Indicators
        public void USI_SetList(List<mdl_UnsortedIndicators> list)
        {
            USI_ListHolder = list;
        }
        public List<mdl_UnsortedIndicators> USI_GetList()
        {
            return USI_ListHolder;
        }

        /// Indicator-SubCategory
        public void ISC_SetList(List<jmdl_IndicatorSubCat> list)
        {
            ISC_ListHolder = list;
        }
        public List<jmdl_IndicatorSubCat> ISC_GetList()
        {
            return ISC_ListHolder;
        }

        public void SET_ISC_ID(int id, string name)
        {
            ISC_SelectedID = id;
            ISC_SelectedName = name;
        }
        public int Get_ISC_ID()
        {
            return ISC_SelectedID;
        }
        public string Get_ISC_Name()
        {
            return ISC_SelectedName;
        }

        /// Indicator-SubIndicator
        public void ISI_SetList(List<jmdl_IndicatorsSubInd> list)
        {
            ISI_ListHolder = list;
        }

        public List<jmdl_IndicatorsSubInd> ISI_GetList()
        {
            return ISI_ListHolder;
        }

        public void SET_ISI_ID(int id, string name)
        {
            ISI_SelectedID = id;
            ISI_SelectedName = name;
        }
        public int Get_ISI_ID()
        {
            return ISI_SelectedID;
        }
        public string Get_ISI_Name()
        {
            return ISI_SelectedName;
        }


        /// Users
        public void SET_U(List<mdl_Users> list)
        {
            U_HOLDER = list;
        }

        public List<mdl_Users> GET_U()
        {
            return U_HOLDER;
        }
        #endregion


        public void SetDatabase(DatabaseInitiator db)
        {
            DBHOlder = db;
        }
        
        public DatabaseInitiator GetDatabase()
        {
            return DBHOlder;
        }
    }
}