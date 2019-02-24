using Dash.Forms.DependencyInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunPage : ContentPage
    {
        public RunPage()
        {
            InitializeComponent();
        }

        private void Init()
        {
            var service = DependencyService.Get<ILocationService>();
            var hasPermission = service.CheckGPSPermission();
            if (hasPermission == true)
            {
                var map = new Map(MapSpan.FromCenterAndRadius(new Position(37, -122), Distance.FromMiles(0.3)))
                {
                    IsShowingUser = true,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                /*MapContainer.*/Content = map;
            }
        }
    }
}