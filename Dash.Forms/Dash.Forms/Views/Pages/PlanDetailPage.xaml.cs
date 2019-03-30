using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlanDetailPage : ContentPage
    {
        public PlanDetailPage(Plan plan)
        {
            InitializeComponent();
            BindingContext = plan;
        }
    }
}