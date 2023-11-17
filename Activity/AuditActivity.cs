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

        #region Database and Models

        DatabaseInitiator dbInit = new DatabaseInitiator("sql.freedb.tech", "3306", "freedb_ccydc_test_db", "freedb_ccydc", "r*kmjEa6N#KUsDN");
        public List<mdl_Categories> _Categories = new List<mdl_Categories>();
        public List<mdl_Indicators> _Indicators = new List<mdl_Indicators>();
        public List<mdl_SubIndicators> _SubIndicators = new List<mdl_SubIndicators>();
        public List<jmdl_IndicatorsSubInd> _jmISI = new List<jmdl_IndicatorsSubInd>();


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
            //SQLDB = handler.WritableDatabase; OLD 

            PullCategories();
            PullIndicators();
            PullSubIndicators();
            PullAssociate_ISI();

            // Create adapter and set it to RecyclerView
            adapter = new adpt_Categories(_Categories, _Indicators, _jmISI);
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



        #region Database Queries

        public void PullCategories()
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

        public void PullIndicators()
        {
            int _indID, _indNo;
            double _indScoreValue;
            string _indTitle, _indStatus, _indType;

            string getIndQuery = "SELECT * From Indicators WHERE IndicatorStatus = 'ACTIVE'";

            MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(getIndQuery, conn))
                {
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            _indID = read.GetInt32(read.GetOrdinal("IndicatorID"));
                            _indNo = read.GetInt32(read.GetOrdinal("IndicatorNumber"));
                            _indScoreValue = read.GetDouble(read.GetOrdinal("ScoreValue"));
                            _indTitle = read.GetString(read.GetOrdinal("Indicator"));
                            _indStatus = read.GetString(read.GetOrdinal("IndicatorStatus"));
                            _indType = read.GetString(read.GetOrdinal("IndicatorType"));

                            mdl_Indicators a = new mdl_Indicators(_indID, _indNo, _indScoreValue, _indTitle, _indStatus, _indType);
                            {
                                a.IndicatorID = _indID;
                                a.IndicatorNumber = _indNo;
                                a.ScoreValue = _indScoreValue;
                                a.Indicator = _indTitle;
                                a.IndicatorStatus = _indStatus;
                                a.IndicatorType = _indType;
                            }

                            _Indicators.Add(a);
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

        public void PullSubIndicators()
        {
            int _subIndID;
            string _subIndTitle, _subIndType, _subIndStatus;
            double _subIndScoreValue;

            string getSubIndQuery = "SELECT * FROM SubIndicators WHERE SubIndicatorStatus = 'ACTIVE'";

            MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(getSubIndQuery, conn))
                {
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            _subIndID = read.GetInt32(read.GetOrdinal("SubIndicatorID"));
                            _subIndTitle = read.GetString(read.GetOrdinal("SubIndicator"));
                            _subIndType = read.GetString(read.GetOrdinal("SubIndicatorType"));
                            _subIndStatus = read.GetString(read.GetOrdinal("SubIndicatorStatus"));
                            _subIndScoreValue = read.GetDouble(read.GetOrdinal("ScoreValue"));

                            mdl_SubIndicators a = new mdl_SubIndicators(_subIndID, _subIndTitle, _subIndType, _subIndStatus, _subIndScoreValue);
                            {
                                a.SubIndicatorID = _subIndID;
                                a.SubIndicator = _subIndTitle;
                                a.SubIndicatorType = _subIndType;
                                a.SubIndicatorStatus = _subIndStatus;
                                a.ScoreValue = _subIndScoreValue;
                            }

                            _SubIndicators.Add(a);
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

        public void PullAssociate_ISI()
        {
            int indicatorFK, subindicatorFK;

            string query = "SELECT * FROM `Associate_Indicator_to_SubIndicator`";

            MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            indicatorFK = read.GetInt32(read.GetOrdinal("IndicatorID_fk"));
                            subindicatorFK = read.GetInt32(read.GetOrdinal("SubIndicatorID_fk"));

                            jmdl_IndicatorsSubInd a = new jmdl_IndicatorsSubInd(subindicatorFK, indicatorFK);
                            {
                                a.SubIndicatorID_fk = subindicatorFK;
                                a.IndicatorID_fk = indicatorFK;
                            }

                            _jmISI.Add(a);
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

        #endregion

    }
}