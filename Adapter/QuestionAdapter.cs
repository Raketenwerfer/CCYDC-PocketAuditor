using Android.App;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Android.Database;
using Android.Database.Sqlite;
using PocketAuditor.Class;
using PocketAuditor.Database;
using System.Collections.Generic;
using System.Linq;
using PocketAuditor.Fragment;
using SQLite;

namespace PocketAuditor.Adapter
{
    public class QuestionAdapter : RecyclerView.Adapter
    {
        private readonly List<QuestionModel> _questions;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        private readonly ManageCQ _activity;

        int _selectedQuestion;

        public QuestionAdapter(List<QuestionModel> questions, ManageCQ activity)
        {
            _questions = questions;
            _activity = activity;

            handler = new DB_Initiator(_activity);
            SQLDB = handler.WritableDatabase;
        }

        public override int ItemCount => _questions.Count();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            QuestionModel questionModel = _questions[position];
            ItemViewHolder view = holder as ItemViewHolder;

            view.Display_CatID.Text = questionModel.EntryID.ToString();
            view.Display_CatQues.Text = questionModel.Indicator;

            view.EditQuestion.Click += (sender, e) => _RenameQuestion(holder, position);
            view.DeleteQuestion.Click += (sender, e) => _DeleteQuestion(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View questionView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.question_model, parent, false);
            return new ItemViewHolder(questionView);
        }

        public void _RenameQuestion(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder view = holder as ItemViewHolder;

            _handleRename(holder, position);
            Toast.MakeText(Application.Context, "Renaming " + view.Display_CatQues.Text, ToastLength.Short).Show();
        }

        public void _DeleteQuestion(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder view = holder as ItemViewHolder;

            _handleDelete(holder, position);
            Toast.MakeText(Application.Context, "Deleting " + view.Display_CatQues.Text, ToastLength.Short).Show();
        }

        public void GetQuestionEntryID(string i1)
        {
            ICursor cList = SQLDB.RawQuery("SELECT EntryID FROM Entry_tbl " +
                                           "WHERE Indicator = '" + i1 + "'", new string[] { });

            if (cList.Count > 0)
            {
                cList.MoveToFirst();

                do
                {
                    _selectedQuestion = cList.GetInt(cList.GetColumnIndex("EntryID"));
                }
                while (cList.MoveToNext());

                cList.Close();
            }
        }

        public void _handleRename(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder view = holder as ItemViewHolder;

            LayoutInflater layoutInflater = LayoutInflater.FromContext(_activity);
            View mView = layoutInflater.Inflate(Resource.Layout.new_question_prompt, null);

            AlertDialog.Builder builder = new AlertDialog.Builder(_activity);
            builder.SetView(mView);

            var userContent = mView.FindViewById<EditText>(Resource.Id.ANQuestion_eT);
            var promptTitle = mView.FindViewById<TextView>(Resource.Id.qm_title);
            var promptDesc = mView.FindViewById<TextView>(Resource.Id.qm_desc);

            userContent.Text = view.Display_CatQues.Text;

            GetQuestionEntryID(userContent.Text);

            promptTitle.Text = "RENAME QUESTION";
            promptDesc.Text = "Enter your new question name:";

            builder.SetCancelable(false)
                .SetPositiveButton("Rename", delegate
                {
                    var _db = new SQLiteConnection(handler._ConnPath);

                    // Use placeholders and parameters in your SQL query
                    _db.Execute("UPDATE Entry_tbl " +
                                "SET Indicator = ? " +
                                "WHERE EntryID = ?", userContent.Text.ToString(), _selectedQuestion);

                    Toast.MakeText(Application.Context, "Renaming successful!!", ToastLength.Short).Show();
                    _db.Commit();
                    _db.Close();

                    _activity.PullEntries();

                })
                .SetNegativeButton("Cancel", delegate
                {
                    builder.Dispose();
                });

            builder.Create().Show();
        }

        private void _handleDelete(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder view = holder as ItemViewHolder;

            LayoutInflater layoutInflater = LayoutInflater.FromContext(_activity);
            View mView = layoutInflater.Inflate(Resource.Layout.new_question_prompt, null);

            AlertDialog.Builder builder = new AlertDialog.Builder(_activity);
            builder.SetView(mView);

            var userContent = mView.FindViewById<EditText>(Resource.Id.ANQuestion_eT);
            var promptTitle = mView.FindViewById<TextView>(Resource.Id.qm_title);
            var promptDesc = mView.FindViewById<TextView>(Resource.Id.qm_desc);

            userContent.Text = view.Display_CatQues.Text;

            GetQuestionEntryID(userContent.Text);

            promptTitle.Text = "DELETE QUESTION";
            promptDesc.Text = "Are you sure you want to delete this question?";
            userContent.Visibility = ViewStates.Visible;

            builder.SetCancelable(false)
                .SetPositiveButton("Delete", delegate
                {
                    var _db = new SQLiteConnection(handler._ConnPath);

                    // Use placeholders and parameters in your SQL query
                    _db.Execute("UPDATE Entry_tbl " +
                                "SET EntryStatus = 'INACTIVE' " +
                                "WHERE EntryID = ?", _selectedQuestion);

                    Toast.MakeText(Application.Context, "Deletion successful!", ToastLength.Short).Show();
                    _db.Commit();
                    _db.Close();

                    _activity.PullEntries();
                })
                .SetNegativeButton("Cancel", delegate
                {
                    builder.Dispose();
                });

            builder.Create().Show();
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView Display_CatID;
            public TextView Display_CatQues;
            public ImageView EditQuestion;
            public ImageView DeleteQuestion;


            public ItemViewHolder(View itemView) : base(itemView)
            {
                Display_CatID = itemView.FindViewById<TextView>(Resource.Id.Pull_CID);
                Display_CatQues = itemView.FindViewById<TextView>(Resource.Id.Display_CatIndi);
                EditQuestion = itemView.FindViewById<ImageView>(Resource.Id.btnEditQuestion);
                DeleteQuestion = itemView.FindViewById<ImageView>(Resource.Id.btnDelQuestion);

            }

        }
    }
}