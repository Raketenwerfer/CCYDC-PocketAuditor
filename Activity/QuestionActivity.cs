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

        public List<QuestionModel> ques_List = new List<QuestionModel>();

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.question_model);

            mQ_Recycler = FindViewById<RecyclerView>(Resource.Id.questionRecycler);
            mQ_Recycler.SetLayoutManager(new LinearLayoutManager(this));

            handler = new DB_Initiator(this);
            SQLDB = handler.WritableDatabase;

        }
    }
}