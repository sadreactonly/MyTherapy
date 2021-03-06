﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MyTherapy.Service
{
	[BroadcastReceiver]
	public class TherapyReminderReciever : BroadcastReceiver
	{
        private AppManager appManager = new AppManager();
        public const string URGENT_CHANNEL = "com.companyname.mytherapy.urgent";
        public const int NOTIFY_ID = 1100;
        public override void OnReceive(Context context, Intent intent)
        {
            var therapy = appManager.GetTodayTherapy();
			if (therapy != null) { 
                var mess = intent.GetStringExtra("message");
                var title = intent.GetStringExtra("title");
            
                string message = string.Format(mess, therapy.Dose);

                var importance = NotificationImportance.High;
                NotificationChannel chan = new NotificationChannel(URGENT_CHANNEL, "Urgent", importance);
                chan.EnableVibration(true);
                chan.LockscreenVisibility = NotificationVisibility.Public;

                var resultIntent = new Intent(context, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                var pendingIntent = PendingIntent.GetActivity(context, 0, resultIntent, PendingIntentFlags.UpdateCurrent);

                var notificationBuilder = new Notification.Builder(context)
                    .SetSmallIcon(Resource.Drawable.medicine)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetContentIntent(pendingIntent)
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                    .SetAutoCancel(true)
                    .SetChannelId(URGENT_CHANNEL);

                NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
                notificationManager.CreateNotificationChannel(chan);

                notificationManager.Notify(NOTIFY_ID, notificationBuilder.Build());
            }
        }
    }
}