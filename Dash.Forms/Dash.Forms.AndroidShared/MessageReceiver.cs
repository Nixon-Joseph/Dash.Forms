using Android.Content;

namespace Dash.Forms.AndroidShared
{
    public class MessageReceiver : BroadcastReceiver
    {
        private readonly IBroadcastReceiverActivity Activity;

        public MessageReceiver(IBroadcastReceiverActivity activity)
        {
            Activity = activity;
        }

        public override void OnReceive(Context context, Intent intent)
        {
            Activity.OnRecieve(context, intent.GetStringExtra("message"));
        }
    }

    public interface IBroadcastReceiverActivity
    {
        void OnRecieve(Context context, string message);
    }
}
