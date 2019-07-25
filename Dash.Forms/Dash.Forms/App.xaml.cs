using Dash.Forms.Extensions;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Dash.Forms
{
    public partial class App : Application
    {

        public App(string androidIntentDataLastPathSegment = null)
        {
            VersionTracking.Track();

            InitializeComponent();
            XF.Material.Forms.Material.Init(this, "Material.Configuration");
            MainPage = new AppNav();

            if (androidIntentDataLastPathSegment.IsNullOrEmpty() == false)
            {
                MessagingCenter.Send(string.Empty, Constants.DroidAppShortcutInvoked, androidIntentDataLastPathSegment);
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=c4130136-3e17-463f-a147-626b41dff809;" + "ios=a5315d74-ee68-430c-a60f-da68a7c4f13e", typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
