using Dash.Forms.Models;
using Dash.Forms.Views.Pages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppNavMaster : ContentPage
    {
        public ListView ListView;

        public AppNavMaster()
        {
            InitializeComponent();

            BindingContext = new AppNavMasterViewModel();
            ListView = MenuItemsListView;
        }

        class AppNavMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<IAppNavMenuItem> MenuItems { get; set; }

            public AppNavMasterViewModel()
            {
                MenuItems = new ObservableCollection<IAppNavMenuItem>(new IAppNavMenuItem[]
                {
                    new AppNavMenuItem<HomePage> { Title = "Home", Icon = "menu_home.png" },
                    new AppNavMenuItem<RunTabbedPage> { Title = "Free Run", Icon = "menu_run.png" },
                    //new AppNavMenuItem<RunPage> { Title = "Free Run", Icon = "menu_run.png" },
                    new AppNavMenuItem<LogsPage> { Title = "Logs", Icon = "menu_logs.png" },
                    new AppNavMenuItem<PlansPage> { Title = "Plans", Icon = "menu_plan.png" },
                    new AppNavMenuItem<MetricsPage> { Title = "Metrics", Icon = "menu_metrics.png" },
                    new AppNavMenuItem<SettingsPage> { Title = "Settings", Icon = "menu_settings.png" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                {
                    return;
                }

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}