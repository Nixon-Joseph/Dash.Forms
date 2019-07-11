﻿using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Extensions;
using Dash.Forms.Helpers.Storage;
using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ILocationService LocationService;
        private readonly ITextToSpeach TextToSpeach;
        private readonly Timer Timer;
        private readonly List<LocationData> Locations;
        private DateTime StartTime;
        private TimeSpan PauseOffset = new TimeSpan();
        private TimeSpan SegmentPauseOffset = new TimeSpan();
        private bool IsTracking = false;
        private bool JustUnpaused = false;
        private bool HasStoppedService = false;
        private double TotalDistance = 0d;
        private double TotalSegmentDistance = 0d;
        private RunState CurrentState = RunState.Unstarted;
        private double? MaxElevation = null;
        private double? MinElevation = null;
        private double? LastMaxDistance = null;
        private int LastDistanceCheckCounter;
        private readonly int LastDistanceCheckThreshold = 4;
        private bool LocationServiceStarted = false;
        private readonly IEnumerable<TrainingSegment> TrainingSegments;
        private int CurrentSegmentIndex = 0;
        private readonly List<RunSegment> RunSegments = null;
        private readonly string ProgramId;
        private readonly int WeekNum;
        private readonly int DayNum;
        private readonly TrainingType TrainingType;
        private readonly double TrainingEndValue;
        public bool HitHalfway = false;
        public bool HitEnd = false;
        public bool UseMiles = true;

        public RunTabbedPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            LocationService = DependencyService.Get<ILocationService>();
            LocationService.AddLocationChangedListener(_locationService_LocationChanged);

            TextToSpeach = DependencyService.Get<ITextToSpeach>();

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

            Timer = new Timer() { Interval = 1000 };
            Timer.Elapsed += _timer_Elapsed;
            Timer.Start();

            Locations = new List<LocationData>();

            if (LocationService.GetQuickLocation() is LocationData currentLoc)
            {
                RunMap.MoveToRegion(MapSpan.FromCenterAndRadius(currentLoc.GetPosition(), Distance.FromKilometers(0.1d)));
            }
        }

        public RunTabbedPage(IEnumerable<TrainingSegment> segments, TrainingType type) : this()
        {
            TrainingType = type;
            TrainingSegments = segments.OrderBy(s => s.SegmentNumber);
            if (TrainingSegments.Count() > 0)
            {
                TrainingEndValue = TrainingSegments.Sum(t => t.Value);
                RunSegments = new List<RunSegment> { new RunSegment(segments.FirstOrDefault()) };
            }
        }

        public RunTabbedPage(TrainingDay day, string programId, int weekNum) : this(day.Segments, day.TrainingType)
        {
            ProgramId = programId;
            WeekNum = weekNum;
            DayNum = day.DayNumber;
        }

        private async void RunCancelButton_Clicked(object sender, EventArgs e)
        {
            if (LocationServiceStarted == true)
            {
                StopLocationService();
            }
            await Navigation.PopAsync();
        }

        private void StartRunButton_Clicked(object sender, EventArgs e)
        {
            LocationService.Start();
            LocationServiceStarted = true;
            StartTime = DateTime.UtcNow;
            SetRunState(RunState.Running);
            TextToSpeach.Speak("Lets go!");
            if (GetCurrentSegment() is RunSegment curSegment)
            {
                SpeakNextSegment(curSegment);
                curSegment.StartTime = StartTime;
            }
            IsTracking = true;
        }

        private void SpeakNextSegment(RunSegment segment)
        {
            string message = null;
            switch (segment.Speed)
            {
                case SegmentSpeeds.Walk:
                    message = "Walk.";
                    break;
                case SegmentSpeeds.FastWalk:
                    message = "Brisk Walk.";
                    break;
                case SegmentSpeeds.Jog:
                    message = "Jog.";
                    break;
                case SegmentSpeeds.Run:
                    message = "Run.";
                    break;
                case SegmentSpeeds.SteadyRun:
                    message = "Steady Run.";
                    break;
                case SegmentSpeeds.TempoRun:
                    message = "Tempo Run.";
                    break;
                case SegmentSpeeds.Sprint:
                    message = "Sprint. Maximum Effort.";
                    break;
                case SegmentSpeeds.None:
                default:
                    break;
            }
            if (message.IsNullOrEmpty() == false)
            {
                TextToSpeach.Speak(message);
            }
        }

        ~RunTabbedPage()
        {
            if (HasStoppedService == false)
            {
                StopLocationService();
            }
        }

        private void PauseButton_Clicked(object sender, System.EventArgs e)
        {
            IsTracking = !IsTracking;
            if (IsTracking == true)
            {
                JustUnpaused = true;
                SetRunState(RunState.Running);
            }
            else
            {
                SetRunState(RunState.Paused);
            }
            RunMap.HasScrollEnabled = !IsTracking;
        }

        private async void FinishButton_Clicked(object sender, System.EventArgs e)
        {
            await EndRun();
        }

        private async Task EndRun()
        {
            if (await DisplayAlert("Finish Run?", "Are you sure you want to end your run?", "Yes", "No"))
            {
                if (HasStoppedService == false) // should always be true here I think.
                {
                    StopLocationService();
                    Timer.Stop();
                    if (GetCurrentSegment() is RunSegment curSegment)
                    {
                        curSegment.Duration = GetSegmentDuration(curSegment);
                    }

                    var duration = DateTime.UtcNow - (StartTime + PauseOffset);
                    var runData = new RunData()
                    {
                        Start = StartTime,
                        End = DateTime.UtcNow,
                        Distance = TotalDistance,
                        Elapsed = duration,
                        DayNumber = DayNum,
                        TrainingProgramId = ProgramId,
                        IsFreeRun = ProgramId.IsNullOrEmpty(),
                        WeekNumber = WeekNum,
                        Segments = RunSegments ?? new List<RunSegment>() {
                            new RunSegment()
                            {
                                Distance = TotalDistance,
                                Duration = duration,
                                Locations = Locations,
                                Speed = SegmentSpeeds.None,
                                StartTime = StartTime
                            }
                        }
                    };
                    var runStorage = new RunDataStorageHelper();
                    runStorage.Insert(runData);
                    await Navigation.PopAsync();
                }
            }
        }

        private RunSegment GetCurrentSegment()
        {
            if (RunSegments != null)
            {
                return RunSegments.ElementAt(CurrentSegmentIndex);
            }
            return null;
        }

        private void StopLocationService()
        {
            HasStoppedService = true;
            LocationService.RemoveLocationChangedListener(_locationService_LocationChanged);
            LocationService.Stop();
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsTracking == true)
            {
                var spent = GetRunDuration();
                if (GetCurrentSegment() is RunSegment curSegment)
                {
                    if (TrainingType == TrainingType.Time)
                    {
                        if (HitHalfway == false && spent.TotalMinutes >= TrainingEndValue / 2)
                        {
                            HitHalfway = true;
                            SpeakHalfway();
                        }
                        // end of segment
                        if (GetCurrentTrainingSegment() is TrainingSegment trainingSegment && GetSegmentDuration(curSegment).TotalMinutes >= trainingSegment.Value)
                        {
                            HandleNextSegment();
                        }
                    }
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    MapTimeLabel.Text = spent.ToString(spent.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");
                    StatsTimeLabel.Text = MapTimeLabel.Text;
                });
            }
            else if (CurrentState == RunState.Paused)
            {
                PauseOffset = PauseOffset.Add(TimeSpan.FromSeconds(1));
                SegmentPauseOffset = SegmentPauseOffset.Add(TimeSpan.FromSeconds(1));
            }
        }

        private void SpeakHalfway()
        {
            TextToSpeach.Speak("You're halfway there! Keep going!");
        }

        private void SpeakEnd()
        {
            TextToSpeach.Speak("Whew! You made it, well done!");
        }

        private TrainingSegment GetCurrentTrainingSegment()
        {
            if (TrainingSegments != null && TrainingSegments.Count() > 0)
            {
                return TrainingSegments.ElementAt(CurrentSegmentIndex);
            }
            return null;
        }

        private void _locationService_LocationChanged(object sender, LocationData loc)
        {
            if (IsTracking == true)
            {
                var newPos = new Position(loc.Latitude, loc.Longitude);
                if (Locations.Count() > 0 && Locations.Last() is LocationData lastLoc && lastLoc.IsTracked == true)
                {
                    RunMap.AddPosition(newPos);
                    RunMap.MoveToRegion(MapSpan.FromCenterAndRadius(newPos, Distance.FromKilometers(GetMaxDistance() / 1000d)));
                    MinElevation = Math.Min(MinElevation ?? lastLoc.Altitude, lastLoc.Altitude);
                    MaxElevation = Math.Max(MaxElevation ?? lastLoc.Altitude, lastLoc.Altitude);
                    var meters = LocationService.GetDistance(lastLoc.GetPosition(), newPos);
                    TotalDistance += meters;
                    var convertedDistance = ConvertMeters(TotalDistance);
                    if (GetCurrentSegment() is RunSegment curSegment && curSegment.Locations.Count() > 0)
                    {
                        TotalSegmentDistance += meters;
                        if (TrainingType == TrainingType.Distance)
                        {
                            if (HitHalfway == false && convertedDistance >= TrainingEndValue / 2)
                            {
                                HitHalfway = true;
                                SpeakHalfway();
                            }
                            // end of segment
                            if (GetCurrentTrainingSegment() is TrainingSegment trainingSegment && ConvertMeters(TotalSegmentDistance) >= trainingSegment.Value)
                            {
                                HandleNextSegment();
                            }
                        }
                    }
                    var pace = convertedDistance > 0d ? TimeSpan.FromMinutes(GetRunDuration().TotalMinutes / convertedDistance) : TimeSpan.FromMinutes(0);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ElevationLabel.Text = (MaxElevation.Value - MinElevation.Value).ToString("N1");
                        RunDistanceLabel.Text = convertedDistance.ToString("N2");
                        StatsDistanceLabel.Text = convertedDistance.ToString("N2");
                        if (convertedDistance > 0.1d)
                        {
                            StatsPaceLabel.Text = pace.ToString("mm':'ss");
                        }
                        else
                        {
                            StatsPaceLabel.Text = "∞";
                        }
                        //https://fitness.stackexchange.com/a/36045
                        //                                 distance * weight * constant // should be in metric
                        StatsCaloriesLabel.Text = ((int)((TotalDistance / 1000) * 190 * 1.036)).ToString();
                    });
                }
                if (JustUnpaused == true)
                {
                    JustUnpaused = false;
                    RunMap.AddPosition(newPos, false);
                }
            }
            loc.IsTracked = IsTracking;
            Locations.Add(loc);
            GetCurrentSegment()?.Locations.Add(loc);
        }

        private void HandleNextSegment()
        {
            SegmentPauseOffset = new TimeSpan();
            if (CurrentSegmentIndex < TrainingSegments.Count() - 1)
            {
                CurrentSegmentIndex++;
                RunSegments.Add(new RunSegment(GetCurrentTrainingSegment()) { StartTime = DateTime.UtcNow });
                SpeakNextSegment(GetCurrentSegment());
            }
            else if (HitEnd == false)
            {
                HitEnd = true;
                SpeakEnd();
            }
        }

        private double ConvertMeters(double meteres)
        {
            return UseMiles == true ? (meteres / 1609.344) : (meteres / 1000);
        }

        private TimeSpan GetRunDuration()
        {
            return DateTime.UtcNow - (StartTime + PauseOffset);
        }

        private TimeSpan GetSegmentDuration(RunSegment runSegment)
        {
            return DateTime.UtcNow - (runSegment.StartTime + SegmentPauseOffset);
        }

        private double GetMaxDistance()
        {
            double distance = LastMaxDistance ?? 0.1d;
            if (LastMaxDistance != null)
            {
                if (LastDistanceCheckCounter++ > LastDistanceCheckThreshold)
                {
                    LastDistanceCheckCounter = 0;
                    if (RunMap?.RouteCoordinates.Count() > 0)
                    {
                        var (minLat, minLng, maxLat, maxLng) = RunMap.RouteCoordinates.GetMinsAndMaxes();
                        distance = Math.Max(distance, LocationService.GetDistance(minLat.Value, minLng.Value, maxLat.Value, maxLng.Value));
                        LastMaxDistance = distance;
                    }
                }
            }
            else
            {
                LastMaxDistance = distance;
            }
            return distance;
        }

        protected override bool OnBackButtonPressed()
        {
            if (CurrentState != RunState.Unstarted)
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
            CurrentState = state;
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