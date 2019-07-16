using Dash.Forms.Extensions;
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
