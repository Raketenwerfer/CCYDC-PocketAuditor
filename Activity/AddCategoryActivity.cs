using Android.App;
using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using PocketAuditor.Database;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Activity
{
    [Activity(Label = "AddCategoryActivity")]
    public class AddCategoryActivity : AppCompatActivity
    {
        private Android.App.AlertDialog alertDialog;

        EditText CateName_eT;
        Button AddCategory_Save, Cancel_Category;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        string get_sequence = "SELECT COUNT(*) FROM Category_tbl";
        int sequence;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.add_category);

            /// Please lang ko remove sa Category Questions, Action Plans
            /// ug Category Number. Kani nga view kay pang-add rajud ug
            /// Category Name. Ang Category_ID ug CategoryStatus kay handled na sa
            /// GetRowSequenceCount() ug "ACTIVE" na value sa _db.Execute()
            CateName_eT = FindViewById<EditText>(Resource.Id.ACName_eT); //4 category Title

            AddCategory_Save = FindViewById<Button>(Resource.Id.AddCa_btnSave);
            Cancel_Category = FindViewById<Button>(Resource.Id.btnCancel);

            AddCategory_Save.Click += AddCategory_Save_Click; 
            Cancel_Category.Click += Cancel_Category_Click;
            
            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;
        }

        private void GetRowSequenceCount()
        {
            ICursor gseq = SQLDB.RawQuery(get_sequence, new string[] { });

            sequence = gseq.GetInt(gseq.GetColumnIndex("COLUMN(*)"));
        }

        
        private void AddCategory_Save_Click(object sender, EventArgs e)
        {
            //string categoryID = CateNumber_eT.Text;
            GetRowSequenceCount();
            sequence++;

            var _db = new SQLiteConnection(handler._ConnPath);

            _db.Execute("INSERT INTO Category_tbl(Category_ID, CategoryTitle, CategoryStatus)" +
                            "VALUES (?, ?, ?)", sequence, CateName_eT.Text, "ACTIVE");

            Toast.MakeText(Application.Context, "New Category Inserted", ToastLength.Short).Show();
        }

        private void Cancel_Category_Click(object sender, EventArgs e)
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
            builder.SetMessage("Are you sure you want to close this page?");

            builder.SetPositiveButton("Yes", (dialog, which) =>
            {
                Finish();
            });
            builder.SetNegativeButton("No", (dialog, which) =>
            {
                alertDialog.Dismiss();
            });

            alertDialog = builder.Create();
            alertDialog.Show();
        }
    }
}