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
            PlansListView.ItemsSource = new List<TrainingPlan>()
            {
                new TrainingPlan5K(),
                new TrainingPlan() { Type = PlanType.TenK, Title = "10k Training", Image = "plan_10k.jpg", Caption = "Think you can handle 6.2 miles? No time like the present!" },
                new TrainingPlan() { Type = PlanType.HalfMarathon, Title = "Half Marathon Training", Image = "plan_half.jpg", Caption = "Step up to a full half marathon. You can do this!" }
            };
            PlansListView.ItemSelected += PlansListView_ItemSelected;
        }

        private async void PlansListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PlansListView.SelectedItem = null;
            if (e.SelectedItem is TrainingPlan plan)
            {
                await Navigation.PushAsync(new PlanDetailPage(plan));
            }
        }
    }
}