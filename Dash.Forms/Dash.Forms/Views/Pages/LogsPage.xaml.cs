using Dash.Forms.Helpers.Storage;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogsPage : ContentPage
    {
        public IEnumerable<LogGroup> Logs;// = new List<LogGroup>() {
        //    new LogGroup("Week 1", "1") { new Log(), new Log() },
        //    new LogGroup("Week 2", "2") { new Log(), new Log() },
        //    new LogGroup("Week 3", "3") { new Log() },
        //    new LogGroup("Week 4", "4") { new Log(), new Log() },
        //    new LogGroup("Week 5", "5") { new Log(), new Log() },
        //    new LogGroup("Week 6", "6") { new Log(), new Log() },
        //    new LogGroup("Week 7", "7") { new Log(), new Log() }
        //};

        public LogsPage()
        {
            InitializeComponent();
            var runData = new RunDataStorageHelper().GetAll();
            Logs = runData.Select(r => new LogGroup(r.Start.ToShortDateString(), ""));
            LogListView.ItemsSource = Logs;
            LogListView.ItemSelected += LogListView_ItemSelected;
        }

        private void LogListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            LogListView.SelectedItem = null;
        }
    }

    public class LogGroup : List<Log>
    {
        public string Title { get; set; }
        public string ShortName { get; set; }
        public LogGroup(string title, string shortName)
        {
            Title = title;
            ShortName = shortName;
        }
    }

    public class Log
    {

    }
}