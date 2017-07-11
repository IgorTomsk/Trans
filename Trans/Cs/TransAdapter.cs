using System;
using System.Collections.Generic;
using System.Net;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;


using System.Runtime;
using System.Linq;

namespace Trans
{
    public class TransAdapter : BaseAdapter<HistoryData>
    {
        List<HistoryData> items;
        Activity context;

        ViewHolderHomeScreenItem viewHolder;

        public TransAdapter(Activity context, List<HistoryData> items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override HistoryData this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];

            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.OneTransLine, null);  // here we connect one row to list item

                InitializeViewHolder(view);

                view.Tag = viewHolder;
            }
            else
            {
                viewHolder = (ViewHolderHomeScreenItem)view.Tag;
            }

            FillViewHolder(item);

            return view;
        }

        private void InitializeViewHolder(View view)
        {
            try
            {
                viewHolder = new ViewHolderHomeScreenItem();

                viewHolder.fromText = view.FindViewById<TextView>(Resource.Id.whatTrans);
                viewHolder.toText = view.FindViewById<TextView>(Resource.Id.ResultTrans);
                            }
            catch (Exception ex)
            {
                var q = 1;
            }
        }

        private void FillViewHolder(HistoryData item)
        {
            try
            {

                viewHolder.fromText.Text = item.textFrom;
                viewHolder.toText.Text = item.textTo;
            }  
            catch
            {
                var a = 2;
            }
                
        }

 


    }

    class ViewHolderHomeScreenItem : Java.Lang.Object
    {
        internal TextView fromText;
        internal TextView toText;

    }
}