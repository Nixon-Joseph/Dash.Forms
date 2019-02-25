using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Models;
using Dash.Forms.Views.Pages;
using System;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppNav : MasterDetailPage
    {
        private bool _toastOpen = false;

        public AppNav()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        protected override bool OnBackButtonPressed()
        {
            if ((Detail as NavigationPage).CurrentPage.GetType() == typeof(HomePage) && _toastOpen == false)
            {
                DependencyService.Get<IMessageService>().ShortToast("Press the back button once more to exit.");
                _toastOpen = true;
                Timer timer = new Timer((scope) => { _toastOpen = false; }, this, 3000, Timeout.Infinite);
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is IAppNavMenuItem item)
            {
                var page = (Page)Activator.CreateInstance(item.TargetType);
                MasterPage.ListView.SelectedItem = null;
                page.Title = item.Title;

                await (Detail as NavigationPage).PushAsync(page);
                IsPresented = false;
            }
            else
            {
                MasterPage.ListView.SelectedItem = null;
            }
        }
    }
}