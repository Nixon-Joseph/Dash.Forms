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
        private readonly Debouncer WeightDebouncer = new Debouncer();

        public static BindableProperty WeightLabelTextProperty = BindableProperty.Create(nameof(WeightLabelText), typeof(string), typeof(SettingsPage), string.Empty);

        public static readonly int MaxYear = DateTime.Now.Year;
        public static readonly int MinYear = MaxYear - 100;
        //public static BindableProperty YearProperty = BindableProperty.Create(nameof(Year), typeof(int), typeof(SettingsPage), Preferences.Get("Pref.Birthday", new DateTime(MaxYear - 20, 1, 15)).Year,
        //    propertyChanged: (b, o, n) => {
        //        if (b is SettingsPage _this && n is int newVal)
        //        {
        //            _this.WeightDebouncer.Debouce(() => {
        //                Preferences.Set("Pref.Weight", newWeight);
        //            });
        //        }
        //    }
        //);
        //public static BindableProperty MonthProperty = BindableProperty.Create(nameof(Month), typeof(string), typeof(SettingsPage), Preferences.Get("Pref.Birthday", new DateTime(MaxYear - 20, 1, 15)).ToString("MMMM"),
        //    propertyChanged: (b, o, n) => {
        //        if (b is SettingsPage _this && n is int newVal)
        //        {
        //            _this.WeightDebouncer.Debouce(() => {
        //                Preferences.Set("Pref.Weight", newWeight);
        //            });
        //        }
        //    }
        //);

        public string WeightLabelText
        {
            get { return (string)GetValue(WeightLabelTextProperty); }
            set { SetValue(WeightLabelTextProperty, value); }
        }

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

        public string Weight
        {
            get { return (string)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }

        public SettingsPage()
        {
            //UnitsOptions.Add(new UnitsRadioItem() { Display = "Metric (kg, km)", Type = UnitsType.Metric });
            //UnitsOptions.Add(new UnitsRadioItem() { Display = "Imperial (lbs, mi)", Type = UnitsType.Imperial });

            //UnitsSelected.Subscribe((newVal) =>
            //{
            //    if (newVal != null)
            //    {
            //        Preferences.Set("Pref.Units", (int)newVal.Type);
            //        WeightLabelText = newVal.Type == UnitsType.Imperial ? "Pounds" : "Kilos";
            //    }
            //});

            //var unitsType = (UnitsType)Preferences.Get("Pref.Units", (int)UnitsType.Imperial);
            //UnitsSelected.Value = UnitsOptions.FirstOrDefault(u => u.Type == unitsType) ?? UnitsOptions[1];

            InitializeComponent();

            BindingContext = this;
        }
    }
    public enum UnitsType
    {
        Metric,
        Imperial
    }

    public class UnitsRadioItem
    {
        public string Display { get; set; }
        public UnitsType Type { get; set; }
    }
}