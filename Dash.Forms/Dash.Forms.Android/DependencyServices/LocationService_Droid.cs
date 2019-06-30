using Android;
using Android.Content.PM;
using Android.Locations;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Droid.BroadcastReceivers;
using Dash.Forms.Droid.DependencyServices;
using Dash.Forms.Droid.Extensions;
using Dash.Forms.Droid.Services;
using Dash.Forms.Models.Run;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

[assembly: Dependency(typeof(LocationService_Droid))]
namespace Dash.Forms.Droid.DependencyServices
{
    public class LocationService_Droid : ILocationService
    {
        private static LocationBroadcastReceiver _receiver;
        private static LocationService _service;

        public LocationService_Droid()
        {
            _service = _service ?? new LocationService();
            if (_receiver == null)
            {
                _receiver = new LocationBroadcastReceiver();
                MainActivity.Instance.RegisterReceiver(_receiver, new Android.Content.IntentFilter(Constants.Action.LOCATION_CHANGED));
            }
        }

        public void AddLocationChangedListener(Action<object, LocationData> listener)
        {
            _receiver.LocationChanged += (obj, e) => listener(obj, MainActivity.Mapper.Map<LocationData>(e.Location));
        }

        public void RemoveLocationChangedListener(Action<object, LocationData> listener)
        {
            _receiver.LocationChanged -= (obj, e) => listener(obj, MainActivity.Mapper.Map<LocationData>(e.Location));
        }

        public void Start()
        {
            MainActivity.Instance.BaseContext.StartForegroundServiceCompat<LocationService>(Constants.Action.START_SERVICE);
        }

        public void Stop()
        {
            MainActivity.Instance.BaseContext.StartForegroundServiceCompat<LocationService>(Constants.Action.STOP_SERVICE);
        }

        public bool CheckGPSPermission()
        {
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(MainActivity.Instance, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted)
            {
                return true;
            }
            else if (ActivityCompat.ShouldShowRequestPermissionRationale(MainActivity.Instance, Manifest.Permission.AccessFineLocation))
            {
                //Explain to the user why we need to read the contacts
                Snackbar.Make(MainActivity.Instance.FindViewById(Android.Resource.Id.Content), "Location access is required to track your runs.", Snackbar.LengthIndefinite)
                        .SetAction("OK", v => ActivityCompat.RequestPermissions(MainActivity.Instance, new string[] { Manifest.Permission.AccessFineLocation }, Constants.Permission.LOCATION_PERMISSION))
                        .Show();
            }
            else
            {
                ActivityCompat.RequestPermissions(MainActivity.Instance, new string[] { Manifest.Permission.AccessFineLocation }, Constants.Permission.LOCATION_PERMISSION);
            }
            return false;
        }

        public double GetDistance(Position p1, Position p2)
        {
            return GetDistance(p1.Latitude, p1.Longitude, p2.Latitude, p2.Longitude);
        }

        public double GetDistance(double lat1, double long1, double lat2, double long2)
        {
            var coords1 = new Location("")
            {
                Latitude = lat1,
                Longitude = long1
            };
            var coords2 = new Location("")
            {
                Latitude = lat2,
                Longitude = long2
            };
            return coords1.DistanceTo(coords2);
        }

        public LocationData GetQuickLocation()
        {
            var criteria = new Criteria();
            LocationManager locManager = MainActivity.Instance.GetSystemService(Android.Content.Context.LocationService) as LocationManager;
            var provider = locManager.GetBestProvider(criteria, true);
            var locData = locManager.GetLastKnownLocation(provider);
            return locData == null ? null : MainActivity.Mapper.Map<LocationData>(locData);
        }
    }
}
