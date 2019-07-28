using Dash.Forms.Extensions;
using Dash.Forms.Helpers;
using Dash.Forms.Helpers.Storage;
using SkiaSharp;
using Xamarin.Cross.Charts;
using Xamarin.Cross.Charts.Charts;
using Xamarin.Cross.Charts.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MetricsPage : ContentPage
    {
        public static BindableProperty TotalTimeProperty = BindableProperty.Create(nameof(TotalTime), typeof(string), typeof(MetricsPage), "00:00:00");
        public static BindableProperty TotalDistanceProperty = BindableProperty.Create(nameof(TotalDistance), typeof(string), typeof(MetricsPage), "0");
        public string TotalTime
        {
            get { return (string)GetValue(TotalDistanceProperty); }
            set { SetValue(TotalDistanceProperty, value); }
        }
        public string TotalDistance
        {
            get { return (string)GetValue(TotalDistanceProperty); }
            set { SetValue(TotalDistanceProperty, value); }
        }
        public string DistanceType { get; set; }
        private readonly UnitsType Type;

        public MetricsPage()
        {
            BindingContext = this;

            Type = PreferenceHelper.GetUnits();
            DistanceType = Type.GetDistanceUnitString();

            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var runDataStore = new RunDataStorageHelper();
            TotalDistance = runDataStore.GetTotalDistance().ConvertMeters(Type).ToString("N1");
            var totalElapsed = runDataStore.GetTotalElapsed();
            TotalTime = totalElapsed.ToString(totalElapsed.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");

            ChartView.Chart = new LineChart()
            {
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelTextSize = 60f,
                Inputs = new[]
                {
                    new ChartInput(200)
                    {
                        Label = "January",
                        DisplayValue = "200",
                        Color = SKColor.Parse("#266489")
                    },
                    new ChartInput(400)
                    {
                        Label = "February",
                        DisplayValue = "400",
                        Color = SKColor.Parse("#68B9C0")
                    },
                    new ChartInput(-100)
                    {
                        Label = "March",
                        DisplayValue = "-100",
                        Color = SKColor.Parse("#90D585")
                    }
                }
            };
        }
    }
}