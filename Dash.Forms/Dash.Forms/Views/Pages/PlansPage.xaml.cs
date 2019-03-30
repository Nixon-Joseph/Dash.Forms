using Dash.Forms.Helpers;
using Dash.Forms.Models.Run;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlansPage : ContentPage
    {
        public PlansPage()
        {
            InitializeComponent();
            PlansListView.ItemsSource = new List<Plan>()
            {
                new Plan() { Title = "5k Training", Image = "plan_5k.jpg", Caption = "3.1 miles. Wherever you want, as fast as you can!" },
                new Plan() { Title = "10k Training", Image = "plan_10k.jpg", Caption = "Think you can handle 6.2 miles? No time like the present!" },
                new Plan() { Title = "Half Marathon Training", Image = "plan_half.jpg", Caption = "Step up to a full half marathon. You can do this!" }
            };
            PlansListView.ItemSelected += PlansListView_ItemSelected;
        }

        private async void PlansListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PlansListView.SelectedItem = null;
            if (e.SelectedItem is Plan plan)
            {
                await NavigationHelper.PushAsync(new PlanDetailPage(plan));
            }
        }
    }
}