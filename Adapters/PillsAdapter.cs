using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using MyTherapy.Models;

namespace MyTherapy.Adapters
{
	public class PillsAdapter : BaseAdapter<Pills>
	{
		List<Pills> items;
		Activity context;
		public PillsAdapter(Activity context, List<Pills> items)
			: base()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position) => position;

		public override Pills this[int position] => items[position];

		public override int Count => items.Count;

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.list_item_pills, null);
			view.FindViewById<TextView>(Resource.Id.textView1).Text = string.Format(context.GetString(Resource.String.list_item), position + 1);
			view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Date.ToString("dd.MM.yyyy.");
			view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Quantity.ToString();

			return view;
		}
		public void RemoveItemAt(int position) => items.RemoveAt(position);

		internal Pills GetFromItem(int position) => items[position];
	}
}