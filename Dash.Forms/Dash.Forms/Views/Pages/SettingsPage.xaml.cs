using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Dash.Forms.Helpers;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public static BindableProperty WeightLabelTextProperty = BindableProperty.Create(nameof(WeightLabelText), typeof(string), typeof(SettingsPage), string.Empty);
        public static BindableProperty WeightProperty = BindableProperty.Create(nameof(Weight), typeof(string), typeof(SettingsPage), Preferences.Get("Pref.Weight", 120d).ToString(),
            propertyChanged: (b, o, n) => {
                if (b is SettingsPage _this && n is string newVal)
                {
                    double.TryParse(newVal, out double newWeight);
                    _this.WeightDebouncer.Debouce(() => {
                        Preferences.Set("Pref.Weight", newWeight);
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
                    Preferences.Set("Pref.Units", (int)newItem.Type);
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
            get { return (UnitsOptionsItem) GetValue(SelectedUnitsProperty); }
            set { SetValue(SelectedUnitsProperty, value); }
        }

        public List<UnitsOptionsItem> UnitsOptions { get; set; } = new List<UnitsOptionsItem>();

        private readonly Debouncer WeightDebouncer = new Debouncer();

        public SettingsPage()
        {
            UnitsOptions.Add(new UnitsOptionsItem(UnitsType.Metric));
            UnitsOptions.Add(new UnitsOptionsItem(UnitsType.Imperial));

            var unitsType = (UnitsType)Preferences.Get("Pref.Units", (int)UnitsType.Imperial);
            SelectedUnits = UnitsOptions.FirstOrDefault(u => u.Type == unitsType) ?? UnitsOptions[1];

            InitializeComponent();

            BindingContext = this;
        }
    }
    public enum UnitsType
    {
        Metric,
        Imperial
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