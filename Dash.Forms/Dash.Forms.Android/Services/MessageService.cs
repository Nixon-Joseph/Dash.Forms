using Android.App;
using Android.Content;

namespace Dash.Forms.Droid.Services
{
    [Service]
    [IntentFilter(new string[] { "com.google.android.gms.wearable.MESSAGE_RECEIVED" }, DataScheme = "wear", DataHost = "*", DataPathPrefix = Constants.WEARABLE_MESSAGE_PATH)]
    public class MessageService : AndroidShared.Services.MessageService { }
}
