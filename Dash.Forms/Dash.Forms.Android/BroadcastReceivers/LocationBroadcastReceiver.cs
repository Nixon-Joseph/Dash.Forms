using Android.App;
using Android.Content;
using Android.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Forms.Droid.BroadcastReceivers
{
    [IntentFilter(new[] { Constants.Action.LOCATION_CHANGED })]
    public class LocationBroadcastReceiver : BroadcastReceiver
    {
        public event EventHandler<LocationChangedEventArgs> LocationChanged;

        public override void OnReceive(Context context, Intent intent)
        {
            switch (intent.Action)
            {
                case Constants.Action.LOCATION_CHANGED:
                    LocationChanged?.Invoke(this, new LocationChangedEventArgs(intent.GetParcelableExtra(Constants.Extra.LOCATION_DATA) as Location));
                    break;
            }
        }
    }
}
