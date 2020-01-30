using Dash.Forms.Controls;
using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


//https://github.com/sthewissen/KickassUI.ParallaxCarousel
namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlansPage : ContentPage
    {
        //private int _currentIndex;
        private List<Color> _backgroundColors = new List<Color>();
        private int _backgroundCount;
        private double _pageSizePercent;

        public Wrapper Wrapper { get; set; }

        public PlansPage()
        {
            InitializeComponent();

            Wrapper = new Wrapper
            {
                Items = new List<CarouselItem>()
                {
                    new CarouselItem{ Plan = new TrainingPlan5K(), Position=0, Type="5K Training", Title = "5K", BackgroundColor= Color.FromHex("#9866d5"), StartColor=Color.FromHex("#f3463f"),  EndColor=Color.FromHex("#fece49")},
                    new CarouselItem{ Plan = new TrainingPlan10K(), Position=0, Type="10K Training", Title = "10K", BackgroundColor= Color.FromHex("#fab62a"), StartColor=Color.FromHex("#42a7ff"),  EndColor=Color.FromHex("#fab62a")},
                    new CarouselItem{ Plan = new TrainingPlan() { Type = PlanType.HalfMarathon, Title = "Half Marathon Training", Image = "plan_half", Caption = "Step up to a full half marathon. You can do this!" }, Position=0, Type="Half Marathon Training", Title = "13.1", BackgroundColor= Color.FromHex("#425cfc"), StartColor=Color.FromHex("#33ccf3"),  EndColor=Color.FromHex("#ccee44")}
                }
            };

            this.BindingContext = Wrapper;

            // Create out a list of background colors based on our items colors so we can do a gradient on scroll.
            for (int i = 0; i < Wrapper.Items.Count; i++)
            {
                var current = Wrapper.Items[i];
                var next = Wrapper.Items.Count > i + 1 ? Wrapper.Items[i + 1] : null;

                if (next != null)
                {
                    _backgroundColors.AddRange(SetGradients(current.BackgroundColor, next.BackgroundColor, 100));
                }
                else
                {
                    _backgroundColors.Add(current.BackgroundColor);
                }
            }

            _backgroundCount = _backgroundColors.Count();
            _pageSizePercent = 100d / (double)_backgroundCount;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Need to start somewhere...
            BackgroundColor = _backgroundColors.First();
        }

        //private double lastScroll = 0;
        private void CarouselLayout_Scrolled(object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            if (sender is CarouselLayout layout)
            {
                var totalPercent = e.ScrollX / (layout.ContentSize.Width - layout.Width);
                //Debug.WriteLine($"totalPercent: {totalPercent}, lowerIndex: {lowerIndex}, upperIndex: {upperIndex}, pageSizePercent, {pageSizePercent}, pageMod: {pageMod}, percentThroughLower: {percentThroughLower}");
                var bgIndex = Math.Max(0, Math.Min((int)(_backgroundCount * totalPercent), _backgroundCount - 1));
                BackgroundColor = _backgroundColors[bgIndex];

                int lowerIndex = (int)(totalPercent / _pageSizePercent);
                var percentThroughLower = ((totalPercent % _pageSizePercent) / _pageSizePercent * 100d);
                Wrapper.Items[lowerIndex].Position = -percentThroughLower;
                int upperIndex = lowerIndex + 1;
                if (upperIndex < Wrapper.Items.Count())
                {
                    Wrapper.Items[upperIndex].Position = 100 - percentThroughLower;
                }
            }
        }

        // Create a list of all the colors in between our start and end color.
        public static IEnumerable<Color> SetGradients(Color start, Color end, int steps)
        {
            var colorList = new List<Color>();

            double aStep = ((end.A * 255) - (start.A * 255)) / steps;
            double rStep = ((end.R * 255) - (start.R * 255)) / steps;
            double gStep = ((end.G * 255) - (start.G * 255)) / steps;
            double bStep = ((end.B * 255) - (start.B * 255)) / steps;

            for (int i = 0; i < 100; i++)
            {
                var a = (start.A * 255) + (int)(aStep * i);
                var r = (start.R * 255) + (int)(rStep * i);
                var g = (start.G * 255) + (int)(gStep * i);
                var b = (start.B * 255) + (int)(bStep * i);

                colorList.Add(Color.FromRgba(r / 255, g / 255, b / 255, a / 255));
            }

            return colorList;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PlanDetailPage(Wrapper.CurrentItem.Plan));
        }

        //private async void PlansListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    PlansListView.SelectedItem = null;
        //    if (e.SelectedItem is TrainingPlan plan)
        //    {
        //        await Navigation.PushAsync(new PlanDetailPage(plan));
        //    }
        //}
    }

    public class CarouselItem : INotifyPropertyChanged
    {
        public string Title { get; set; }
        //public int Price { get; set; }
        public string Name { get { return Plan.Title; } }
        public Color StartColor { get; set; }
        public Color EndColor { get; set; }
        public Color BackgroundColor { get; set; }
        public string Type { get; set; }
        public string ImageSrc { get { return Plan.Image; } }
        public int Rotation { get; set; }
        public string Description { get { return Plan.Caption; } }

        double _position;
        public event PropertyChangedEventHandler PropertyChanged;

        public double Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    OnPropertyChanged();
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TrainingPlan Plan { get; set; }
    }

    public class Wrapper : BindableObject
    {
        public static BindableProperty CurrentIndexProperty = BindableProperty.Create(nameof(CurrentIndex), typeof(int), typeof(Wrapper), 0);
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }

        public CarouselItem CurrentItem
        {
            get { return Items?[CurrentIndex]; }
        }

        public List<CarouselItem> Items { get; set; } = new List<CarouselItem>();
    }
}