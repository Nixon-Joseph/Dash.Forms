using Android.Content;
using Dash.Forms.AndroidShared.Interfaces;

namespace Dash.Forms.AndroidShared.Receivers
{
    public class WearableMessageReceiver : BroadcastReceiver
    {
        private readonly IActivityMessageReceiver Activity;

        public WearableMessageReceiver(IActivityMessageReceiver activity)
        {
            Activity = activity;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            Activity.OnMessageReceive(intent.GetStringExtra("message"));
        }
    }
}
