using Android.App;
using Android.Content;
using Android.Gms.Wearable;
using Android.Support.V4.Content;
using System.Text;

namespace Dash.Forms.AndroidWear.Services
{
    [Service]
    [IntentFilter(new string[] { "com.google.android.gms.wearable.MESSAGE_RECEIVED" }, DataScheme = "wear", DataHost = "*", DataPathPrefix = Constants.WEARABLE_MESSAGE_PATH)]
    public class MessageService : WearableListenerService
    {
        public override void OnMessageReceived(IMessageEvent p0)
        {
            if (p0.Path.Equals(Constants.WEARABLE_MESSAGE_PATH))
            {
                var message = Encoding.UTF8.GetString(p0.GetData());
                var messageIntent = new Intent();
                messageIntent.SetAction(Intent.ActionSend);
                messageIntent.PutExtra("message", message);

                LocalBroadcastManager.GetInstance(this).SendBroadcast(messageIntent);
            }
            else
            {
                base.OnMessageReceived(p0);
            }
        }
    }
}
