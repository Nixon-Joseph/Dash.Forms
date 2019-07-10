using Dash.Forms.Controls;
using Dash.Forms.Models.Run;
using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickRunStartupPage : ContentPage
    {
        private bool IsOpen = false;
        private readonly uint ExpandDuration = 250;
        private readonly double DistanceWhenOpen = 140;
        private readonly Easing OpeningEasing = Easing.Linear;
        private readonly double RadMult = Math.PI / 180d;

        private ObservableCollection<BindableTrainingSegment> Segments;

        public QuickRunStartupPage()
        {
            ToolbarItems.Add(new ToolbarItem("Go!", "", async () => { await Navigation.PushAsync(new RunTabbedPage()); }));
            Segments = new ObservableCollection<BindableTrainingSegment>();

            InitializeComponent();

            SegmentCollectionView.ItemsSource = Segments;
            FAB.Clicked += FAB_Clicked;
            Backdrop.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                NumberOfTapsRequired = 1,
                Command = new Command(ToggleFABs)
            });
            CoolFAB.Clicked += CoolFAB_Clicked;
            WarmupFAB.Clicked += WarmupFAB_Clicked;
            WalkFAB.Clicked += WalkFAB_Clicked;
            RunFAB.Clicked += RunFAB_Clicked;
        }

        private void RunFAB_Clicked(object sender, EventArgs e)
        {
            AddSegment(SegmentSpeeds.Run, "Run");
        }

        private void WalkFAB_Clicked(object sender, EventArgs e)
        {
            AddSegment(SegmentSpeeds.Walk, "Walk");
        }

        private void WarmupFAB_Clicked(object sender, EventArgs e)
        {
            AddSegment(SegmentSpeeds.FastWalk, "Warmup");
        }

        private void CoolFAB_Clicked(object sender, EventArgs e)
        {
            AddSegment(SegmentSpeeds.Walk, "Cooldown");
        }

        private void AddSegment(SegmentSpeeds speed, string name)
        {
            Segments.Add(new BindableTrainingSegment(speed, 5, name));
            ToggleFABs();
        }

        private void FAB_Clicked(object sender, EventArgs e)
        {
            ToggleFABs();
        }

        private void ToggleFABs()
        {
            if (IsOpen == true)
            {
                CloseSubFABs(CoolFAB, WarmupFAB, WalkFAB, RunFAB);
                FAB.RotateTo(0, ExpandDuration, OpeningEasing);
                Backdrop.FadeTo(0, ExpandDuration);
                Device.StartTimer(TimeSpan.FromMilliseconds(ExpandDuration), () =>
                {
                    if (IsOpen == false)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Backdrop.IsVisible = false;
                        });
                    }
                    return false;
                });
            }
            else
            {
                OpenSubFABs((CoolFAB, 270), (WarmupFAB, 300), (WalkFAB, 330), (RunFAB, 0));
                FAB.RotateTo(225, ExpandDuration, OpeningEasing);
                Backdrop.IsVisible = true;
                Backdrop.FadeTo(0.5, ExpandDuration);
            }

            IsOpen = !IsOpen;
        }

        private void OpenSubFABs(params (FAButton fab, double angleDegrees)[] fabTuples)
        {
            foreach (var (fab, angleDegrees) in fabTuples)
            {
                fab.TranslateTo(DistanceWhenOpen * Math.Sin(angleDegrees * RadMult), DistanceWhenOpen * -Math.Cos(angleDegrees * RadMult), ExpandDuration, OpeningEasing);
                fab.FadeTo(1, ExpandDuration, OpeningEasing);
            }
        }

        private void CloseSubFABs(params FAButton[] fabs)
        {
            foreach (var fab in fabs)
            {
                fab.TranslateTo(0, 0, ExpandDuration, OpeningEasing);
                fab.FadeTo(0, ExpandDuration, OpeningEasing);
            }
        }

        private class BindableTrainingSegment : BindableObject
        {
            public readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(int), typeof(BindableTrainingSegment), 5);
            public readonly BindableProperty SpeedProperty = BindableProperty.Create(nameof(Speed), typeof(SegmentSpeeds), typeof(BindableTrainingSegment), SegmentSpeeds.None);
            public readonly BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(BindableTrainingSegment), "");

            public int Value
            {
                get { return (int)GetValue(ValueProperty); }
                set { SetValue(ValueProperty, value); }
            }

            public SegmentSpeeds Speed
            {
                get { return (SegmentSpeeds)GetValue(SpeedProperty); }
                set { SetValue(SpeedProperty, value); }
            }

            public string Label
            {
                get { return (string)GetValue(LabelProperty); }
                set { SetValue(LabelProperty, value); }
            }

            public BindableTrainingSegment(SegmentSpeeds speed, int value, string label)
            {
                Value = value;
                Speed = speed;
                Label = label;
            }
        }
    }
}