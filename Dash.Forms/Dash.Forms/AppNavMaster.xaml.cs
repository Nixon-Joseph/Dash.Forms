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
                    new AppNavMenuItem<HomePage> { Title = "Home" },
                    new AppNavMenuItem<RunTabbedPage> { Title = "Free Run" },
                    new AppNavMenuItem<LogsPage> { Title = "Logs" },
                    new AppNavMenuItem<MetricsPage> { Title = "Metrics" },
                    new AppNavMenuItem<SettingsPage> { Title = "Settings" },
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