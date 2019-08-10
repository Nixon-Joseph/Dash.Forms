using Android.App;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Support.Wearable.Activity;
using Android.Support.Wearable.Views;
using Android.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dash.Forms.AndroidWear
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/MainTheme.Launcher",
        Name = "com.DashFitness.AppBeta.RunActivity",
        MainLauncher = true)]
    public class RunActivity : WearableActivity, ISensorEventListener
    {
        private const string TAG = nameof(RunActivity);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_run);

            var mSensorManager = (SensorManager)GetSystemService(SensorService);
            var mHeartRateSensor = mSensorManager.GetDefaultSensor(SensorType.HeartRate);
            var mStepCountSensor = mSensorManager.GetDefaultSensor(SensorType.StepCounter);
            var mStepDetectSensor = mSensorManager.GetDefaultSensor(SensorType.StepDetector);

            mSensorManager.RegisterListener(this, mHeartRateSensor, SensorDelay.Normal);
            mSensorManager.RegisterListener(this, mStepCountSensor, SensorDelay.Normal);
            mSensorManager.RegisterListener(this, mStepDetectSensor, SensorDelay.Normal);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            Log.Debug(TAG, "onAccuracyChanged - accuracy: " + accuracy);
        }

        public void OnSensorChanged(SensorEvent e)
        {
            string message;
            switch (e.Sensor.Type)
            {
                case SensorType.HeartRate:
                    message = ((int)e.Values.First()).ToString();
                    // set heart rate label
                    Log.Debug(TAG, message);
                    break;
                case SensorType.StepCounter:
                    message = ((int)e.Values.First()).ToString();
                    Log.Debug(TAG, $"Count {message}");
                    // set step counter label
                    break;
                case SensorType.StepDetector:
                    Log.Debug(TAG, "Step Detected");
                    break;
                default:
                    Log.Debug(TAG, "Unknown sensor type");
                    break;
            }
        }
    }
}
