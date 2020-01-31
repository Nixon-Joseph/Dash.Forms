using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Helpers;
using System;
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

            ButtonTest2.Clicked += ButtenTest2_Clicked;

            MessagingCenter.Subscribe<string, string>(string.Empty, Constants.DroidAppWearMessageSentToHandheld, (sender, message) => {
                DependencyService.Get<IMessageService>().ShortToast("Wear sent a message to handheld.");
            });
        }

        private void ButtenTest2_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IMessageService>().ShortToast("Attempt to open wear.");
            MessagingCenter.Send(string.Empty, Constants.OpenWearApp);
        }

        private void ButtonTest_Clicked(object sender, System.EventArgs e)
        {
            DependencyService.Get<IMessageService>().ShortToast("Sent test message to wear.");
            WearHelper.SendMessageToWear("This is a test message!");
        }
    }
}