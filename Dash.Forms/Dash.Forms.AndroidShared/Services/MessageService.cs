using Android.App;
using Android.Content;
using Android.Gms.Wearable;
using Android.Runtime;
using Android.Support.V4.Content;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Forms.AndroidShared.Services
{
    [Service]
    [IntentFilter(new string[] { "com.google.android.gms.wearable.MESSAGE_RECEIVED" }, DataScheme = "wear", DataHost = "*", DataPathPrefix = PATH)]
    public class MessageService : WearableListenerService
    {
        private const string PATH = "/my_path";

        public override void OnMessageReceived(IMessageEvent messageEvent)
        {
            if (messageEvent.Path.Equals(PATH))
            {
                string message = Encoding.UTF8.GetString(messageEvent.GetData());
                var messageIntent = new Intent();
                messageIntent.SetAction(Intent.ActionSend);
                messageIntent.PutExtra("message", message);

                LocalBroadcastManager.GetInstance(this).SendBroadcast(messageIntent);
            }
            else
            {
                base.OnMessageReceived(messageEvent);
            }
        }

        public static async void SendMessageToConnectedDevices(Context context, string message)
        {
            Task<JavaList<INode>> wearableList = WearableClass.GetNodeClient(context).GetConnectedNodesAsync();
            try
            {
                JavaList<INode> nodes = await wearableList;
                foreach (INode node in nodes)
                {
                    var sendMessageTask = WearableClass.GetMessageClient(context).SendMessage(node.Id, PATH, Encoding.UTF8.GetBytes(message));

                    try
                    {
                        //Block on a task and get the result synchronously//
                        sendMessageTask.Wait();
                        //if the Task fails, then…..//
                    }
                    catch (Exception exception)
                    {
                        //TO DO: Handle the exception//
                    }
                }
            }
            catch (Exception exception)
            {

            }
        }
    }
}
