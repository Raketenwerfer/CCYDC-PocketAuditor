using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using PocketAuditor.Database;
using System.Collections.Generic;
using PocketAuditor.Class;

namespace PocketAuditor.Adapter
{
    public class ItemAdapter : RecyclerView.Adapter
    {
        private readonly List<ItemModel> itemList;

        private readonly DB_Initiator dbInitiator;
        private readonly Context context;

        private readonly DataSharingService dss = DataSharingService.GetInstance();

        public int c01;

        public ItemAdapter(List<ItemModel> itemList, DB_Initiator dbInitiator)
        {
            this.itemList = itemList;
            this.dbInitiator = dbInitiator;
            this.context = Application.Context;
        }

        public override int ItemCount => itemList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ItemModel itemModel = itemList[position];
            ItemViewHolder view = holder as ItemViewHolder;

            // Bind Data from the ItemModel to the Views inside the ViewHolder
            view.Identifier.Text = itemModel.CategoryTitle;
            view.Question.Text = itemModel.EntryQuestion;
            view.Remark.Text = itemModel.Remark;

            // Set the state of the radio buttons based on the itemModel
            view.rbnYes.Checked = itemModel.IsYesBtnSelected;
            view.rbnNo.Checked = itemModel.IsNoBtnSelected;

             // Set Click listeners for the radiobuttons 
            view.rbnYes.Click += (sender, e) => OnYesButtonClick(holder, position);
            view.rbnNo.Click += (sender, e) => OnNoButtonClick(holder, position);

            // Set Text Change Listener for the editText
            view.Remark.TextChanged += (sender, e) => OnRemarkTextChanged(position, e.Text.ToString());
        }

        private void OnNoButtonClick(RecyclerView.ViewHolder holder, int position)
        {

            ItemViewHolder x = holder as ItemViewHolder;
            ItemModel itemModel = itemList[position];

            itemModel.checkValue = false;
            itemModel.isTrue = "false";
            
            itemModel.IsNoBtnSelected = true;
            itemModel.IsYesBtnSelected = false;

            if (itemModel.btnIsInteracted != "yes")
            {
                itemModel.btnIsInteracted = "yes";
                c01++;
            }
            else
            {
                itemModel.btnIsInteracted = "yes";
            }


            NotifyItemChanged(position);

            UpdateTracker();
        }

        private void OnYesButtonClick(RecyclerView.ViewHolder holder, int position)
        {

            ItemViewHolder x = holder as ItemViewHolder;
            ItemModel itemModel = itemList[position];

            itemModel.checkValue = true;
            itemModel.isTrue = "true";

            itemModel.IsNoBtnSelected = false;
            itemModel.IsYesBtnSelected = true;

            if (itemModel.btnIsInteracted != "yes")
            {
                itemModel.btnIsInteracted = "yes";
                c01++;
            }
            else
            {
                itemModel.btnIsInteracted = "yes";
            }


            NotifyItemChanged(position);

            UpdateTracker();
        }

        private void UpdateTracker()
        {

            dss.SetInteractions(c01);
            dss.SetItemCount(itemList.Count);
            dss.UpdateProgress();
        }

        private void OnRemarkTextChanged(int position, string v)
        {
            ItemModel itemModel = itemList[position];
            itemModel.Remark = v;
        }


       
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the item_model layout and create ViewHolder instance
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_model, parent, false);
            return new ItemViewHolder(itemView);
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView Identifier;
            public TextView Question;
            public RadioButton rbnYes;
            public RadioButton rbnNo;
            public EditText Remark;

            public ItemViewHolder(View itemView) : base(itemView) // Generating Constructor
            {
                // Initialize views from item layout
                Identifier = itemView.FindViewById<TextView>(Resource.Id.II_Identifier);
                Question = itemView.FindViewById<TextView>(Resource.Id.II_Question);
                rbnYes = itemView.FindViewById<RadioButton>(Resource.Id.rbn_Yes);
                rbnNo = itemView.FindViewById<RadioButton>(Resource.Id.rbn_No);
                Remark = itemView.FindViewById<EditText>(Resource.Id.II_Remark);
            }
        }
    }
}