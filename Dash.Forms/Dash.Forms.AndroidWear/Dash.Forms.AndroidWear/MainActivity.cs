using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.Wearable.Activity;
using Android.Widget;
using Dash.Forms.AndroidShared;
using Dash.Forms.AndroidShared.Interfaces;
using Dash.Forms.AndroidShared.Receivers;
using System;

namespace Dash.Forms.AndroidWear
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/MainTheme.Launcher",
        Name = "com.DashFitness.AppBeta.MainActivity",
        MainLauncher = true)]
    public class MainActivity : WearableActivity, IActivityMessageReceiver
    {
        TextView textView;

        protected override void OnCreate(Bundle bundle)
        {
            SetTheme(Resource.Style.MainTheme);

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
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(new WearableMessageReceiver(this), newFilter);
        }

        private async void Button_Click(object sender, EventArgs e)
        {
            _ = await NodeManager.SendMessageToNodes(this, "test message");
        }

        public void OnMessageReceive(string message)
        {
            textView.SetText(message, TextView.BufferType.Normal);
        }
    }
}


