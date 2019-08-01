using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            Title = "Dash";

            InitializeComponent();

            ButtonTest.Clicked += ButtonTest_Clicked;
        }

        private void ButtonTest_Clicked(object sender, System.EventArgs e)
        {
            DependencyService.Get<IMessageService>().ShortToast("Sent test message to wear.");
            WearHelper.SendMessageToWear("This is a test message!");
        }
    }
}