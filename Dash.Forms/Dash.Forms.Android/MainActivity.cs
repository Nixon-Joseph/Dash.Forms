using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Wearable;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Dash.Forms.Droid.DependencyServices;
using System;
using System.Text;
using XF.Material.Droid;

namespace Dash.Forms.Droid
{
    [Activity(
        Label = "Dash",
        Theme = "@style/MainTheme.Launcher",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Exported = true,
        Name = "com.DashFitness.AppBeta.MainActivity")]
    [MetaData("android.app.shortcuts", Resource = "@xml/shortcuts")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }
        internal static View ContentView { get { return Instance.FindViewById(Android.Resource.Id.Content); } }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.MainTheme);

            Instance = Instance ?? this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += HandleExceptions;

            Xamarin.Forms.Forms.SetFlags(/*"Shell_Experimental", *//*"Visual_Experimental", */"CollectionView_Experimental", "FastRenderers_Experimental");
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Material.Init(this, savedInstanceState);
            LoadApplication(new App(Intent?.Data?.LastPathSegment));

            var lService = new LocationService_Droid();
            lService.CheckGPSPermission();

            IntentFilter newFilter = new IntentFilter(Intent.ActionSend);
            MessageReceiver messageReceiver = new MessageReceiver(this);
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(messageReceiver, newFilter);

            Xamarin.Forms.MessagingCenter.Subscribe<string, string>(string.Empty, Dash.Forms.Constants.DroidAppWearMessageSentToWear, async (sender, message) =>
            {
                try
                {
                    using (var nodes = await WearableClass.GetNodeClient(this).GetConnectedNodesAsync())
                    {
                        foreach (INode node in nodes)
                        {
                            var sendMessageTask = WearableClass.GetMessageClient(this).SendMessage(node.Id, Constants.WEARABLE_MESSAGE_PATH, Encoding.UTF8.GetBytes(message));
                            try
                            {
                                //Block on a task and get the result synchronously//
                                sendMessageTask.Wait();
                                //if the Task fails, then…..//
                            }
                            catch (Exception exception)
                            {
                                //TO DO: Handle the exception//
                            }
                        }
                    }
                }
                catch (Exception exception)
                {

                }
            });
        }

        public override void OnBackPressed()
        {
            Material.HandleBackButton(base.OnBackPressed);
        }

        private void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            var thing = e.ExceptionObject;
        }

        public void OnRecieve(Context context, string message)
        {
            Xamarin.Forms.MessagingCenter.Send(context, message);
        }

        private class MessageReceiver : BroadcastReceiver
        {
            private readonly MainActivity Activity;

            public MessageReceiver(MainActivity activity)
            {
                Activity = activity;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                Xamarin.Forms.MessagingCenter.Send(string.Empty, Dash.Forms.Constants.DroidAppWearMessageSentToHandheld, intent.GetStringExtra("message"));
            }
        }
    }
}