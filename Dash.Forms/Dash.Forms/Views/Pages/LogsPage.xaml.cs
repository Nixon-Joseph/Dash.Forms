using Dash.Forms.Helpers.Storage;
using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogsPage : ContentPage
    {
        public IEnumerable<LogGroup> Logs;

        public LogsPage()
        {
            InitializeComponent();
            var runData = new RunDataStorageHelper().GetAll();
            try
            {
                var logs = runData.GroupBy(r => r.TrainingProgramId).Select(g => new LogGroup(GetPlanTitle(g.Key), g.OrderByDescending(r => r.Start)));
                Logs = logs;
            }
            catch (Exception ex)
            {
                var thing = ex.Message;
            }
            LogListView.ItemsSource = Logs;
            LogListView.ItemSelected += LogListView_ItemSelected;
        }

        private string GetPlanTitle(string key)
        {
            switch (key)
            {
                default:
                    return "Free Run";
            }
        }

        private void LogListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            LogListView.SelectedItem = null;
            if (e.SelectedItem is RunData run)
            {
                Navigation.PushAsync(new LogDetailPage(run));
            }
        }
    }

    public class LogGroup : List<RunData>
    {
        public string Title { get; set; }
        public LogGroup(string title, IEnumerable<RunData> runDatas)
        {
            Title = title;
            AddRange(runDatas);
        }
    }
}