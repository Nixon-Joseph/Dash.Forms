using Android.App;
using Android.Content;
using Android.Gms.Wearable;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.Wearable.Activity;
using Android.Widget;
using Google.Android.Wearable.Intent;
using System;
using System.Text;

namespace Dash.Forms.AndroidWear
{
    [Activity(Label = "@string/app_name", MainLauncher = true)]
    public class MainActivity : WearableActivity
    {
        TextView textView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (Build.Model == "Ticwatch2-i18n")
            {
                SetContentView(Resource.Layout.activity_main_ticwatch);
            }
            else
            {
                SetContentView(Resource.Layout.activity_main_boxview);
            }
            textView = FindViewById<TextView>(Resource.Id.text);

            if (Build.Model == "Ticwatch2-i18n")
            {
                textView.SetText("this is different text!", TextView.BufferType.Normal);
            }

            var button = FindViewById<Button>(Resource.Id.test_button);
            button.Click += Button_Click;

            SetAmbientEnabled();

            IntentFilter newFilter = new IntentFilter(Intent.ActionSend);
            MessageReceiver messageReceiver = new MessageReceiver(this);
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(messageReceiver, newFilter);

            //RemoteIntent.StartRemoteActivity(this, );
        }

        private void Button_Click(object sender, EventArgs e)
        {
            SendMessageToNodes("test message");
        }

        public async void SendMessageToNodes(string message)
        {
            try
            {
                using (var nodes = await WearableClass.GetNodeClient(this).GetConnectedNodesAsync())
                {
                    foreach (INode node in nodes)
                    {
                        try
                        {
                            var sendMessageTask = await System.Threading.Tasks.Task.Run(() => Android.Gms.Tasks.TasksClass.Await(WearableClass.GetMessageClient(this).SendMessage(node.Id, Constants.WEARABLE_MESSAGE_PATH, Encoding.UTF8.GetBytes(message))));
                        }
                        catch (Exception exception)
                        {
                            //TO DO: Handle the exception//
                        }
                    }
                }
            }
            catch (Exception exception)
            {

            }
        }

        private class MessageReceiver : BroadcastReceiver
        {
            private readonly MainActivity Activity;

            public MessageReceiver(MainActivity activity)
            {
                Activity = activity;
            }

            public override void OnReceive(Context context, Intent intent)
            {
                var message = intent.GetStringExtra("message");
                Activity.textView.SetText(message, TextView.BufferType.Normal);
            }
        }
    }
}


