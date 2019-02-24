using Android.App;
using Android.Content;
using Android.OS;

namespace Dash.Forms.Droid.Extensions
{
    public static class ContextExtensions
    {
        public static void StartForegroundServiceCompat<T>(this Context context, Bundle args = null) where T : Service
        {
            var intent = new Intent(context, typeof(T));

            if (args != null)
            {
                intent.PutExtras(args);
            }

            context.StartForegroundServiceCompat<T>(intent);
        }

        public static void StartForegroundServiceCompat<T>(this Context context, string action)
        {
            var intent = new Intent(context, typeof(T));

            if (string.IsNullOrEmpty(action) == false)
            {
                intent.SetAction(action);
            }

            context.StartForegroundServiceCompat<T>(intent);
        }

        public static void StartForegroundServiceCompat<T>(this Context context, Intent intent)
        {
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }
    }
}
