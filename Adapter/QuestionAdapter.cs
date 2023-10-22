using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Android.Support.Design.Widget;
using AndroidX.DrawerLayout.Widget;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Android.Database;
using Android.Database.Sqlite;
using PocketAuditor.Class;
using PocketAuditor.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PocketAuditor.Fragment;

namespace PocketAuditor.Adapter
{
    public class QuestionAdapter : RecyclerView.Adapter
    {
        private readonly List<QuestionModel> _questions;

        public DB_Initiator handler;
        public SQLiteDatabase SQLDB;

        private readonly ManageCQ _activity;

        public QuestionAdapter(List<QuestionModel> questions, ManageCQ activity)
        {
            _questions = questions;
            _activity = activity;
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
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.question_model, parent, false);
            return new ItemViewHolder(itemView);
        }

        public void _RenameQuestion(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder view = holder as ItemViewHolder;

            _handleRename();
            Toast.MakeText(Application.Context, "Renaming " + view.Display_CatQues.Text, ToastLength.Short).Show();
        }

        public void _DeleteQuestion(RecyclerView.ViewHolder holder, int position)
        {
            ItemViewHolder view = holder as ItemViewHolder;

            _handleDelete();
            Toast.MakeText(Application.Context, "Deleting " + view.Display_CatQues.Text, ToastLength.Short).Show();
        }

        public void _handleRename()
        {
            LayoutInflater layoutInflater = LayoutInflater.FromContext(_activity);
            View mView = layoutInflater.Inflate(Resource.Layout.new_question_prompt, null);

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(_activity);
            builder.SetView(mView);

            var userContent = mView.FindViewById<EditText>(Resource.Id.ANQuestion_eT);
            var promptTitle = mView.FindViewById<TextView>(Resource.Id.qm_title);
            var promptDesc = mView.FindViewById<TextView>(Resource.Id.qm_desc);

            promptTitle.Text = "RENAME QUESTION";
            promptDesc.Text = "Enter your new question name:";


            builder.SetCancelable(false)
                .SetPositiveButton("Rename", delegate
                {
                    string newQuestion = userContent.Text;

                })
                .SetNegativeButton("Cancel", delegate
                {
                    builder.Dispose();
                });
            builder.Create().Show();
        }

        private void _handleDelete()
        {
            LayoutInflater layoutInflater = LayoutInflater.FromContext(_activity);
            View mView = layoutInflater.Inflate(Resource.Layout.new_question_prompt, null);

            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(_activity);
            builder.SetView(mView);

            var userContent = mView.FindViewById<EditText>(Resource.Id.ANQuestion_eT);
            var promptTitle = mView.FindViewById<TextView>(Resource.Id.qm_title);
            var promptDesc = mView.FindViewById<TextView>(Resource.Id.qm_desc);

            promptTitle.Text = "DELETE QUESTION";
            promptDesc.Text = "Are you sure you want to delete this question?";
            userContent.Visibility = ViewStates.Invisible;

            builder.SetCancelable(false)
                .SetPositiveButton("Delete", delegate
                {
                    string newQuestion = userContent.Text;

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