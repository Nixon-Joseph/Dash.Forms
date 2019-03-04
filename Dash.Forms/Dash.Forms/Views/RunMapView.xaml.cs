using Dash.Forms.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunMapView : ContentView
    {
        public RunMapView()
        {
            InitializeComponent();
        }

        public CustomMap GetMap()
        {
            return RunMap;
        }
    }
}