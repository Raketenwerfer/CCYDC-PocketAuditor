using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using PocketAuditor.Database;
using System;
using System.Collections.Generic;

namespace PocketAuditor.Adapter
{
    public class QuestionAdapter : RecyclerView.Adapter
    {
        public List<QuestionAdapter> QuestionAdapters = new List<QuestionAdapter>();

        public event EventHandler<QuestionAdapterClickEventArgs> ItemClick;
        public event EventHandler<QuestionAdapterClickEventArgs> ItemLongClick;
        string[] items;

        private readonly DB_Initiator dbInitiator;
        private readonly Context context;

        public QuestionAdapter(string[] QuesAdapter)
        {
            items = QuesAdapter;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            //var id = Resource.Layout.__YOUR_ITEM_HERE;
            //itemView = LayoutInflater.From(parent.Context).
            //       Inflate(QuestionAdapter, parent, false);

            var vh = new QuestionAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as QuestionAdapterViewHolder;
            //holder.TextView.Text = items[position];
        }

        public override int ItemCount => items.Length;

        void OnClick(QuestionAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(QuestionAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class QuestionAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView CategoryNum;
        public EditText DisplayQuestion;
        public ImageView QuestionEdit;
        public ImageView QuestionDelete;


        public QuestionAdapterViewHolder(View itemView, Action<QuestionAdapterClickEventArgs> clickListener,
                            Action<QuestionAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            itemView.Click += (sender, e) => clickListener(new QuestionAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new QuestionAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
        }
    }

    public class QuestionAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}