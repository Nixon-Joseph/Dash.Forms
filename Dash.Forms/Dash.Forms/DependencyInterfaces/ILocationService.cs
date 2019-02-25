using Dash.Forms.Models.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Dash.Forms.DependencyInterfaces
{
    public interface ILocationService
    {
        void AddLocationChangedListener(Action<object, LocationData> listener);
        void RemoveLocationChangedListener(Action<object, LocationData> listener);
        void Start();
        void Stop();
        bool CheckGPSPermission();
        double GetDistance(Position p1, Position p2);
        double GetDistance(double lat1, double long1, double lat2, double long2);
    }
}
