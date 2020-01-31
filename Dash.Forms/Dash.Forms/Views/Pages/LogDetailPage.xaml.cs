using Dash.Forms.DependencyInterfaces;
using Dash.Forms.Extensions;
using Dash.Forms.Helpers;
using Dash.Forms.Models.Run;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
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
            try
            {
                InitializeComponent();

                double totalDistance = runData.Distance;

                foreach (var segment in runData.Segments.OrderBy(s => s.StartTime))
                {
                    foreach (var loc in segment.Locations.OrderBy(l => l.Timestamp))
                    {
                        RunMap.AddPosition(loc.GetPosition());
                    }
                }

                var (minLat, minLng, maxLat, maxLng) = RunMap.RouteCoordinates.GetMinsAndMaxes();
                var mapDistance = DependencyService.Get<ILocationService>().GetDistance(minLat.Value, minLng.Value, maxLat.Value, maxLng.Value);

                var useMiles = PreferenceHelper.GetUnits() == UnitsType.Imperial;
                var convertedDistance = useMiles ? (totalDistance.ToMiles()) : (totalDistance.ToKilometers());
                RunDistanceLabel.Text = convertedDistance.ToString("N2");
                StatsDistanceLabel.Text = convertedDistance.ToString("N2");
                if (convertedDistance > 0.1d)
                {
                    StatsPaceLabel.Text = TimeSpan.FromMinutes(runData.Elapsed.TotalMinutes / convertedDistance).ToString("mm':'ss");
                }
                else
                {
                    StatsPaceLabel.Text = "∞";
                }
                StatsCaloriesLabel.Text = ((int)RunHelper.CalculateCalories(totalDistance, PreferenceHelper.GetWeight())).ToString();
                MapTimeLabel.Text = runData.Elapsed.ToString(runData.Elapsed.Hours > 0 ? "hh':'mm':'ss" : "mm':'ss");
                StatsTimeLabel.Text = MapTimeLabel.Text;

                RunMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position((minLat.Value + maxLat.Value) / 2, (minLng.Value + maxLng.Value) / 2), Distance.FromKilometers(mapDistance / 800d)));
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>() { { "Location", "LogDetailPage.cs.Constructor" } });
                throw ex;
            }
        }
    }
}