
using Android.App;
using Android.Content.PM;
using Android.OS;
using Dash.Forms.Droid.DependencyServices;
using System;

namespace Dash.Forms.Droid
{
    [Activity(Label = "Dash.Forms", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Android.Content.Context _Context { get; private set; }
        public static Activity _ { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            _Context = BaseContext;
            _ = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += HandleExceptions;

            global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental", "FastRenderers_Experimental");
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            var lService = new LocationService_Droid();
            lService.CheckGPSPermission();
        }

        private void HandleExceptions(object sender, UnhandledExceptionEventArgs e)
        {
            var thing = e.ExceptionObject;
        }
    }
}