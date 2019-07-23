using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Extensions;
using Dash.Forms.Helpers;
using Dash.Forms.Models.Run;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Dash.Forms.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogDetailPage : TabbedPage
    {
        public LogDetailPage(RunData runData)
        {
            InitializeComponent();

            double totalDistance = 0d;

            foreach (var segment in runData.Segments.OrderBy(s => s.StartTime))
            {
                totalDistance += segment.Distance;
                foreach (var loc in segment.Locations.OrderBy(l => l.Timestamp))
                {
                    RunMap.AddPosition(loc.GetPosition());
                }
            }

            var (minLat, minLng, maxLat, maxLng) = RunMap.RouteCoordinates.GetMinsAndMaxes();
            var mapDistance = DependencyService.Get<ILocationService>().GetDistance(minLat.Value, minLng.Value, maxLat.Value, maxLng.Value);

            var useMiles = true;
            var convertedDistance = useMiles ? (totalDistance.ToMiles()) : (totalDistance.ToKilometers());
            RunDistanceLabel.Text = convertedDistance.ToString("N2");
            StatsDistanceLabel.Text = convertedDistance.ToString("N2");
            StatsPaceLabel.Text = TimeSpan.FromMinutes(runData.Elapsed.TotalMinutes / convertedDistance).ToString("mm':'ss");
            StatsCaloriesLabel.Text = ((int)RunHelper.CalculateCalories(totalDistance, PreferenceHelper.GetWeight())).ToString();
            MapTimeLabel.Text = runData.Elapsed.ToString(runData.Elapsed.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");
            StatsTimeLabel.Text = MapTimeLabel.Text;

            RunMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position((minLat.Value + maxLat.Value) / 2, (minLng.Value + maxLng.Value) / 2), Distance.FromKilometers(mapDistance / 800d)));
        }
    }
}