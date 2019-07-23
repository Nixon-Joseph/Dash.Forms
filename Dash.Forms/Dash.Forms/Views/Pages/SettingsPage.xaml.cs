using Dash.Forms.Helpers;
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
        public static BindableProperty WeightProperty = BindableProperty.Create(nameof(Weight), typeof(string), typeof(SettingsPage), PreferenceHelper.GetWeight(PreferenceHelper.GetUnits()).ToString(),
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
        public BindableProperty AgeProperty = BindableProperty.Create(nameof(Age), typeof(int), typeof(SettingsPage), PreferenceHelper.GetAge(),
            propertyChanged: (b, o, n) =>
            {
                if (b is SettingsPage _this && n is int newAge)
                {
                    _this.AgeDebouncer.Debouce(() =>
                    {
                        PreferenceHelper.SetAge(newAge);
                    });
                }
            }
        );

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
        public int Age
        {
            get { return (int)GetValue(AgeProperty); }
            set { SetValue(AgeProperty, value); }
        }

        public List<int> AgeOptions { get; set; } = new List<int>();
        public List<UnitsOptionsItem> UnitsOptions { get; set; } = new List<UnitsOptionsItem>();

        private readonly Debouncer WeightDebouncer = new Debouncer();
        private readonly Debouncer AgeDebouncer = new Debouncer();

        public SettingsPage()
        { 
            UnitsOptions.Add(new UnitsOptionsItem(UnitsType.Metric));
            UnitsOptions.Add(new UnitsOptionsItem(UnitsType.Imperial));
            for (int ageYear = 10; ageYear <= 120; ageYear++)
            {
                AgeOptions.Add(ageYear);
            }

            var unitsType = PreferenceHelper.GetUnits();
            SelectedUnits = UnitsOptions.FirstOrDefault(u => u.Type == unitsType) ?? UnitsOptions[1];

            InitializeComponent();

            BindingContext = this;
        }
    }

    public class UnitsOptionsItem
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

        public string Display { get; set; }
        public UnitsType Type { get; set; }
    }
}