using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanDetailPage : ContentPage
    {
        private TrainingPlan Plan;
        private IEnumerable<PlanWeekContainer> Weeks;

        public PlanDetailPage(TrainingPlan plan)
        {
            InitializeComponent();
            Plan = plan;
            BindingContext = plan;

            Weeks = plan.Weeks.Select(w => new PlanWeekContainer(w));
            PlanList.ItemsSource = Weeks;
            PlanList.ItemSelected += PlanList_ItemSelected;
        }

        private async void PlanList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PlanList.SelectedItem = null;
            if (e.SelectedItem is TrainingDay day)
            {
                if (await DisplayAlert("Start Run?", $"Would you like to start this run?", "Lets go!", "Nope") == true)
                {
                    var week = Weeks.FirstOrDefault(w => w.Contains(day));
                    await Navigation.PushAsync(new RunTabbedPage(day, Plan.Id, week?.WeekNumber ?? 0));
                }
            }
        }

        private class PlanWeekContainer : List<PlanDayContainer>
        {
            public PlanWeekContainer(TrainingWeek week)
            {
                WeekNumber = week.WeekNumber;
                AddRange(week.Days.Select(d => new PlanDayContainer(d)));
            }

            public int WeekNumber { get; set; }
        }

        private class PlanDayContainer : TrainingDay
        {
            public PlanDayContainer(TrainingDay day)
            {
                DayNumber = day.DayNumber;
                Segments = day.Segments;
                TrainingType = day.TrainingType;
            }

            public string Summary { get { return $"Total Time: {Segments.Sum(s => s.Value)} Minutes"; } }
        }
    }
}