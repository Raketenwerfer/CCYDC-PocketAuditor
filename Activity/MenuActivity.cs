using Android.App;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.CardView.Widget;
using MySqlConnector;
using Pocket_Auditor_Admin_Panel.Auxiliaries;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Adapter;
using PocketAuditor.Class;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;

namespace PocketAuditor.Fragment
{
    [Activity(Label = "MenuActivity", NoHistory = true)]
    public class MenuActivity : AppCompatActivity
    {

        /// Home Wifi: 192.168.254.102
        /// Built-in Route: 127.0.0.1
        /// CCS AP: 172.176.8.208
        public DatabaseInitiator dbInit;
        public DataSharingService DSS;
        public Spinner list;
        public adpt_Barangay adapter;

        public List<mdl_SKChapters> chapters = new List<mdl_SKChapters>();


        ImageButton beginAudit;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.menu);

            DSS = DataSharingService.GetInstance();
            dbInit = DSS.GetDatabase();

            PullChapters();

            list = FindViewById<Spinner>(Resource.Id.ChapterSelectSpinner);
            adapter = new adpt_Barangay(this, Android.Resource.Layout.SimpleSpinnerItem, chapters);
            list.Adapter = adapter;

            try
            {
                if (chapters != null)
                {
                    list.SetSelection(0);
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
            }


            beginAudit = FindViewById<ImageButton>(Resource.Id.BeginAuditButton);

            beginAudit.Click += BeginAudit_Click;


            list.ItemSelected += (sender, e) =>
            {
                var selectedChapter = adapter.GetItem(e.Position);
                DSS.SetSelectedChapter(selectedChapter.ChapterID, selectedChapter.Barangay,
                    selectedChapter.hasFinishedAudit);
            };
        }


        private void BeginAudit_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(AuditActivity));
        }

        public void PullChapters()
        {
            string query = "SELECT ChapterID, Barangay FROM skchapters WHERE hasFinishedAudit = 0";

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
                            int id = read.GetInt32(read.GetOrdinal("ChapterID"));
                            string name = read.GetString(read.GetOrdinal("Barangay"));

                            mdl_SKChapters chp = new mdl_SKChapters(id, name, false);
                            chapters.Add(chp);
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

            }
        }
    }
}