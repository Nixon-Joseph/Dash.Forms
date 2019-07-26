using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Maps;

namespace Dash.Forms.Extensions
{
    public static class ObjectExtensions
    {
        public static Position GetPosition(this LocationData locData)
        {
            return new Position(locData.Latitude, locData.Longitude);
        }

        public static (double? minLat, double? minLng, double? maxLat, double? maxLng) GetMinsAndMaxes(this IEnumerable<Position> positions)
        {
            double? minLat = null, minLng = null, maxLat = null, maxLng = null;
            if (positions != null && positions.Count() > 0)
            {
                foreach (var position in positions)
                {
                    minLat = Math.Min(minLat ?? position.Latitude, position.Latitude);
                    minLng = Math.Min(minLng ?? position.Longitude, position.Longitude);
                    maxLat = Math.Max(maxLat ?? position.Latitude, position.Latitude);
                    maxLng = Math.Max(maxLng ?? position.Longitude, position.Longitude);
                }
            }
            return (minLat, minLng, maxLat, maxLng);
        }

        public static double GetTotalElevationChange(this IEnumerable<LocationData> locs)
        {
            double? elevationMin = null;
            double? elevationMax = null;
            if (locs != null && locs.Count() > 0)
            {
                foreach (var loc in locs)
                {
                    if (loc.HasAltitude == true)
                    {
                        elevationMax = Math.Max(elevationMax ?? loc.Altitude, loc.Altitude);
                        elevationMin = Math.Min(elevationMin ?? loc.Altitude, loc.Altitude);
                    }
                }
            }
            return (elevationMax ?? 0d) - (elevationMin ?? 0d);
        }

        public static string ToOptionalHourString(this TimeSpan span)
        {
            return span.ToString(span.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");
        }
    }
}
