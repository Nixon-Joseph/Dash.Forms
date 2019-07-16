using Android.App;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Views;
using AutoMapper;
using Dash.Forms.Droid.DependencyServices;
using Dash.Forms.Models.Run;
using System;
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
        public static IMapper Mapper { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.MainTheme);

            Instance = Instance ?? this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            try
            {
                Mapper = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Location, LocationData>()
                        .AfterMap((s, d) => d.Timestamp = DateTime.UtcNow.Ticks);
                }).CreateMapper();
            }
            catch (Exception ex)
            {
                var thing = ex.Message;
            }

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
            var thing = e.ExceptionObject;
        }
    }
}