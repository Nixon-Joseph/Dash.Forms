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
    }
}
