using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using PocketAuditor.Adapter;
using PocketAuditor.Class;
using PocketAuditor.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Activity
{
    [Activity(Label = "QuestionActivity")]
    public class QuestionActivity : AppCompatActivity
    {
        private RecyclerView mQ_Recycler;
        //private QuestionAdapter ques_Adapter;

        public List<QuestionModel> ques_List = new List<QuestionModel>();

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.question_model);

            mQ_Recycler = FindViewById<RecyclerView>(Resource.Id.Questionrecycler);
            mQ_Recycler.SetLayoutManager(new LinearLayoutManager(this));

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;

            //ques_Adapter = new QuestionAdapter(ques_List, handler);
            //mQ_Recycler.SetAdapter(ques_Adapter);

            //DisplayQuestionByCategoryClicked();
        }


        //private void DisplayQuestionByCategoryClicked()
        //{
        //    string selectedCategoryName = "Category Title"; // Replace with the actual selected category name

        //    // Clear the ques_List before populating it with the new data
        //    ques_List.Clear();

        //    string query = "SELECT E.Indicator, E.EntryID, C.Category_ID, C.CategoryTitle " +
        //        "FROM Entry_tbl E INNER JOIN Category_tbl C ON E.CategoryID = C.Category_ID " +
        //        "WHERE C.CategoryTitle = ? ORDER BY QuesNo ASC";

        //    ICursor showItems = SQLDB.RawQuery(query, new string[] { selectedCategoryName });

        //    if (showItems.Count > 0)
        //    {
        //        showItems.MoveToFirst();

        //        do
        //        {
        //            string q_EntryID = showItems.GetString(showItems.GetColumnIndex("EntryID"));
        //            string q_Question = showItems.GetString(showItems.GetColumnIndex("Indicator"));

        //            // Create a QuestionModel without q_Remarks
        //            QuestionModel question = new QuestionModel(q_EntryID, selectedCategoryName, q_Question)
        //            {
        //                EntryID = q_EntryID,
        //                Indicator = q_Question
        //            };

        //            ques_List.Add(question);
        //        }
        //        while (showItems.MoveToNext());

        //        showItems.Close();
        //    }

        //    // Notify the adapter that the data has changed
        //    ques_Adapter.NotifyDataSetChanged();
        //}

    }
}