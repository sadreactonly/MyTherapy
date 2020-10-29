using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.Design.Button;
using Android.Widget;
using MyTherapy.Adapters;
using MyTherapy.Models;
using static Android.Widget.AdapterView;

namespace MyTherapy
{
	[Activity(Label = "PillsActivity")]
	public class PillsActivity : Activity
	{
		private EditText inrEditText;
		private MaterialButton saveButton;
		private MaterialButton inrDateButton;
		private DateTime date;
		private PillsRepository db;
		private ListView appointmentsListView;
		private PillsAdapter adapter;
		private AlertDialog.Builder ad;
		private EditText et;
		List<Pills> allPills;

		protected  override void OnCreate(Bundle savedInstanceState)
		{
			SetContentView(Resource.Layout.manage_pills);
			base.OnCreate(savedInstanceState);
			db = new PillsRepository();
			inrEditText = FindViewById<EditText>(Resource.Id.editText1);
			inrDateButton = FindViewById<MaterialButton>(Resource.Id.materialButton1);
			saveButton = FindViewById<MaterialButton>(Resource.Id.materialButton2);
			appointmentsListView = FindViewById<ListView>(Resource.Id.listView1);
			appointmentsListView.ItemLongClick += PillsListView_ItemLongClick;
			allPills =  db.Get();
			adapter = new PillsAdapter(this, allPills);
			appointmentsListView.Adapter = adapter;



			saveButton.Enabled = false;

			inrDateButton.Click += InrDateButton_Click;
			saveButton.Click += SaveButton_Click;

		}

		private void PillsListView_ItemLongClick(object sender, ItemLongClickEventArgs e)
		{
			Pills myitem = allPills[e.Position];

			LinearLayout l = new LinearLayout(this);

			et = new EditText(this)
			{
				Text = myitem.Quantity.ToString()
			};



			l.AddView(et);

			ad = new AlertDialog.Builder(this);
			ad.SetTitle("Update Quantity value");
			ad.SetView(l); // <----
			ad.SetPositiveButton("Update", (senderAlert, args) =>
			{
				//CreateScheme();
				UpdateQuantity(e.Position, et.Text);
				//Finish();
			});

			ad.SetNegativeButton("Cancel", (senderAlert, args) =>
			{
				//Finish();
			});

			ad.Show();


		}

		private  void UpdateQuantity(int position, string text)
		{

			allPills[position].Quantity = int.Parse(text);
			db.Update(allPills[position]);

			adapter.NotifyDataSetChanged();
		}

		private  void SaveButton_Click(object sender, EventArgs e)
		{
			if (!inrEditText.Text.Any()) return;
			var quantity = int.Parse(inrEditText.Text);
			var pills = new Pills()
			{
				Quantity = quantity,
				Date = date
			};
			db.Insert(pills);
			allPills.Add(pills);
			adapter.NotifyDataSetChanged();

		}

		private void InrDateButton_Click(object sender, EventArgs e)
		{
			DatePickerDialog datePickerDialog = new DatePickerDialog(this);
			datePickerDialog.DateSet += DatePickerDialog_DateSet;
			datePickerDialog.Show();

		}

		private void DatePickerDialog_DateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			date = e.Date;
			inrDateButton.Text = date.ToShortDateString();
			saveButton.Enabled = true;
		}

	}
}