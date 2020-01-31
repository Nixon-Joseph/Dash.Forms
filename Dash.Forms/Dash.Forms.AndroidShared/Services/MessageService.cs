using Android.Content;
using Android.Gms.Wearable;
using Android.Support.V4.Content;
using System.Text;

namespace Dash.Forms.AndroidShared.Services
{
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
