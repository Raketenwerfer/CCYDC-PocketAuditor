using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using PocketAuditor.Class;
using PocketAuditor.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Adapter
{
    public class QuestionAdapter : RecyclerView.Adapter
    {
        private readonly List<QuestionModel> _questions;
        private readonly DB_Initiator _dbInitiator;

        public QuestionAdapter(List<QuestionModel> questions, DB_Initiator dbInitiator)
        {
            _questions = questions;
            _dbInitiator = dbInitiator;
        }

        public override int ItemCount => _questions.Count();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            QuestionModel questionModel = _questions[position];
            ItemViewHolder view = holder as ItemViewHolder;

            view.Display_CatID.Text = questionModel.CatID;
            view.Display_CatQues.Text = questionModel.Indicator;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.question_model, parent, false);
            return new ItemViewHolder(itemView);
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView Display_CatID;
            public TextView Display_CatQues;

            public ItemViewHolder(View itemView) : base(itemView)
            {
                Display_CatID = itemView.FindViewById<TextView>(Resource.Id.Display_catID);
                Display_CatQues = itemView.FindViewById<TextView>(Resource.Id.Display_cateQues);
            }
        }
    }
}