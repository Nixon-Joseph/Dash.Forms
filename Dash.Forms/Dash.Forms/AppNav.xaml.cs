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
            (Detail as NavigationPage).Popped += AppNav_Changed;
            (Detail as NavigationPage).Pushed += AppNav_Changed;
        }

        private void AppNav_Changed(object sender, NavigationEventArgs e)
        {
            if (sender is NavigationPage navPage)
            {
                IsGestureEnabled = IsRunPage(navPage.CurrentPage.GetType()) == false;
            }
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

        private bool IsRunPage(Type pageType)
        {
            return pageType == typeof(RunPage) || pageType == typeof(RunTabbedPage);
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