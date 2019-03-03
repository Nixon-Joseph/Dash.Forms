using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Extensions;
using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunPage : ContentPage
    {
        private readonly ILocationService _locationService;
        private readonly DateTime _startTime;
        private readonly Timer _timer;
        private readonly TimeSpan _pauseOffset = new TimeSpan();
        private readonly List<LocationData> _locations;
        private bool _isTracking = true;
        private bool _justUnpaused = false;
        private bool _hasStoppedService = false;
        private double _totalDistance = 0d;

        public RunPage()
        {
            //NavigationPage.SetHasNavigationBar(this, false);

            InitializeComponent();
            //_locationService = DependencyService.Get<ILocationService>();
            //_locationService.AddLocationChangedListener(_locationService_LocationChanged);
            //_locationService.Start();

            //StopButton.Clicked += StopButton_Clicked;
            //PauseButton.Clicked += PauseButton_Clicked;

            _timer = new Timer() { Interval = 1000 };
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            _startTime = DateTime.Now;

            _locations = new List<LocationData>();

            RunCarousel.ItemsSource = new List<View>()
            {
                new RunMapView(),
                new RunStatsView()
            };
        }

        ~RunPage()
        {
            if (_hasStoppedService == false)
            {
                StopLocationService();
            }
        }

        private void PauseButton_Clicked(object sender, System.EventArgs e)
        {
            _isTracking = !_isTracking;
            if (_isTracking == true)
            {
                _justUnpaused = true;
                //PauseButton.Text = "Pause";
            }
            else
            {
                //PauseButton.Text = "Resume";
            }
            //RunMap.HasScrollEnabled = !_isTracking;
        }

        private async void StopButton_Clicked(object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Cancel Run?", "Are you sure you want to end your run?", "Yes", "No"))
            {
                if (_hasStoppedService == false) // should always be true here I think.
                {
                    StopLocationService();
                    await Navigation.PopAsync();
                }
            }
        }

        private void StopLocationService()
        {
            _hasStoppedService = true;
            _locationService.RemoveLocationChangedListener(_locationService_LocationChanged);
            _locationService.Stop();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_isTracking == true)
            {
                var spent = DateTime.Now - (_startTime + _pauseOffset);
                Device.BeginInvokeOnMainThread(() =>
                {
                    //TimeElapsedLabel.Text = spent.ToString(spent.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");
                });
            }
            else
            {
                _pauseOffset.Add(TimeSpan.FromSeconds(1));
            }
        }

        protected override void OnAppearing()
        {

        }

        private void _locationService_LocationChanged(object sender, Models.Run.LocationData e)
        {
            if (_isTracking == true)
            {
                var newPos = new Position(e.Latitude, e.Longitude);
                if (_locations.Count() > 0 && _locations.Last() is LocationData lastLoc && lastLoc.IsTracked == true)
                {
                    var useMiles = true; // should be a setting later
                    var meters = _locationService.GetDistance(lastLoc.GetPosition(), newPos);
                    _totalDistance += useMiles ? (meters / 1609.344) : (meters / 1000);
                    Device.BeginInvokeOnMainThread(() => {
                        //DistanceGoneLabel.Text = _totalDistance.ToString("N2");
                    });
                }
                if (_justUnpaused == true)
                {
                    _justUnpaused = false;
                    //RunMap.AddPosition(newPos, false);
                }
                //RunMap.AddPosition(newPos);
                //RunMap.MoveToRegion(MapSpan.FromCenterAndRadius(newPos, Distance.FromKilometers(GetMaxDistance() / 1000d)));
            }
            e.IsTracked = _isTracking;
            _locations.Add(e);
        }

        private double GetMaxDistance()
        {
            double minLat = 0, minLng = 0, maxLat = 0, maxLng = 0;
            //foreach (var position in RunMap.RouteCoordinates)
            //{
            //    minLat = Math.Min(minLat, position.Latitude);
            //    minLng = Math.Min(minLng, position.Longitude);
            //    maxLat = Math.Max(maxLat, position.Latitude);
            //    maxLng = Math.Max(maxLng, position.Longitude);
            //}
            return Math.Max(0.1d, _locationService.GetDistance(minLat, minLng, maxLat, maxLng));
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}