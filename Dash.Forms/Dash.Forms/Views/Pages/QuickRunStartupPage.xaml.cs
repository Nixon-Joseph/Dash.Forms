using Dash.Forms.Controls;
using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
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
            ToolbarItems.Add(new ToolbarItem("Go!", "", async () =>
            {
                List<TrainingSegment> segments = new List<TrainingSegment>();
                var index = 1;
                foreach (var segment in Segments)
                {
                    segments.Add(new TrainingSegment(index++, segment.Speed, segment.Value));
                }
                await Navigation.PushAsync(new RunTabbedPage(segments, TrainingType.Time));
            }));
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
                CloseSubFABs(CoolFABContainer, WarmupFABContainer, WalkFABContainer, RunFABContainer);
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
                OpenSubFABs((CoolFABContainer, 270), (WarmupFABContainer, 300), (WalkFABContainer, 330), (RunFABContainer, 0));
                FAB.RotateTo(225, ExpandDuration, OpeningEasing);
                Backdrop.IsVisible = true;
                Backdrop.FadeTo(0.5, ExpandDuration);
            }

            IsOpen = !IsOpen;
        }

        private void OpenSubFABs(params (AbsoluteLayout fabContainer, double angleDegrees)[] fabTuples)
        {
            foreach (var (fabContainer, angleDegrees) in fabTuples)
            {
                fabContainer.TranslateTo(DistanceWhenOpen * Math.Sin(angleDegrees * RadMult), DistanceWhenOpen * -Math.Cos(angleDegrees * RadMult), ExpandDuration, OpeningEasing);
                foreach (var fabContainerChild in fabContainer.Children)
                {
                    if (fabContainerChild is FAButton btn)
                    {
                        btn.FadeTo(1, ExpandDuration, OpeningEasing);
                    }
                    else if (fabContainerChild is Label label)
                    {
                        label.FadeTo(1, ExpandDuration, OpeningEasing);
                        label.TranslateTo(-((label.Text.Length - 4) * 5), 0, ExpandDuration, OpeningEasing);
                    }
                }
            }
        }

        private void CloseSubFABs(params AbsoluteLayout[] fabContainers)
        {
            foreach (var fabContainer in fabContainers)
            {
                fabContainer.TranslateTo(0, 0, ExpandDuration, OpeningEasing);
                foreach (var fabContainerChild in fabContainer.Children)
                {
                    if (fabContainerChild is View view)
                    {
                        view.FadeTo(0, ExpandDuration, OpeningEasing);
                        view.TranslateTo(0, 0, ExpandDuration, OpeningEasing);
                    }
                }
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