using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using PocketAuditor.Database;
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

        EditText CateNumber_eT, CateName_eT, CateQuestion_eT;
        Button AddCategory_Save, Cancel_Category;
        ImageView AQuestion;

        int sequence;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.add_category);

            CateNumber_eT = FindViewById<EditText>(Resource.Id.ACNumber_eT); //4 category ID
            CateName_eT = FindViewById<EditText>(Resource.Id.ACName_eT); //4 category Title
            CateQuestion_eT = FindViewById<EditText>(Resource.Id.ACQuestion_eT); //editText 4 question
            AQuestion = FindViewById<ImageView>(Resource.Id.AddQuestion);

            AddCategory_Save = FindViewById<Button>(Resource.Id.AddCa_btnSave);
            Cancel_Category = FindViewById<Button>(Resource.Id.btnCancel);

            AddCategory_Save.Click += AddCategory_Save_Click; 
            Cancel_Category.Click += Cancel_Category_Click;
            AQuestion.Click += AQuestion_Click;
            
            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;
        }

        private void AQuestion_Click(object sender, EventArgs e)
        {
            //If mo input ang user sa ACQuestion.EditText inig click sa ImageView nga ADD
            //the inputted question will save in the database and display
            //mo display siyag message "Question Added"
        }

        private void AddCategory_Save_Click(object sender, EventArgs e)
        {
            //string categoryID = CateNumber_eT.Text;

            string add_cat_query = "";
            string get_sequence = "";

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