﻿using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Result = MyTherapy.Models.Result;

namespace MyTherapy.Adapters
{
	public class PlaceAdapter : BaseAdapter<Result>
	{
		List<Result> items;
		Activity context;
		public PlaceAdapter(Activity context, List<Result> items)
			: base()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position) => position;

		public override Result this[int position] => items[position];

		public override int Count => items.Count;

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.list_item_place, null);

			view.FindViewById<TextView>(Resource.Id.textView1).Text = string.Format(context.GetString(Resource.String.list_item), position + 1);
			view.FindViewById<TextView>(Resource.Id.textView2).Text = item.name;
			view.FindViewById<TextView>(Resource.Id.textView3).Text = item.vicinity;

			return view;
		}
		public void RemoveItemAt(int position) => items.RemoveAt(position);

		internal Result GetFromItem(int position) => items[position];
	}
}