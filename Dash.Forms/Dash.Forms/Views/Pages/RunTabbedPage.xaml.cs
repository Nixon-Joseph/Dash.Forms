using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Extensions;
using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunTabbedPage : TabbedPage
    {
        private readonly ILocationService _locationService;
        private readonly Timer _timer;
        private readonly List<LocationData> _locations;
        private readonly DateTime _startTime;
        private TimeSpan _pauseOffset = new TimeSpan();
        private bool _isTracking = false;
        private bool _justUnpaused = false;
        private bool _hasStoppedService = false;
        private double _totalDistance = 0d;
        private RunState _currentState = RunState.Unstarted;

        public RunTabbedPage()
        {
            InitializeComponent();

            _locationService = DependencyService.Get<ILocationService>();
            _locationService.AddLocationChangedListener(_locationService_LocationChanged);

            RunStartRunButton.Clicked += StartRunButton_Clicked;
            StatsStartRunButton.Clicked += StartRunButton_Clicked;

            RunPauseButton.Clicked += PauseButton_Clicked;
            StatsPauseButton.Clicked += PauseButton_Clicked;

            RunCancelButton.Clicked += RunCancelButton_Clicked;
            StatsCancelButton.Clicked += RunCancelButton_Clicked;

            RunResumeButton.Clicked += PauseButton_Clicked;
            StatsResumeButton.Clicked += PauseButton_Clicked;

            RunFinishButton.Clicked += FinishButton_Clicked;
            StatsFinishButton.Clicked += FinishButton_Clicked;

            _timer = new Timer() { Interval = 1000 };
            _timer.Elapsed += _timer_Elapsed;
            _startTime = DateTime.Now;
            _timer.Start();

            _locations = new List<LocationData>();

            if (_locationService.GetQuickLocation() is LocationData currentLoc)
            {
                RunMap.MoveToRegion(MapSpan.FromCenterAndRadius(currentLoc.GetPosition(), Distance.FromKilometers(0.1d)));
            }
        }

        private async void RunCancelButton_Clicked(object sender, EventArgs e)
        {
            StopLocationService();
            await Navigation.PopAsync();
        }

        private void StartRunButton_Clicked(object sender, EventArgs e)
        {
            _locationService.Start();
            SetRunState(RunState.Running);
            _isTracking = true;
        }

        ~RunTabbedPage()
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
                SetRunState(RunState.Running);
            }
            else
            {
                SetRunState(RunState.Paused);
            }
            RunMap.HasScrollEnabled = !_isTracking;
        }

        private async void FinishButton_Clicked(object sender, System.EventArgs e)
        {
            await EndRun();
        }

        private async Task EndRun()
        {
            if (await DisplayAlert("Cancel Run?", "Are you sure you want to end your run?", "Yes", "No"))
            {
                if (_hasStoppedService == false) // should always be true here I think.
                {
                    StopLocationService();
                    _timer.Stop();
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
                var pace = TimeSpan.FromMinutes(spent.TotalMinutes / _totalDistance);
                Device.BeginInvokeOnMainThread(() =>
                {
                    MapTimeLabel.Text = spent.ToString(spent.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");
                    StatsTimeLabel.Text = MapTimeLabel.Text;
                    if (_totalDistance < 0.1d || pace > TimeSpan.FromHours(1))
                    {
                        StatsPaceLabel.Text = "∞";
                    }
                    else
                    {
                        StatsPaceLabel.Text = pace.ToString("mm':'ss");
                    }
                    //https://fitness.stackexchange.com/a/36045
                    //                                 distance * weight * constant // should be in metric
                    StatsCaloriesLabel.Text = ((int)(_totalDistance * 190 * 1.036)).ToString();
                });
            }
            else
            {
                _pauseOffset = _pauseOffset.Add(TimeSpan.FromSeconds(1));
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
                        RunDistanceLabel.Text = _totalDistance.ToString("N2");
                        StatsDistanceLabel.Text = _totalDistance.ToString("N2");
                    });
                }
                if (_justUnpaused == true)
                {
                    _justUnpaused = false;
                    RunMap.AddPosition(newPos, false);
                }
                RunMap.AddPosition(newPos);
                RunMap.MoveToRegion(MapSpan.FromCenterAndRadius(newPos, Distance.FromKilometers(GetMaxDistance() / 1000d)));
            }
            e.IsTracked = _isTracking;
            _locations.Add(e);
        }

        private double GetMaxDistance()
        {
            double? minLat = null, minLng = null, maxLat = null, maxLng = null;
            double distance = 0.1d;
            if (RunMap?.RouteCoordinates.Count() > 0)
            {
                foreach (var position in RunMap.RouteCoordinates)
                {
                    minLat = Math.Min(minLat ?? position.Latitude, position.Latitude);
                    minLng = Math.Min(minLng ?? position.Longitude, position.Longitude);
                    maxLat = Math.Max(maxLat ?? position.Latitude, position.Latitude);
                    maxLng = Math.Max(maxLng ?? position.Longitude, position.Longitude);
                }
                distance = Math.Max(distance, _locationService.GetDistance(minLat.Value, minLng.Value, maxLat.Value, maxLng.Value));
            }
            return distance;
        }

        protected override bool OnBackButtonPressed()
        {
            if (_currentState != RunState.Unstarted)
            {
                Task.Run(EndRun);
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        private void SetRunState(RunState state)
        {
            _currentState = state;
            RunUnstartedButtonStack.IsVisible = state == RunState.Unstarted;
            StatsUnstartedButtonStack.IsVisible = state == RunState.Unstarted;
            RunRunningButtonStack.IsVisible = state == RunState.Running;
            StatsRunningButtonStack.IsVisible = state == RunState.Running;
            RunPausedButtonStack.IsVisible = state == RunState.Paused;
            StatsPausedButtonStack.IsVisible = state == RunState.Paused;
        }

        private enum RunState
        {
            Unstarted,
            Paused,
            Running,
            Finished
        }
    }
}