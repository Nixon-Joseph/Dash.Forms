using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Wearable;
using Android.OS;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Dash.Forms.AndroidShared;
using Dash.Forms.AndroidShared.Interfaces;
using Dash.Forms.AndroidShared.Receivers;
using Dash.Forms.Droid.DependencyServices;
using Google.Android.Wearable.Intent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IActivityMessageReceiver, CapabilityClient.IOnCapabilityChangedListener
    {
        internal static MainActivity Instance { get; private set; }
        internal static View ContentView { get { return Instance.FindViewById(Android.Resource.Id.Content); } }
        private const string CAPABILITY_WEAR_APP = "verify_remote_dash_wear_app";
        private const string PLAY_STORE_APP_URI = "market://details?id=com.DashFitness.AppBeta";
        private IEnumerable<INode> WearNodesWithApp;
        private IEnumerable<INode> AllConnectedNodes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.MainTheme);

            Instance = Instance ?? this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += HandleExceptions;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            Xamarin.Forms.Forms.SetFlags(/*"Shell_Experimental", *//*"Visual_Experimental", */"CollectionView_Experimental", "FastRenderers_Experimental");
            Xamarin.FormsMaps.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Material.Init(this, savedInstanceState);
            LoadApplication(new App(Intent?.Data?.LastPathSegment));

            var lService = new LocationService_Droid();
            lService.CheckGPSPermission();

            IntentFilter newFilter = new IntentFilter(Intent.ActionSend);
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(new WearableMessageReceiver(this), newFilter);

            Xamarin.Forms.MessagingCenter.Subscribe<string>(string.Empty, Forms.Constants.OpenWearApp, (sender) =>
            {
                OpenPlayStoreOnWearDevice();
            });

            Xamarin.Forms.MessagingCenter.Subscribe<string, string>(string.Empty, Forms.Constants.DroidAppWearMessageSentToWear, async (sender, message) =>
            {
                _ = await NodeManager.SendMessageToNodes(this, message, (ex) =>
                {
                    Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                });
            });
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError(e.Exception);
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError(e.Exception);
        }

        public override void OnBackPressed()
        {
            Material.HandleBackButton(base.OnBackPressed);
        }

        private void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError(e.ExceptionObject as Exception);
        }

        public void OnMessageReceive(string message)
        {
            Xamarin.Forms.MessagingCenter.Send(string.Empty, Dash.Forms.Constants.DroidAppWearMessageSentToHandheld, message);
        }

        protected override void OnPause()
        {
            base.OnPause();

            WearableClass.GetCapabilityClient(this).RemoveListener(this, CAPABILITY_WEAR_APP);
        }

        protected override void OnResume()
        {
            base.OnResume();

            WearableClass.GetCapabilityClient(this).AddListener(this, CAPABILITY_WEAR_APP);

            FindWearDevicesWithApp();

            FindAllWearDevices();
        }

        public void OnCapabilityChanged(ICapabilityInfo capabilityInfo)
        {
            WearNodesWithApp = capabilityInfo.Nodes;
            FindAllWearDevices();
        }

        private async void FindWearDevicesWithApp()
        {
            WearNodesWithApp = (await WearableClass.GetCapabilityClient(this).GetCapabilityAsync(CAPABILITY_WEAR_APP, CapabilityClient.FilterAll))?.Nodes;

            VerifyNode();
        }

        private async void FindAllWearDevices()
        {
            AllConnectedNodes = await NodeManager.GetConnectedNodes(this);
            VerifyNode();
        }

        private void VerifyNode()
        {
            if (WearNodesWithApp != null && AllConnectedNodes != null)
            {

            }
            else if (WearNodesWithApp.Count() > 0)
            {

            }
        }

        private void OpenPlayStoreOnWearDevice()
        {
            if (AllConnectedNodes != null && WearNodesWithApp != null)
            {
                var nodesWithoutApp = new List<INode>();
                foreach (var node in AllConnectedNodes)
                {
                    if (WearNodesWithApp.Contains(node) == false)
                    {
                        nodesWithoutApp.Add(node);
                    }
                }

                if (nodesWithoutApp.Count() > 0)
                {
                    Intent intent = new Intent(Intent.ActionView)
                        .AddCategory(Intent.CategoryBrowsable)
                        .SetData(Android.Net.Uri.Parse(PLAY_STORE_APP_URI));

                    foreach (var node in nodesWithoutApp)
                    {
                        RemoteIntent.StartRemoteActivity(this, intent, new ResultReceiver(new Handler((message) =>
                        {
                            if (message.Arg1 == RemoteIntent.ResultOk)
                            {
                                Toast toast = Toast.MakeText(Application.Context, "Store opened on wear device", ToastLength.Long);
                            }
                            else if (message.Arg1 == RemoteIntent.ResultFailed)
                            {
                                Toast toast = Toast.MakeText(Application.Context, "Can't open store on wear device", ToastLength.Long);
                            }
                            else
                            {
                                throw new Exception("Unexpected result");
                            }
                        })), node.Id);
                    }
                }
            }
        }
    }
}