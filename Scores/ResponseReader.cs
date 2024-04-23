using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MySqlConnector;
using Pocket_Auditor_Admin_Panel.Auxiliaries;
using Pocket_Auditor_Admin_Panel.Classes;
using PocketAuditor.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Scores
{
    public class ResponseReader
    {
        private static ResponseReader instance;
        private int AuditorID;
        private string ID_Handle;
        DataSharingService DSS;
        DatabaseInitiator dbInit;

        public List<mdl_ScoreTable> Scores = new List<mdl_ScoreTable>();

        private ResponseReader()
        {
            DSS = DataSharingService.GetInstance();
            dbInit = DSS.GetDatabase();
        }

        public static ResponseReader GetInstance()
        {
            if (instance  == null)
            {
                instance = new ResponseReader();
            }

            return instance;
        }

        public void AddResponse(int chapter, int category, string subcategory,
            int indicator, string subindicator, bool ischecked, string itemchecked, string remarks,
            string subindtype)
        {
            Scores.Add(new mdl_ScoreTable(chapter, category, subcategory, indicator, subindicator,
                ischecked, itemchecked, remarks, subindtype));
        }


        public void SubmitToDatabase()
        {
            MySqlConnection conn = dbInit.GetConnection();

            try
            {
                conn.Open();

                string query = "INSERT INTO scoretable (ChapterID_fk, CategoryID_fk, SubCategoryID_fk, " +
                    "IndicatorID_fk, SubIndicatorID_fk, IsChecked, ItemChecked, Remarks, SubIndicatorType, " +
                    "AuditorID_fk, DateSubmitted) " +
                    "VALUES (@CHP_ID, @CAT_ID, @SCAT_ID, @IND_ID, @SIND_ID, @CHK, @ITEMCHK, @RMRKS, @SI_TYPE, " +
                    "@AUDITOR_ID, @DATE)";

                using (MySqlCommand cmd =  new MySqlCommand(query, conn))
                {
                    foreach (mdl_ScoreTable item in Scores)
                    {
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@CHP_ID", item.ChapterID_fk);
                        cmd.Parameters.AddWithValue("@CAT_ID", item.CategoryID_fk);
                        cmd.Parameters.AddWithValue("@SCAT_ID", item.SubCategoryID_fk);
                        cmd.Parameters.AddWithValue("@IND_ID", item.IndicatorID_fk);
                        cmd.Parameters.AddWithValue("@SIND_ID", item.SubIndicatorID_fk);
                        cmd.Parameters.AddWithValue("@CHK", item.IsChecked);
                        cmd.Parameters.AddWithValue("@ITEMCHK", item.ItemChecked);
                        cmd.Parameters.AddWithValue("@RMRKS", item.Remarks);
                        cmd.Parameters.AddWithValue("@SI_TYPE", item.SubIndicatorType);
                        cmd.Parameters.AddWithValue("@AUDITOR_ID", null);
                        cmd.Parameters.AddWithValue("@DATE", DateTime.Now.ToShortDateString());

                        cmd.ExecuteNonQuery();
                    }

                    Toast.MakeText(Application.Context, "New data submitted successfully!", ToastLength.Short).Show();
                }


                string updateChpStats = "UPDATE skchapters SET hasFinishedAudit = @AUD_ST " +
                    "WHERE ChapterID = @CHP_ID";

                using (MySqlCommand cmd = new MySqlCommand(updateChpStats, conn))
                {
                    foreach (mdl_ScoreTable item in Scores)
                    {
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@CHP_ID", item.ChapterID_fk);
                        cmd.Parameters.AddWithValue("@AUD_ST", 1);

                        cmd.ExecuteNonQuery();
                    }

                    Toast.MakeText(Application.Context, "New data submitted successfully!", ToastLength.Short).Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.Message, ToastLength.Short).Show();
            }
        }
    }
}