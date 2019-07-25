using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Dash.Forms.Droid.DependencyServices;
using System;
using System.Collections.Generic;
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

        }

        public override void OnBackPressed()
        {
            Material.HandleBackButton(base.OnBackPressed);
        }

        private void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            Microsoft.AppCenter.Crashes.Crashes.TrackError(e.ExceptionObject as Exception, new Dictionary<string, string>() { { "Type", "Unhandled" } });
        }
    }
}