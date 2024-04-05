using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Pocket_Auditor_Admin_Panel.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketAuditor.Adapter
{
    public class adpt_Barangay : ArrayAdapter<mdl_SKChapters>
    {
        private List<mdl_SKChapters> chapters;
        private LayoutInflater inflater;

        public adpt_Barangay(Context context, int resource, List<mdl_SKChapters> objects)
            : base(context, resource, objects)
        {
            inflater = LayoutInflater.From(context);
            chapters = objects;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;

            if (convertView == null)
            {
                convertView = inflater.Inflate(Android.Resource.Layout.SimpleSpinnerItem, parent, false);
                holder = new ViewHolder();
                holder.textView = convertView.FindViewById<TextView>(Android.Resource.Id.Text1);
                convertView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            holder.textView.Text = chapters[position].Barangay;

            return convertView;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder;

            if (convertView == null)
            {
                convertView = inflater.Inflate(Android.Resource.Layout.SimpleSpinnerDropDownItem, parent, false);
                holder = new ViewHolder();
                holder.textView = convertView.FindViewById<TextView>(Android.Resource.Id.Text1);
                convertView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            holder.textView.Text = chapters[position].Barangay;

            return convertView;
        }

        public override long GetItemId(int position)
        {
            return chapters[position].ChapterID;
        }

        public override int Count => chapters.Count;

        private class ViewHolder : Java.Lang.Object
        {
            public TextView textView;
        }
    }
}