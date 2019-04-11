using Android.Content;
using Android.Gms.Maps.Model;
using Dash.Forms.Controls;
using Dash.Forms.Droid.Renderers;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

// reference for for building on iOS
//https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/map/polyline-map-overlay

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Dash.Forms.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        List<Position> routeCoordinates;
        private bool _isMapReady = false;
        private int _mapLineColor = 0x66FF0000;

        public CustomMapRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement is CustomMap oldFormsMap)
            {
                oldFormsMap.AddedPosition -= FormsMap_AddedPosition;
            }

            if (e.NewElement is CustomMap formsMap)
            {
                routeCoordinates = formsMap.RouteCoordinates.ToList();
                formsMap.AddedPosition += FormsMap_AddedPosition;
                Control.GetMapAsync(this);
            }
        }

        private void FormsMap_AddedPosition(object sender, NewPositionEventArgs e)
        {
            if (_isMapReady == true && routeCoordinates.Count() > 0 && e.ShowPath == true && routeCoordinates.Last() is Position lastPos)
            {
                if (NativeMap != null)
                {
                    var polylineOptions = new PolylineOptions();
                    polylineOptions.InvokeColor(_mapLineColor);
                    polylineOptions.Add(new LatLng(lastPos.Latitude, lastPos.Longitude));
                    polylineOptions.Add(new LatLng(e.Position.Latitude, e.Position.Longitude));
                    NativeMap.AddPolyline(polylineOptions);
                }
            }
            routeCoordinates.Add(e.Position);
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);
            _isMapReady = true;

            if (NativeMap != null)
            {
                var polylineOptions = new PolylineOptions();
                polylineOptions.InvokeColor(_mapLineColor);

                foreach (var position in routeCoordinates)
                {
                    polylineOptions.Add(new LatLng(position.Latitude, position.Longitude));
                }

                NativeMap.AddPolyline(polylineOptions);
                NativeMap.UiSettings.MyLocationButtonEnabled = false;
            }
        }
    }
}
