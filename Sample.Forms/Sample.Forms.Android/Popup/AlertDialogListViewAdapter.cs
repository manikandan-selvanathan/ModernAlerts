using System;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace ModernAlerts.Droid
{ 
    public class AlertDialogListViewAdapter : BaseAdapter<string>
    {
        string[] items;
        Activity context;
        Color _backgroundColor, _fontcolor;
        public AlertDialogListViewAdapter(Activity context, string[] items,Color backgroundcolor,Color fontcolor) : base()
        {
            this.context = context;
            this.items = items;
            _backgroundColor = backgroundcolor;
            _fontcolor = fontcolor;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override string this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; 
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.listview_light, null);
            var ll = view.FindViewById<LinearLayout>(Resource.Id.rootListView);
            ll.SetBackgroundColor(_backgroundColor);
            var textview = view.FindViewById<TextView>(Resource.Id.textview);
            textview.Text = items[position];
            textview.SetTextColor(_fontcolor);
            return view;
        }
    }

}
