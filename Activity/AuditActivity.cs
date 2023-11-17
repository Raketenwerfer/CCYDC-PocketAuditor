using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using MySqlConnector;
using Pocket_Auditor_Admin_Panel.Auxiliaries;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Adapter;
using PocketAuditor.Class;
using PocketAuditor.Fragment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketAuditor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class AuditActivity : AppCompatActivity
    {
        private RecyclerView recycler;
        private adpt_Categories adapter;

        #region New Code Block


        public List<mdl_Categories> _Categories = new List<mdl_Categories>();

        DatabaseInitiator dbInit = new DatabaseInitiator("sql.freedb.tech", "3306", "freedb_ccydc_test_db", "freedb_ccydc", "r*kmjEa6N#KUsDN");

        #endregion

        Button next;
        public ImageView returnMenu;
        public TextView audit_progress;

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
            //next.Click += Next_Click;

            audit_progress = FindViewById<TextView>(Resource.Id.audit_progress);

            // Initialize the database and establishes a connection string
            //handler = new DB_Initiator(this); OLD DB
            //SQLDB = handler.WritableDatabase; OLD DB

            DisplayData();
            
            // Create adapter and set it to RecyclerView
            adapter = new adpt_Categories(_Categories);
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
            int _catID;
            string _catTitle, _catStatus;

            string getCatQuery = "SELECT * FROM Categories WHERE CategoryStatus = 'ACTIVE'";

            MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(getCatQuery, conn))
                {
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            _catID = read.GetInt32(read.GetOrdinal("CategoryID"));
                            _catTitle = read.GetString(read.GetOrdinal("CategoryTitle"));
                            _catStatus = read.GetString(read.GetOrdinal("CategoryStatus"));

                            mdl_Categories a = new mdl_Categories(_catID, _catTitle, _catStatus);
                            {
                                a.CategoryID = _catID;
                                a.CategoryTitle = _catTitle;
                                a.CategoryStatus = _catStatus;
                            }

                            _Categories.Add(a);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
            }
            finally
            {
                conn.Close();
            }
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}