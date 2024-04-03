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
using static Java.Text.Normalizer;

namespace PocketAuditor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class AuditActivity : AppCompatActivity
    {
        private RecyclerView recycler;
        private adpt_Categories adapter;

        #region Database and Models


        /// Home Wifi: 192.168.254.102
        /// Built-in Route: 127.0.0.1
        DatabaseInitiator dbInit = new DatabaseInitiator("192.168.254.102", "ccydc_database", "root", ";");
        public List<mdl_Categories> _Categories = new List<mdl_Categories>();
        public List<mdl_SubCategories> _SubCategories = new List<mdl_SubCategories>();
        public List<mdl_Indicators> _Indicators = new List<mdl_Indicators>();
        public List<mdl_UnsortedIndicators> _UnsortedIndicators = new List<mdl_UnsortedIndicators>();
        public List<mdl_SubIndicators> _SubIndicators = new List<mdl_SubIndicators>();
        public List<jmdl_IndicatorsSubInd> _jmISI = new List<jmdl_IndicatorsSubInd>();
        public List<jmdl_CategoriesIndicators> _jmCI = new List<jmdl_CategoriesIndicators>();
        public List<jmdl_CategoriesSubCategories> _jmCSC = new List<jmdl_CategoriesSubCategories>();
        public List<jmdl_IndicatorSubCat> _jmISC = new List<jmdl_IndicatorSubCat>();

        public DataSharingService DSS;

        #endregion

        Button next;
        public ImageView returnMenu;
        public TextView audit_progress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);


            DSS = DataSharingService.GetInstance();
            DSS.SetProgress(audit_progress);

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
            PullAssociate_CI();
            PullSubCategories();
            PullAssociate_CSC();
            PullAssociate_ISC();
            PullUnsortedIndicators();

            // Create adapter and set it to RecyclerView
            adapter = new adpt_Categories(_Categories, _Indicators, _jmISI, _jmCI, _jmCSC, this);
            recycler.SetAdapter(adapter);

            // This line of code will erase all entries in the EntryAnswers_tbl table
            // This is done so it can be reused for new audits. Will be moved elsewhere
            // Once other functionalities are done

            //SQLDB.RawQuery("DELETE FROM EntryAnswers_tbl", null);

            audit_progress.Enabled = false;
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

            string getCatQuery = "SELECT * FROM categories WHERE CategoryStatus = 'ACTIVE'";

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

        public void PullSubCategories()
        {
            _SubCategories.Clear();

            string query = "SELECT * FROM subcategory";

            MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Extract data from the reader and create an instance of mdl_SubCategories
                            int id = reader.GetInt32(reader.GetOrdinal("SubCategoryID"));
                            string title = reader.GetString(reader.GetOrdinal("SubCategoryTitle"));
                            string status = reader.GetString(reader.GetOrdinal("SubCategoryStatus"));

                            // Create an instance of mdl_SubCategories and add it to the list
                            mdl_SubCategories subCategory = new mdl_SubCategories(id, title, status);
                            _SubCategories.Add(subCategory);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void PullIndicators()
        {
            int _indID;
            double _indScoreValue;
            string _indTitle, _indStatus;

            string getIndQuery = "SELECT * From indicators WHERE IndicatorStatus = 'ACTIVE'";

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
                            _indScoreValue = read.GetDouble(read.GetOrdinal("ScoreValue"));
                            _indTitle = read.GetString(read.GetOrdinal("Indicator"));
                            _indStatus = read.GetString(read.GetOrdinal("IndicatorStatus"));

                            mdl_Indicators a = new mdl_Indicators(_indID, _indScoreValue, _indTitle, _indStatus);
                            {
                                a.IndicatorID = _indID;
                                a.ScoreValue = _indScoreValue;
                                a.Indicator = _indTitle;
                                a.IndicatorStatus = _indStatus;
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

            string getSubIndQuery = "SELECT * FROM subindicators WHERE SubIndicatorStatus = 'ACTIVE'";

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
                DSS.SI_SetList(_SubIndicators);
            }
        }

        public void PullAssociate_ISI()
        {
            int indicatorFK, subindicatorFK;

            string query = "SELECT * FROM associate_indicator_to_subindicator";

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
                DSS.ISI_SetList(_jmISI);
            }
        }

        public void PullAssociate_CI()
        {
            int indicatorID, categoryID;
            string catTitle, indicator;
            double indScoreValue;

            string query = "SELECT C.CategoryID, C.CategoryTitle, I.IndicatorID, I.Indicator, I.ScoreValue\r\n" +
                "FROM associate_category_to_indicator AtC " +
                "INNER JOIN categories C on AtC.CategoryID_fk = C.CategoryID " +
                "INNER JOIN indicators I on AtC.IndicatorID_fk = I.IndicatorID " +
                "WHERE (C.CategoryStatus = 'ACTIVE' AND I.IndicatorStatus = 'ACTIVE')";

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
                            indicatorID = read.GetInt32(read.GetOrdinal("IndicatorID"));
                            indicator = read.GetString(read.GetOrdinal("Indicator"));
                            categoryID = read.GetInt32(read.GetOrdinal("CategoryID"));
                            catTitle = read.GetString(read.GetOrdinal("CategoryTitle"));
                            indScoreValue = read.GetDouble(read.GetOrdinal("ScoreValue"));

                            jmdl_CategoriesIndicators a = new jmdl_CategoriesIndicators(categoryID, catTitle, indicatorID,
                                indicator, indScoreValue);
                            {
                                a.CategoryID = categoryID;
                                a.CategoryTitle = catTitle;
                                a.IndicatorID = indicatorID;
                                a.Indicator = indicator;
                                a.ScoreValue = indScoreValue;
                            }

                            _jmCI.Add(a);
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

        public void PullAssociate_CSC()
        {
            _jmCSC.Clear();

            string query = "SELECT SC.SubCategoryID, SC.SubCategoryTitle, SC.SubCategoryStatus, " +
                           "C.CategoryID, C.CategoryTitle, C.CategoryStatus " +
                           "FROM subcategory SC " +
                           "JOIN associate_category_to_subcategory ACSC ON SC.SubCategoryID = ACSC.SubCategoryID_fk " +
                           "JOIN categories C ON ACSC.CategoryID_fk = C.CategoryID";

            using MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Extract data from the reader and create an instance of jmdl_CategoriesSubCategories
                            int categoryId = reader.GetInt32(reader.GetOrdinal("CategoryID"));
                            int subcategoryId = reader.GetInt32(reader.GetOrdinal("SubCategoryID"));
                            string categoryTitle = reader.GetString(reader.GetOrdinal("CategoryTitle"));
                            string subCategoryTitle = reader.GetString(reader.GetOrdinal("SubCategoryTitle"));
                            string subCategoryStatus = reader.GetString(reader.GetOrdinal("SubCategoryStatus"));

                            // Create an instance of jmdl_CategoriesSubCategories and add it to the list
                            jmdl_CategoriesSubCategories association = new jmdl_CategoriesSubCategories(categoryId,
                                subcategoryId, categoryTitle, subCategoryTitle, subCategoryStatus);
                            _jmCSC.Add(association);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void PullAssociate_ISC()
        {
            _jmISC.Clear();

            string query = "SELECT " +
               "AIS.IndicatorID_fk, I.Indicator, ACSC.CategoryID_fk, AIS.SubCategoryID_fk, " +
               "SC.SubCategoryTitle, SC.SubCategoryStatus " +
               "FROM associate_indicator_to_subcategory AIS " +
               "JOIN indicators I ON AIS.IndicatorID_fk = I.IndicatorID " +
               "JOIN associate_category_to_subcategory ACSC ON AIS.SubCategoryID_fk = ACSC.SubCategoryID_fk " +
               "JOIN subcategory SC ON ACSC.SubCategoryID_fk = SC.SubCategoryID";

            using MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Extract data from the reader and create an instance of jmdl_IndicatorSubCat
                            int indicatorIDfk = reader.GetInt32(reader.GetOrdinal("IndicatorID_fk"));
                            string indicator = reader.GetString(reader.GetOrdinal("Indicator"));
                            int categoryIDfk = reader.GetInt32(reader.GetOrdinal("CategoryID_fk"));
                            int subCategoryIDfk = reader.GetInt32(reader.GetOrdinal("SubCategoryID_fk"));
                            string subCategoryTitle = reader.GetString(reader.GetOrdinal("SubCategoryTitle"));
                            string subCategoryStatus = reader.GetString(reader.GetOrdinal("SubCategoryStatus"));

                            // Create an instance of jmdl_IndicatorSubCat and add it to the list
                            jmdl_IndicatorSubCat indicatorSubCat = new jmdl_IndicatorSubCat(
                                indicatorIDfk, indicator, categoryIDfk, subCategoryIDfk, subCategoryTitle, subCategoryStatus);

                            _jmISC.Add(indicatorSubCat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DSS.ISC_SetList(_jmISC);
        }


        public void PullUnsortedIndicators()
        {

            _UnsortedIndicators.Clear();

            string query = "SELECT I.IndicatorID, I.Indicator, I.ScoreValue, I.IndicatorStatus, C.CategoryID_fk " +
                "FROM indicators I " +
                "INNER JOIN associate_category_to_indicator C ON C.IndicatorID_fk = I.IndicatorID " +
                "LEFT JOIN associate_indicator_to_subcategory AIS ON AIS.IndicatorID_fk = I.IndicatorID " +
                "LEFT JOIN associate_category_to_subcategory ACSC ON AIS.SubCategoryID_fk = ACSC.SubCategoryID_fk " +
                "WHERE(AIS.IndicatorID_fk IS NULL AND I.IndicatorStatus = 'ACTIVE') " +
                "OR ACSC.SubCategoryID_fk IS NULL";

            using MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader read = cmd.ExecuteReader())
                    {
                        while (read.Read())
                        {
                            int _indID = read.GetInt32(read.GetOrdinal("IndicatorID"));
                            int _catID = read.GetInt32(read.GetOrdinal("CategoryID_fk"));
                            double _indScoreValue = read.GetDouble(read.GetOrdinal("ScoreValue"));
                            string _indTitle = read.GetString(read.GetOrdinal("Indicator"));
                            string _indStatus = read.GetString(read.GetOrdinal("IndicatorStatus"));

                            mdl_UnsortedIndicators a = new mdl_UnsortedIndicators(_indID, _indScoreValue,
                                _indTitle, _indStatus, _catID);


                            _UnsortedIndicators.Add(a);
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

            DSS.USI_SetList(_UnsortedIndicators);
        }

        #endregion

    }
}