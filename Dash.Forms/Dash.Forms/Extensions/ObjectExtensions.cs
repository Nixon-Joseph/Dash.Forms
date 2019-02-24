using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Dash.Forms.Extensions
{
    public static class ObjectExtensions
    {
        public static Position GetPosition(this LocationData locData)
        {
            return new Position(locData.Latitude, locData.Longitude);
        }
    }
}
