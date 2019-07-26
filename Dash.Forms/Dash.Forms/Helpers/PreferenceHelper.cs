using Dash.Forms.Extensions;
using System;
using Xamarin.Essentials;

namespace Dash.Forms.Helpers
{
    public class PreferenceHelper
    {
        public static double GetWeight()
        {
            return Preferences.Get(PrefString(PreferenceTypes.Weight), 0d);
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
            Preferences.Set(PrefString(PreferenceTypes.Weight), kilos);
        }
        public static void SetWeightPounds(double lbs)
        {
            SetWeightKilos(Math.Round(lbs.ToKilos(), 1));
        }
        public static int GetAge()
        {
            return Preferences.Get(PrefString(PreferenceTypes.Age), 30);
        }
        public static void SetAge(int age)
        {
            Preferences.Set(PrefString(PreferenceTypes.Age), age);
        }
        public static UnitsType GetUnits()
        {
            return (UnitsType)Preferences.Get(PrefString(PreferenceTypes.Units), (int)UnitsType.Imperial);
        }
        public static void SetUnits(UnitsType type)
        {
            Preferences.Set(PrefString(PreferenceTypes.Units), (int)type);
        }
        public static PaceNotifierTypes GetEnablePaceNotifier()
        {
            return (PaceNotifierTypes)Preferences.Get(PrefString(PreferenceTypes.PaceNotifier), (int)PaceNotifierTypes.Segment);
        }
        public static void SetEnablePaceNotifier(PaceNotifierTypes type)
        {
            Preferences.Set(PrefString(PreferenceTypes.PaceNotifier), (int)type);
        }

        private static string PrefString(PreferenceTypes type)
        {
            return $"Pref.{type.ToString()}";
        }
    }

    public static class UnitsTypeExtensions
    {
        public static string GetDistanceUnitString(this UnitsType type)
        {
            switch (type)
            {
                case UnitsType.Metric:
                    return "Kilometers";
                case UnitsType.Imperial:
                    return "Miles";
                default:
                    return string.Empty;
            }
        }
    }

    public enum UnitsType
    {
        Metric,
        Imperial
    }

    public enum PaceNotifierTypes
    {
        Unit,
        HalfUnit,
        Segment,
        HalfUnitAndSegment,
        UnitAndSegment
    }

    public enum PreferenceTypes
    {
        Weight,
        Units,
        Age,
        SegmentNotifier,
        PaceNotifier
    }
}
