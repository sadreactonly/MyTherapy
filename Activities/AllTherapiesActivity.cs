using Android.App;
using Android.OS;
using Android.Widget;
using MyTherapy.Models;
using System.Collections.Generic;

namespace MyTherapy.Activities
{
	[Activity(Label = "AllTherapiesActivity")]
	public class AllTherapiesActivity : Activity
	{
        private AppManager appManager;

        ListView listView;
		SchemaAdapter adapter;
        List<DailyTherapy> allTherapies;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			SetContentView(Resource.Layout.manage_therapies);
			base.OnCreate(savedInstanceState);

			listView = FindViewById<ListView>(Resource.Id.mainlistview);
			listView.ItemLongClick += ListView_ItemLongClick;
            appManager = new AppManager(this);
		}

		protected override void OnResume()
		{
			base.OnResume();
            allTherapies = appManager.GetTherapies();
            adapter = new SchemaAdapter(this, allTherapies);
            listView.Adapter = adapter;

        }
        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {

            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            DailyTherapy item = adapter.GetFromItem(e.Position);
             


            alert.SetTitle($"{ item.Date.ToShortDateString()} therapy");
            alert.SetMessage("Manage selected therapy");

            alert.SetPositiveButton("Delete", (senderAlert, args) =>
            {
                appManager.DeleteTherapy(item);
                adapter.RemoveItemAt(e.Position);
                adapter.NotifyDataSetChanged();
            });

            alert.SetNegativeButton("Cancel", (senderAlert, args) =>
            {

            });

            alert.SetNeutralButton("Take/Untake", (senderAlert, args) =>
            {
                allTherapies[e.Position].IsTaken = !item.IsTaken;
                appManager.UpdateTherapy(item);
                adapter.NotifyDataSetChanged();
            });

            Dialog dialog = alert.Create();
            dialog.Show();

        }

    }
}