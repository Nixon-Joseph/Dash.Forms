using Dash.Forms.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Dash.Forms.Helpers
{
    public class PreferenceHelper
    {
        public static double GetWeight()
        {
            return Preferences.Get($"Pref.{PreferenceTypes.Weight.ToString()}", 88d);
        }
        public static double GetWeight(UnitsType type)
        {
            double weight = GetWeight();
            if (type == UnitsType.Imperial)
            {
                weight = weight.ToPounds();
            }
            return weight;
        }
        public static void SetWeightKilos(double kilos)
        {
            Preferences.Set($"Pref.{PreferenceTypes.Weight.ToString()}", kilos);
        }
        public static void SetWeightPounds(double lbs)
        {
            SetWeightKilos(lbs.ToKilos());
        }
        public static int GetAge()
        {
            return Preferences.Get($"Pref.{PreferenceTypes.Age.ToString()}", 30);
        }
        public static void SetAge(double age)
        {
            Preferences.Set($"Pref.{PreferenceTypes.Age.ToString()}", age);
        }
        public static UnitsType GetUnits()
        {
            return (UnitsType)Preferences.Get($"Pref.{PreferenceTypes.Units.ToString()}", (int)UnitsType.Imperial);
        }
        public static void SetUnits(UnitsType type)
        {
            Preferences.Set($"Pref.{PreferenceTypes.Units.ToString()}", (int)type);
        }
    }

    public enum UnitsType
    {
        Metric,
        Imperial
    }

    public enum PreferenceTypes
    {
        Weight,
        Units,
        Age
    }
}
