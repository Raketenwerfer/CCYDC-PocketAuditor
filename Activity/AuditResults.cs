using Android.App;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using PocketAuditor.Class;
using PocketAuditor.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "AuditResults", MainLauncher =false)]
    public class AuditResults : AppCompatActivity
    {
        TextView ScoreDisplay, ActionPlanDisplay, QuestionItemsDisplay;
        Spinner cateDisplay;

        private readonly List<EntryAnswersModel> answersList = new List<EntryAnswersModel>();
        private readonly List<string> ar_Category = new List<string>();
        SQLiteDatabase SQLDB;
        DB_Initiator handler;

        double totalItems, totalScore;
        double percentage;
        readonly double passing_grade = 60;

        readonly private int uniqueCategories;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.audit_results);

            ScoreDisplay = FindViewById<TextView>(Resource.Id.Score_Display);
            ActionPlanDisplay = FindViewById<TextView>(Resource.Id.AP_Display);
            QuestionItemsDisplay = FindViewById<TextView>(Resource.Id.QI_Display);
            cateDisplay = FindViewById<Spinner>(Resource.Id.categories);

            // Initialize the database and establishes a connection string
            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;

            RetrieveAnswers();
            CalculateScores();
            SelectActionPlan();
            PullRemarks();

            PopulateCategoriesSpinner(); //this line to populate the spinner
        }

        private void PopulateCategoriesSpinner()
        {
            foreach (EntryAnswersModel EAM in answersList)
            {
                if (!ar_Category.Exists(i => i == EAM.CategoryName))
                {
                    ar_Category.Add(EAM.CategoryName);
                }
            }

            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, ar_Category.Select(a => a).ToList());
            cateDisplay.Adapter = adapter;
        }

        private void PullRemarks()
        {
            PopulateCategoriesSpinner();

            //Pull Remarks from the database via queries
            QuestionItemsDisplay.Text = "Pull Remarks from the Database here!";
        }

        private void SelectActionPlan()
        {
            if (percentage > passing_grade)
            {
                ActionPlanDisplay.Text = "Score is above passing grade. No action plans recommended";
            }
            else
            {
                ActionPlanDisplay.Text = "Score is below passing grade! Recommend a action plans here!";
            }
        }

        private void CalculateScores()
        {
            totalScore = Convert.ToDouble(answersList.Count(a => a.EntryAnswer.Equals("true")));
            totalItems = Convert.ToDouble(answersList.Count());

            string result;
            result = ((totalScore / totalItems) * 100).ToString();
            percentage = Convert.ToDouble(result);
            ScoreDisplay.Text = "Category 1: " + totalScore + "/" + totalItems + "(" + result + "%)";

        }

        private void RetrieveAnswers()
        {
            int __pkID, __fkID, __fkCID;
            string __answers, __remarks, __ctitle;

            string query = "SELECT A.AnswerID, A.fk_EntryID, A.fk_CatID, A.EntryAnswer, A.EntryRemark," +
                " C.CategoryTitle FROM EntryAnswers_tbl A INNER JOIN Category_tbl C ON A.fk_CatID = C.Category_ID" +
                " ORDER BY Category_ID ASC";
            ICursor ra = SQLDB.RawQuery(query, new string[] { });


            if (ra != null)
            {
                ra.MoveToFirst();

                do
                {
                    __pkID = ra.GetInt(ra.GetColumnIndex("AnswerID"));
                    __fkID = ra.GetInt(ra.GetColumnIndex("fk_EntryID"));
                    __fkCID = ra.GetInt(ra.GetColumnIndex("fk_CatID"));
                    __answers = ra.GetString(ra.GetColumnIndex("EntryAnswer"));
                    __remarks = ra.GetString(ra.GetColumnIndex("EntryRemark"));
                    __ctitle = ra.GetString(ra.GetColumnIndex("CategoryTitle"));

                    EntryAnswersModel b = new EntryAnswersModel(__pkID, __fkID, __fkCID, __answers, __remarks, __ctitle);

                    b.AnswerID = __pkID;
                    b.fk_EntryID = __fkID;
                    b.CategoryID = __fkCID;
                    b.EntryAnswer = __answers;
                    b.EntryRemark = __remarks;
                    b.CategoryName = __ctitle;

                    answersList.Add(b);
                }
                while (ra.MoveToNext());
                ra.Close();
            }
        }
    }
}