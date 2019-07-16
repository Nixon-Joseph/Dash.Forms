using Dash.Forms.Extensions;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace Dash.Forms
{
    public partial class App : Application
    {

        public App(string androidIntentDataLastPathSegment = null)
        {
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
            AppCenter.Start("android=c4130136-3e17-463f-a147-626b41dff809;",// +
                              //"ios={Your iOS App secret here}",
                              typeof(Analytics), typeof(Crashes));
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
