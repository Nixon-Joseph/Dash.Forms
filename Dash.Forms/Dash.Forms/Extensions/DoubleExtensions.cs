using Dash.Forms.Helpers;

namespace Dash.Forms.Extensions
{
    public static class DoubleExtensions
    {
        public static double ToKilometers(this double meters)
        {
            return meters / Constants.Distances.MetersInKilometer;
        }

        public static double ToMiles(this double meters)
        {
            return meters / Constants.Distances.MetersInMile;
        }

        public static double ToKilos(this double lbs)
        {
            return lbs / 2.205;
        }

        public static double ToPounds(this double kilos)
        {
            return kilos * 2.205;
        }

        public static double ConvertMeters(this double meters, UnitsType type)
        {
            if (type == UnitsType.Imperial)
            {
                return meters.ToMiles();
            }
            else
            {
                return meters.ToKilometers();
            }
        }
    }
}
