using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            Title = "Dash";

            InitializeComponent();
        }
    }
}