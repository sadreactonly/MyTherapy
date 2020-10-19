using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Button;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MyTherapy.Models;
using Newtonsoft.Json;
using MyTherapy.Activities;

namespace MyTherapy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView todayTherapyText;
        MaterialButton takeTherapyButton;
        TextView lastINR;
        TextView nextAppointment;
        TextView daysLeftTextView;

        private AppManager appManager; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
           
            todayTherapyText = FindViewById<TextView>(Resource.Id.textView2);
            takeTherapyButton = FindViewById<MaterialButton>(Resource.Id.materialButton1);
            lastINR = FindViewById<TextView>(Resource.Id.textView5);
            nextAppointment = FindViewById<TextView>(Resource.Id.textView6);
            daysLeftTextView = FindViewById<TextView>(Resource.Id.daysLeft);

            takeTherapyButton.Click += TakeTherapyButton_Click;

            appManager = new AppManager(this);
			appManager.TherapyTaken += AppManager_TherapyTaken;       
        }

		private void AppManager_TherapyTaken(object sender, EventArgs e)
		{
            takeTherapyButton.Text = "Taken";
            takeTherapyButton.Enabled = false;
        }

		private void TakeTherapyButton_Click(object sender, EventArgs e)
		{
            var todayTherapy = appManager.GetTodayTherapy();
            todayTherapy.IsTaken = true;
            appManager.TakeTherapy(todayTherapy);
        }

		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
	        int id = item.ItemId;
	        switch (id)
	        {
                case Resource.Id.action_schema:
                    StartActivity(typeof(SchemaActivity));
                    break;
                case Resource.Id.action_inr:                
			        StartActivity(typeof(InrActivity));
			        break;
		        case Resource.Id.action_therapies:
			        StartActivity(typeof(AllTherapiesActivity));
			        break;
                case Resource.Id.action_pills:
                    StartActivity(typeof(PillsActivity));
                    break;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
			appManager.SetAllData(out string lastInrText, out string nextAppointmentText, out string todayTherapyTextText, out bool takeTherapyButtonEnabled, out string daysLeft);

			lastINR.Text = lastInrText;
            nextAppointment.Text = nextAppointmentText;
            todayTherapyText.Text = todayTherapyTextText;
            takeTherapyButton.Enabled = !takeTherapyButtonEnabled;
            daysLeftTextView.Text = daysLeft;
            base.OnResume();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}