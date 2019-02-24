using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.Annotation;
using Android.Support.V4.App;
using Android.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Resource;

namespace Dash.Forms.Droid.Services
{
    [Service]
    public class LocationService : Service, ILocationListener
    {
        static readonly string TAG = typeof(LocationService).FullName;
        protected LocationManager LocMgr = Android.App.Application.Context.GetSystemService("location") as LocationManager;
        private bool isStarted;
        LocationServiceBinder binder;

        public override void OnCreate()
        {
            base.OnCreate();
            Log.Info(TAG, "OnCreate: the service is initializing.");
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            switch (intent.Action)
            {
                case Constants.Action.START_SERVICE:
                    if (isStarted)
                    {
                        Log.Info(TAG, "OnStartCommand: The service is already running.");
                    }
                    else
                    {
                        Log.Info(TAG, "OnStartCommand: The service is starting.");
                        RegisterForegroundService();
                        StartLocationUpdates();
                        isStarted = true;
                    }
                    break;
                case Constants.Action.STOP_SERVICE:
                    Log.Info(TAG, "OnStartCommand: The service is stopping.");
                    StopLocationUpdates();
                    StopForeground(true);
                    StopSelf();
                    isStarted = false;
                    break;
                case Constants.Action.SHOW_PAUSE:
                    NotificationManagerCompat.From(this).Notify(Constants.LOCATION_SERVICE_RUNNING_NOTIFICATION_ID, BuildNotification(true, BuildNotificationChannelId()));
                    break;
                case Constants.Action.SHOW_RESUME:
                    NotificationManagerCompat.From(this).Notify(Constants.LOCATION_SERVICE_RUNNING_NOTIFICATION_ID, BuildNotification(false, BuildNotificationChannelId()));
                    break;
            }

            // This tells Android not to restart the service if it is killed to reclaim resources.
            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            // We need to shut things down.
            Log.Info(TAG, "OnDestroy: The started service is shutting down.");

            // Remove the notification from the status bar.
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(Constants.LOCATION_SERVICE_RUNNING_NOTIFICATION_ID);

            isStarted = false;
            base.OnDestroy();
        }

        void RegisterForegroundService()
        {
            var notification = BuildNotification(true, BuildNotificationChannelId());

            // Enlist this instance of the service as a foreground service
            StartForeground(Constants.LOCATION_SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }
         
        private string BuildNotificationChannelId()
        {
            string channelId = string.Empty;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                channelId = CreateNotificationChannelId("location_service", "Location Service");
            }
            return channelId;
        }

        private Notification BuildNotification(bool showPause, string channelId)
        {
            return new NotificationCompat.Builder(this, channelId)
                .SetContentTitle(Resources.GetString(Resource.String.app_name))
                .SetContentText(Resources.GetString(Resource.String.notification_text))
                .SetSmallIcon(Resource.Drawable.ic_directions_run_black_18dp)
                .SetContentIntent(BuildIntentToShowRunActivity())
                .SetPriority((int)NotificationPriority.Low)
                .SetOngoing(true)
                //.AddAction(BuildEndRunAction())
                //.AddAction(showPause ? BuildPauseRunAction() : BuildResumeRunAction())
                .Build();
        }

        [RequiresApi(Value = (int)Android.OS.BuildVersionCodes.O)]
        private string CreateNotificationChannelId(string channelId, string channelName)
        {
            var chan = new NotificationChannel(channelId, channelName, NotificationImportance.High)
            {
                LightColor = Color.HoloBlueDark,
                LockscreenVisibility = NotificationVisibility.Public
            };
            var service = GetSystemService(NotificationService) as NotificationManager;
            service.CreateNotificationChannel(chan);
            return channelId;
        }

        /// <summary>
        /// Builds a PendingIntent that will display the main activity of the app. This is used when the 
        /// user taps on the notification; it will take them to the run activity of the app.
        /// </summary>
        /// <returns>The content intent.</returns>
        PendingIntent BuildIntentToShowRunActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetAction(Constants.Action.RUN_ACTIVITY);
            notificationIntent.SetFlags(ActivityFlags.SingleTop);
            notificationIntent.PutExtra(Constants.SERVICE_STARTED_KEY, true);

            var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent, PendingIntentFlags.UpdateCurrent);
            return pendingIntent;
        }

        public override IBinder OnBind(Intent intent)
        {
            try
            {
                if (intent != null)
                {
                    binder = new LocationServiceBinder(this);
                    return binder;
                }
            }
            catch (Exception ex) { }
            return null;
        }

        public void StartLocationUpdates()
        {
            try
            {
                var locationCriteria = new Criteria
                {
                    Accuracy = Accuracy.Fine,
                    PowerRequirement = Power.High
                };

                var locationProvider = LocMgr.GetBestProvider(locationCriteria, true);

                LocMgr.RequestLocationUpdates(locationProvider, Constants.LOCATION_MIN_UPDATE_FREQUENCY, 0, this);
            }
            catch (Exception ex) { }
        }

        public void StopLocationUpdates()
        {
            try
            {
                LocMgr.RemoveUpdates(this);
            }
            catch (Exception ex) { }
        }

        //private NotificationCompat.Action BuildEndRunAction()
        //{
        //	var intent = new Intent(this, typeof(RunActionReceiver));
        //	intent.SetAction(Constants.Action.END_RUN);
        //	var pendingIntent = PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.CancelCurrent);

        //	var builder = new NotificationCompat.Action.Builder(Resource.Drawable.ic_stop_black_18dp,
        //												GetText(Resource.String.end_run_notification_action),
        //												pendingIntent);

        //	return builder.Build();
        //}

        //private NotificationCompat.Action BuildPauseRunAction()
        //{
        //	var intent = new Intent(this, typeof(RunActionReceiver));
        //	intent.SetAction(Constants.Action.PAUSE_RUN);
        //	var pendingIntent = PendingIntent.GetBroadcast(this, 1, intent, PendingIntentFlags.CancelCurrent);

        //	var builder = new NotificationCompat.Action.Builder(Resource.Drawable.ic_pause_black_18dp,
        //												GetText(Resource.String.pause_run_notification_action),
        //												pendingIntent);

        //	return builder.Build();
        //}

        //private NotificationCompat.Action BuildResumeRunAction()
        //{
        //	var intent = new Intent(this, typeof(RunActionReceiver));
        //	intent.SetAction(Constants.Action.RESUME_RUN);
        //	var pendingIntent = PendingIntent.GetBroadcast(this, 2, intent, PendingIntentFlags.CancelCurrent);

        //	var builder = new NotificationCompat.Action.Builder(Resource.Drawable.ic_play_arrow_black_18dp,
        //												GetText(Resource.String.resume_run_notification_action),
        //												pendingIntent);

        //	return builder.Build();
        //}

        #region ILocationListener implementation


        public void OnLocationChanged(Android.Locations.Location location)
        {
            try
            {
                //this.LocationChanged(this, new LocationChangedEventArgs(location));
                Intent intent = new Intent(Constants.Action.LOCATION_CHANGED);
                intent.PutExtra(Constants.Extra.LOCATION_DATA, location);
                SendBroadcast(intent);
            }
            catch (Exception ex) { }
        }

        public void OnProviderDisabled(string provider)
        {
            //this.ProviderDisabled(this, new ProviderDisabledEventArgs(provider));
        }

        public void OnProviderEnabled(string provider)
        {
            //this.ProviderEnabled(this, new ProviderEnabledEventArgs(provider));
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            //this.StatusChanged(this, new StatusChangedEventArgs(provider, status, extras));
        }

        #endregion
    }

    public class LocationServiceBinder : Binder
    {
        readonly LocationService service;

        public LocationServiceBinder(LocationService service)
        {
            try
            {
                this.service = service;
            }
            catch (Exception ex) { }
        }

        public LocationService GetLocationServiceBinder()
        {
            try
            {
                return service;
            }
            catch (Exception ex) { return null; }
        }
    }
}
