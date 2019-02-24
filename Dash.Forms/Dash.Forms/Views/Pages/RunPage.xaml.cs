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
            InitializeComponent();
            _locationService = DependencyService.Get<ILocationService>();
            _locationService.AddLocationChangedListener(_locationService_LocationChanged);
            _locationService.Start();

            StopButton.Clicked += StopButton_Clicked;
            PauseButton.Clicked += PauseButton_Clicked;

            _timer = new Timer() { Interval = 1000 };
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
            _startTime = DateTime.Now;

            _locations = new List<LocationData>();
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
                PauseButton.Text = "Pause";
            }
            else
            {
                PauseButton.Text = "Resume";
            }
            RunMap.HasScrollEnabled = !_isTracking;
        }

        private void StopButton_Clicked(object sender, System.EventArgs e)
        {
            if (_hasStoppedService == false) // should always be true here I think.
            {
                StopLocationService();
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
                    TimeElapsedLabel.Text = spent.ToString(spent.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");
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
                    var meters = _locationService.GetDistance(lastLoc.GetPosition(), newPos);
                    var useMiles = true; // should be a setting later
                    _totalDistance += useMiles ? (meters / 1609.344) : (meters / 1000);
                    Device.BeginInvokeOnMainThread(() => {
                        DistanceGoneSpan.Text = _totalDistance.ToString("N2");
                    });
                }
                if (_justUnpaused == true)
                {
                    _justUnpaused = false;
                    RunMap.AddPosition(newPos, false);
                }
                RunMap.AddPosition(newPos);
                RunMap.MoveToRegion(MapSpan.FromCenterAndRadius(newPos, Distance.FromMiles(0.1)));
            }
            e.IsTracked = _isTracking;
            _locations.Add(e);
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}