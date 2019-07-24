using Dash.Forms.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public static BindableProperty WeightLabelTextProperty = BindableProperty.Create(nameof(WeightLabelText), typeof(string), typeof(SettingsPage), string.Empty);
        public static BindableProperty WeightProperty = BindableProperty.Create(nameof(Weight), typeof(string), typeof(SettingsPage), "0",
            propertyChanged: (b, o, n) =>
            {
                if (b is SettingsPage _this && n is string newVal)
                {
                    double.TryParse(newVal, out double newWeight);
                    _this.WeightDebouncer.Debouce(() =>
                    {
                        if (_this.SelectedUnits.Type == UnitsType.Imperial)
                        {
                            PreferenceHelper.SetWeightPounds(newWeight);
                        }
                        else
                        {
                            PreferenceHelper.SetWeightKilos(newWeight);
                        }
                    });
                }
            }
        );
        public BindableProperty SelectedPaceNotifierProperty = BindableProperty.Create(nameof(SelectedPaceNotifier), typeof(DistanceOptionsItem), typeof(SettingsPage),
            propertyChanged: (b, o, n) =>
            {
                if (b is SettingsPage _this && n is DistanceOptionsItem newVal)
                {
                    PreferenceHelper.SetEnablePaceNotifier(newVal.Type);
                }
            }
        );
        public BindableProperty SelectedUnitsProperty = BindableProperty.Create(nameof(SelectedUnits), typeof(UnitsOptionsItem), typeof(SettingsPage),
            propertyChanged: (b, o, n) =>
            {
                if (b is SettingsPage _this && n is UnitsOptionsItem newItem)
                {
                    switch (newItem.Type)
                    {
                        case UnitsType.Metric:
                            _this.WeightLabelText = "Weight (kg)";
                            break;
                        case UnitsType.Imperial:
                            _this.WeightLabelText = "Weight (lbs)";
                            break;
                    }
                    PreferenceHelper.SetUnits(newItem.Type);
                }
            }
        );
        //public BindableProperty AgeProperty = BindableProperty.Create(nameof(Age), typeof(int), typeof(SettingsPage), PreferenceHelper.GetAge(),
        //    propertyChanged: (b, o, n) =>
        //    {
        //        if (b is SettingsPage _this && n is int newAge)
        //        {
        //            _this.AgeDebouncer.Debouce(() =>
        //            {
        //                PreferenceHelper.SetAge(newAge);
        //            });
        //        }
        //    }
        //);

        public string WeightLabelText
        {
            get { return (string)GetValue(WeightLabelTextProperty); }
            set { SetValue(WeightLabelTextProperty, value); }
        }
        public string Weight
        {
            get { return (string)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }
        public UnitsOptionsItem SelectedUnits
        {
            get { return (UnitsOptionsItem)GetValue(SelectedUnitsProperty); }
            set { SetValue(SelectedUnitsProperty, value); }
        }
        public DistanceOptionsItem SelectedPaceNotifier
        {
            get { return (DistanceOptionsItem)GetValue(SelectedPaceNotifierProperty); }
            set { SetValue(SelectedPaceNotifierProperty, value); }
        }
        //public int Age
        //{
        //    get { return (int)GetValue(AgeProperty); }
        //    set { SetValue(AgeProperty, value); }
        //}

        //public List<int> AgeOptions { get; set; } = new List<int>();
        public List<UnitsOptionsItem> UnitsOptions { get; set; } = new List<UnitsOptionsItem>();
        public List<DistanceOptionsItem> PaceNotifierOptions { get; set; } = new List<DistanceOptionsItem>();

        private readonly Debouncer WeightDebouncer = new Debouncer();
        //private readonly Debouncer AgeDebouncer = new Debouncer();

        private UnitsType GetSelectedUnits() { return SelectedUnits.Type; }

        public SettingsPage()
        {
            var unitsType = PreferenceHelper.GetUnits();
            Weight = PreferenceHelper.GetWeight(unitsType).ToString();
            UnitsOptions.Add(new UnitsOptionsItem(UnitsType.Metric));
            UnitsOptions.Add(new UnitsOptionsItem(UnitsType.Imperial));
            SelectedUnits = UnitsOptions.FirstOrDefault(u => u.Type == unitsType) ?? UnitsOptions[1];
            //for (int ageYear = 10; ageYear <= 120; ageYear++)
            //{
            //    AgeOptions.Add(ageYear);
            //}

            PaceNotifierOptions.Add(new DistanceOptionsItem(PaceNotifierTypes.HalfUnit, GetSelectedUnits));
            PaceNotifierOptions.Add(new DistanceOptionsItem(PaceNotifierTypes.Unit, GetSelectedUnits));
            PaceNotifierOptions.Add(new DistanceOptionsItem(PaceNotifierTypes.Segment, GetSelectedUnits));
            PaceNotifierOptions.Add(new DistanceOptionsItem(PaceNotifierTypes.HalfUnitAndSegment, GetSelectedUnits));
            PaceNotifierOptions.Add(new DistanceOptionsItem(PaceNotifierTypes.UnitAndSegment, GetSelectedUnits));
            var distanceNotifierType = PreferenceHelper.GetEnablePaceNotifier();
            SelectedPaceNotifier = PaceNotifierOptions.FirstOrDefault(u => u.Type == distanceNotifierType) ?? PaceNotifierOptions[1];

            InitializeComponent();

            BindingContext = this;
        }
    }

    public abstract class OptionsItem<T>
    {
        public virtual string Display { get; set; }
        public T Type { get; set; }
    }

    public class UnitsOptionsItem : OptionsItem<UnitsType>
    {
        public UnitsOptionsItem() { }
        public UnitsOptionsItem(UnitsType type)
        {
            Type = type;
            switch (type)
            {
                case UnitsType.Metric:
                    Display = "Metric (kg, km)";
                    break;
                case UnitsType.Imperial:
                    Display = "Imperial (lbs, mi)";
                    break;
            }
        }
    }

    public class DistanceOptionsItem : OptionsItem<PaceNotifierTypes>
    {
        public DistanceOptionsItem() { }
        public DistanceOptionsItem(PaceNotifierTypes type, Func<UnitsType> getCurUnitsTypeFunc)
        {
            Type = type;
            GetCurUnitsType = getCurUnitsTypeFunc;
        }

        private Func<UnitsType> GetCurUnitsType { get; }

        public override string Display
        {
            get
            {
                string disp = "";
                var curUnits = GetCurUnitsType() == UnitsType.Imperial ? "mile" : "kilometer";
                switch (Type)
                {
                    case PaceNotifierTypes.HalfUnit:
                        disp = $"Half {curUnits}";
                        break;
                    case PaceNotifierTypes.Unit:
                        disp = $"Full {curUnits}";
                        break;
                    case PaceNotifierTypes.Segment:
                        disp = "Segment";
                        break;
                    case PaceNotifierTypes.HalfUnitAndSegment:
                        disp = $"Half {curUnits} and Segment";
                        break;
                    case PaceNotifierTypes.UnitAndSegment:
                        disp = $"Full {curUnits} and Segment";
                        break;
                }
                return disp;
            }
            set => base.Display = value;
        }
    }
}