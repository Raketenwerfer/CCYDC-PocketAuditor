using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using MySql.Data.MySqlClient;
using Pocket_Auditor_Admin_Panel.Auxiliaries;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Adapter;
using PocketAuditor.Class;
using PocketAuditor.Database;
using PocketAuditor.Fragment;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketAuditor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class AuditActivity : AppCompatActivity
    {
        private RecyclerView recycler;
        private ItemAdapter adapter;

        public List<ItemModel> itemsList = new List<ItemModel>();
        public List<EntryAnswersModel> answersList = new List<EntryAnswersModel>();




        #region New Code Block


        public List<mdl_Categories> mdl_Categories = new List<mdl_Categories>();


        #endregion






        //public DB_Initiator handler;
        //public SQLiteDatabase SQLDB;

        DatabaseInitiator dbInit = new DatabaseInitiator("sql207.infinityfree.com", "if0_35394751_testrun", "if0_35394751", "aTbs7LJAy0B2");

        Button next;
        public ImageView returnMenu;
        public TextView audit_progress;

        private int selectedItemsCount = 0;

        public int _totalInteractions { get; set; }
        public int _totalItems { get; set; }
        public int _totalTrueSelections { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            recycler = FindViewById<RecyclerView>(Resource.Id.recycler);
            recycler.SetLayoutManager(new LinearLayoutManager(this));

            next = FindViewById<Button>(Resource.Id.next);
            next.Click += Next_Click;

            audit_progress = FindViewById<TextView>(Resource.Id.audit_progress);

            // Initialize the database and establishes a connection string
            //handler = new DB_Initiator(this); OLD DB
            //SQLDB = handler.WritableDatabase; OLD DB


            dbInit.GetConnection();

            DisplayData();

            // Create adapter and set it to RecyclerView
            adapter = new ItemAdapter(itemsList);
            recycler.SetAdapter(adapter);

            // This line of code will erase all entries in the EntryAnswers_tbl table
            // This is done so it can be reused for new audits. Will be moved elsewhere
            // Once other functionalities are done

            //SQLDB.RawQuery("DELETE FROM EntryAnswers_tbl", null);

            audit_progress.Enabled = false;

            DataSharingService dss = DataSharingService.GetInstance();
            dss.SetProgress(audit_progress);
        }

        public override void OnBackPressed()
        {
            StartActivity(typeof(MenuActivity));
            Finish();
        }



        private void DisplayData()
        {
            // Queries the questions to display as cardviews. Cell values are stored in ItemModel

            string q_EntryID = null, q_CatID = null, q_Question = null, q_Remarks, qC_Title = null;
            string query = "SELECT E.Indicator, E.IndicatorID, E.IndicatorStatus, " +
                                  "C.Category_ID, C.CategoryTitle " +
                            "FROM Indicators E INNER JOIN Category_tbl C " +
                            "ON E.CategoryID = C.Category_ID " +
                            "WHERE IndicatorStatus = 'ACTIVE' " +
                            "ORDER BY QuesNo ASC";

            ICursor showItems = SQLDB.RawQuery(query, new string[] { });

            if (showItems.Count > 0)
            {
                showItems.MoveToFirst();

                do 
                {
                    q_CatID = showItems.GetString(showItems.GetColumnIndex("Category_ID"));
                    qC_Title = showItems.GetString(showItems.GetColumnIndex("CategoryTitle"));
                    q_EntryID = showItems.GetString(showItems.GetColumnIndex("IndicatorID"));
                    q_Question = showItems.GetString(showItems.GetColumnIndex("Indicator"));
                    q_Remarks = null;

                    ItemModel a = new ItemModel(q_CatID, qC_Title, q_EntryID, q_Question, null, "no", "empty")
                    {
                        IndicatorID = q_EntryID,
                        CategoryTitle = qC_Title,
                        EntryQuestion = q_Question,
                        Remark = q_Remarks
                    };

                    itemsList.Add(a);
                }
                while (showItems.MoveToNext());

                showItems.Close();
            }
        }




        private void Next_Click(object sender, EventArgs e)
        {
            _totalInteractions = itemsList.Where(a => a.btnIsInteracted.Equals("yes")).Count();
            _totalTrueSelections = itemsList.Where(a => a.isTrue.Equals("true")).Count();
            _totalItems = itemsList.Count();


            if (_totalInteractions == _totalItems)
            {

                Toast.MakeText
                (Application.Context,
                "Interacted Items: " + itemsList.Where(a => a.btnIsInteracted.Equals("yes")).Count().ToString() + "\n" +
                "YesIsSelected Items: " + itemsList.Where(a => a.isTrue.Equals("true")).Count().ToString(),
                ToastLength.Short).Show();

                // Testing passing data. Will be replaced with SQL queries soon
                Intent FinishView = new Intent(this, typeof(AuditResults));

                FinishView.PutExtra("p1", Convert.ToDouble(_totalItems));
                FinishView.PutExtra("p2", Convert.ToDouble(_totalTrueSelections));

                ClearTable();
                InsertData();
                StartActivity(FinishView);
            }
            else
            {
                Toast.MakeText(Application.Context, "There are still " + (_totalItems - _totalInteractions).ToString() +
                    " indicators left unanswered!", ToastLength.Long).Show();
            }
        }

        private void ClearTable()
        {
            var _db = new SQLiteConnection(handler._ConnPath);
            string delQuery = $"DELETE FROM EntryAnswers_tbl";

            _db.Execute(delQuery);
        }

        private void InsertData()
        {
            var _db = new SQLiteConnection(handler._ConnPath);

            foreach (ItemModel model in itemsList)
            {
                _db.Execute("INSERT INTO EntryAnswers_tbl(fk_CatID, fk_EntryID, EntryAnswer, EntryRemark)" +
                                "VALUES (?, ?, ?, ?)", model.CatID, model.IndicatorID, model.isTrue, model.Remark);
            }

            _db.Close();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}