using Android.App;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Views;
using AutoMapper;
using Dash.Forms.Droid.DependencyServices;
using Dash.Forms.Models.Run;
using System;

namespace Dash.Forms.Droid
{
    [Activity(Label = "Dash.Forms", Theme = "@style/MainTheme.Launcher", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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

            try
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Location, LocationData>();
                });
            }
            catch (Exception ex)
            {
                var thing = ex.Message;
            }

            base.OnCreate(savedInstanceState);

            AppDomain.CurrentDomain.UnhandledException += HandleExceptions;

            global::Xamarin.Forms.Forms.SetFlags(/*"Shell_Experimental", *//*"Visual_Experimental", */"CollectionView_Experimental", "FastRenderers_Experimental");
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            XF.Material.Droid.Material.Init(this, savedInstanceState);
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