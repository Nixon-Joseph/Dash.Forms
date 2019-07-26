using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Extensions;
using Dash.Forms.Helpers;
using Dash.Forms.Helpers.Storage;
using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunTabbedPage : TabbedPage
    {
        private readonly ILocationService LocationService;
        private readonly Timer Timer;
        private readonly List<LocationData> Locations;
        private DateTime StartTime;
        private TimeSpan PauseOffset = new TimeSpan();
        private TimeSpan SegmentPauseOffset = new TimeSpan();
        private bool IsTracking = false;
        private bool JustUnpaused = false;
        private bool HasStoppedService = false;
        private double TotalDistance = 0d;
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
        private readonly bool UseMiles = false;
        private readonly bool CalcCalories = false;
        private readonly double UserWeight = 0;
        private readonly bool SpeakSegmentEnd = false;
        private readonly bool SpeakDistanceUnits = false;
        private readonly bool SpeakHalfDistanceUnits = false;
        private bool HitHalfway = false;
        private bool HitEnd = false;
        private double NextPaceUnit = 0d;
        private double NextPaceUnitStep = 0d;
        private DateTime LastDistancePaceSpeak;

        public RunTabbedPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            LocationService = DependencyService.Get<ILocationService>();
            LocationService.AddLocationChangedListener(_locationService_LocationChanged);

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

            UseMiles = PreferenceHelper.GetUnits() == UnitsType.Imperial;
            UserWeight = PreferenceHelper.GetWeight();
            var paceSpeakOption = PreferenceHelper.GetEnablePaceNotifier();
            SpeakSegmentEnd = paceSpeakOption == PaceNotifierTypes.Segment || paceSpeakOption == PaceNotifierTypes.HalfUnitAndSegment || paceSpeakOption == PaceNotifierTypes.UnitAndSegment;
            SpeakDistanceUnits = paceSpeakOption == PaceNotifierTypes.UnitAndSegment || paceSpeakOption == PaceNotifierTypes.Unit;
            SpeakHalfDistanceUnits = paceSpeakOption == PaceNotifierTypes.HalfUnitAndSegment || paceSpeakOption == PaceNotifierTypes.HalfUnit;

            if (SpeakDistanceUnits == true)
            {
                NextPaceUnitStep = Constants.Distances.MetersInMile;
            }
            else if (SpeakHalfDistanceUnits == true)
            {
                NextPaceUnitStep = Constants.Distances.MetersInMile / 2;
            }
            NextPaceUnit = NextPaceUnitStep;

            if (UserWeight > 0)
            {
                CalcCalories = true;
            }

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
            LastDistancePaceSpeak = DateTime.UtcNow;
            SetRunState(RunState.Running);
            Speak("Lets go!");
            if (GetCurrentSegment() is RunSegment curSegment)
            {
                SpeakNextSegment(curSegment);
                curSegment.StartTime = StartTime;
            }
            IsTracking = true;
        }

        private void Speak(string text)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TextToSpeech.SpeakAsync(text);
            });
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
                Speak(message);
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
                        if (curSegment.Speed == SegmentSpeeds.Extra)
                        {
                            if (curSegment.Duration.TotalMinutes < 0.5d || await DisplayAlert("Keep Extra?", $"You completed your workout {curSegment.Duration.TotalMinutes} minutes ago.\n\nWould you like to keep the last {curSegment.Duration.TotalMinutes} minutes of your workout?", "No", "Yes") == false)
                            {
                                RunSegments.RemoveAt(CurrentSegmentIndex--);
                            }
                        }
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
                        var segmentDuration = GetSegmentDuration(curSegment);
                        curSegment.Duration = segmentDuration;
                        if (GetCurrentTrainingSegment() is TrainingSegment trainingSegment && segmentDuration.TotalMinutes >= trainingSegment.Value)
                        {
                            HandleNextSegment(curSegment);
                        }
                    }
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    MapTimeLabel.Text = spent.ToOptionalHourString();
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
            Speak("You're halfway there! Keep going!");
        }

        private void SpeakEnd()
        {
            Speak("You made it, way to go!");
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
                        curSegment.Distance += meters;
                        if ((SpeakHalfDistanceUnits || SpeakDistanceUnits) && curSegment.Distance >= NextPaceUnit)
                        {
                            SpeakDistancePace();
                        }
                        if (TrainingType == TrainingType.Distance)
                        {
                            // speak segment distance pace
                            if (HitHalfway == false && convertedDistance >= TrainingEndValue / 2)
                            {
                                HitHalfway = true;
                                SpeakHalfway();
                            }
                            // end of segment
                            var convertedSegmentDistance = ConvertMeters(curSegment.Distance);
                            if (GetCurrentTrainingSegment() is TrainingSegment trainingSegment && convertedSegmentDistance >= trainingSegment.Value)
                            {
                                HandleNextSegment(curSegment);
                            }
                        }
                    }
                    else if ((SpeakHalfDistanceUnits || SpeakDistanceUnits) && TotalDistance >= NextPaceUnit)
                    {
                        SpeakDistancePace();
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
                        if (CalcCalories == true)
                        {
                            StatsCaloriesLabel.Text = ((int)RunHelper.CalculateCalories(TotalDistance, UserWeight)).ToString();
                        }
                        else
                        {
                            StatsCaloriesLabel.Text = "---";
                        }
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

        private void SpeakDistancePace()
        {
            var lastPace = TimeSpan.FromMinutes((DateTime.UtcNow - LastDistancePaceSpeak).TotalMinutes / ConvertMeters(NextPaceUnitStep));
            NextPaceUnit += NextPaceUnitStep;
            LastDistancePaceSpeak = DateTime.UtcNow;
            Speak($"Your pace for the last {GetPaceDistanceText()} has been {lastPace.Minutes} minutes, {lastPace.Seconds} seconds");
        }

        private string GetPaceDistanceText()
        {
            string text = UseMiles ? "mile" : "kilometer";
            if (SpeakHalfDistanceUnits == true)
            {
                text = $"half {text}";
            }
            return text;
        }

        private void HandleNextSegment(RunSegment curSegment)
        {
            LastDistancePaceSpeak = DateTime.UtcNow;
            NextPaceUnit = NextPaceUnitStep;
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
                RunSegments.Add(new RunSegment() { StartTime = DateTime.UtcNow, Speed = SegmentSpeeds.Extra });
                CurrentSegmentIndex++;
            }
            // speak segment pace
            if (SpeakSegmentEnd == true && curSegment.Distance > 0d)
            {
                var segmentPace = TimeSpan.FromMinutes(curSegment.Duration.TotalMinutes / ConvertMeters(curSegment.Distance));
                Speak($"Your pace for the previous segment was {segmentPace.Minutes} minutes, {segmentPace.Seconds} seconds.");
            }
        }

        private double ConvertMeters(double meters)
        {
            return UseMiles ? meters.ToMiles() : meters.ToKilometers();
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